using System;
using System.Audio.Sound;
using System.Audio.Sound.Audio;
using System.MobileFeatures.Ad;
using System.Utility;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.DailyReward
{
    enum ViewState
    {
        Waiting, Ready
    }
    public class DailyRewardScene : MonoBehaviour
    {
        [Header("组件")]
        private SettingsHolder _settingsHolder;
        public ObjectsHolder objectsHolder;
        [Space()]
        public DailyRewardSegment[] segments;
        [Space()]
        public GameObject wheel;
        public GameObject canvasReward;

        public Text coinRewardValue;

        [Space()]
        public GameObject textSpin;
        public GameObject iconLock;

        [Space()]
        public GameObject panelTime;
        public TextMeshProUGUI textTime;

        [Header("Values")]
        public DailyRewardData[] data;

        private ViewState _state = ViewState.Waiting;
        private bool _isSpin = false;
        private bool _isGotReward = false;
        private const int SegmentsCount = 8;
        private float _segmentAngle;

        public FromToFloat time;
        private float _currentTime;

        public float speed = 40f;
        private float _currentSpeed = 0f;

        public float velocity = 0.01f;

        public float timeTick = 0.001f;
        private float _currentTimeTick = 0f;

        private int _currentReward = 0;

        private float _soundSpinPosition = 0f;
        private bool _soundSpinLock = false;


        private void Start()
        {
            _settingsHolder = SettingsHolder.Instance;
            data = _settingsHolder.settingsGame.dailyRewardDatas;
            _segmentAngle = 360f / SegmentsCount;
            LoadSegments();
            LoadState();
        }

        private void Update()
        {
            CalculateSpinSound();
        }

        private void FixedUpdate()
        {
            TryToSpinWheel();
            UpdateTimer();
        }



        #region Wheel
        public void Spin()
        {
            if (_state != ViewState.Ready)
                return;
            if (_isSpin)
                return;

            _currentTime = time.GetRandom();
            _isSpin = true;
        }
        
        public void TryToSpinWheel()
        {
            if (!_isSpin)
                return;

            // Change Values
            _currentTime -= Time.fixedDeltaTime;
            _currentTimeTick -= Time.fixedDeltaTime;

            if (_currentTime > 0)
            {
                if (_currentTimeTick <= 0)
                {
                    _currentTimeTick = timeTick;
                    _currentSpeed += velocity;

                    if (_currentSpeed > speed)
                        _currentSpeed = speed;
                }
            }
            else
            {
                if(_currentTimeTick <= 0)
                    _currentSpeed -= velocity;
                if (_currentSpeed <= 0)
                {
                    _isSpin = false;
                    CalculateReward();
                    return;
                }
            }
            
            // Spin wheel
            wheel.transform.Rotate(Vector3.forward, Time.fixedDeltaTime * _currentSpeed);
        }

        public void CalculateSpinSound()
        {
            if(!_isSpin)
                return;
            float currentAngle = wheel.transform.eulerAngles.z;
            if (_soundSpinLock)
            {
                if (currentAngle >= (360f - _segmentAngle))
                    return;
                
                _soundSpinLock = false;
            }

            if (currentAngle >= _soundSpinPosition)
            {
                _soundSpinPosition += _segmentAngle;
                if (_soundSpinPosition >= 360f)
                {
                    _soundSpinLock = true;
                    _soundSpinPosition = 0f;
                }
                
                Sound.Play(SoundType.DailyRewardWheel);
            }
        }
        
        public void CalculateReward()
        {
            float currentAngle = wheel.transform.eulerAngles.z;
            float limit = 0f;
            foreach (DailyRewardData rewardData in data)
            {
                if (limit + _segmentAngle > currentAngle)
                {
                    if (rewardData.type == DailyRewardType.Coin)
                    {
                        _currentReward = rewardData.value;
                        if(_settingsHolder.settingsAds.showDoubleCoinsAtDailyReward)
                            ShowCanvasReward();
                        else
                            GetReward();
                    }
                    else
                    {
                        StartingToWait();
                        Sound.Play(SoundType.Error);
                    }
                }

                limit += _segmentAngle;
            }
        }
        #endregion

        

        #region PopUp
        public void ShowCanvasReward()
        {
            canvasReward.SetActive(true);
            coinRewardValue.text = _currentReward.ToString();
            Sound.Play(SoundType.Successful);
        }

        public void HideCanvasReward()
        {
            canvasReward.SetActive(false);
            StartingToWait();
        }

        public void GetReward()
        {
            IncreaseCoins(_currentReward);
            HideCanvasReward();
        }

        public void DoubleReward() => objectsHolder.adsInterface.ShowReward(AdRewardType.DailyReward);

        public void ReceiveDoubleReward()
        {
            IncreaseCoins(_currentReward * 2);
            HideCanvasReward();
        }

        public void IncreaseCoins(int value)
        {
            if (_isGotReward)
                return;
            _isGotReward = true;

            int coins = CustomPlayerPrefs.GetInt("coin");
            CustomPlayerPrefs.SetInt("coin", coins + value);
            
            objectsHolder.indicatorsController.UpdateIndicatorCoins();
            Sound.Play(SoundType.Coin);
        }
        #endregion

        
        
        
        
        
        #region Date
        public void LoadState()
        {
            _state = DailyReward.IsAvailable() ? ViewState.Ready : ViewState.Waiting;
            
            UpdateView();
        }

        public void StartingToWait()
        {
            _state = ViewState.Waiting;
            CustomPlayerPrefs.SetString("dailyReward_Completed", DailyReward.GetDate());
            UpdateView();
        }
        private string GetTimeToEndOfDay()
        {
            DateTime date = DateTime.UtcNow;
            DateTime dateNext = date.Date.AddDays(1);
            TimeSpan remaining = dateNext - date;
            
            string hours = remaining.Hours < 10 ? $"0{remaining.Hours}" : remaining.Hours.ToString();
            string minutes = remaining.Minutes < 10 ? $"0{remaining.Minutes}" : remaining.Minutes.ToString();
            string seconds = remaining.Seconds < 10 ? $"0{remaining.Seconds}" : remaining.Seconds.ToString();

            return $"{hours}:{minutes}:{seconds}";
        }
        #endregion
        
        
        
        
        #region View
        public void UpdateView()
        {
            switch (_state)
            {
                case ViewState.Ready: iconLock.SetActive(false); panelTime.SetActive(false); textSpin.SetActive(true);
                    break;
                default: iconLock.SetActive(true); panelTime.SetActive(true); textSpin.SetActive(false);
                    break;
            }
        }

        public void LoadSegments()
        {
            for (int i = 0; i < segments.Length; i++)
                segments[i].Load(data[i]);
        }

        public void UpdateTimer()
        {
            if (_state != ViewState.Waiting)
                return;

            textTime.text = GetTimeToEndOfDay();
        }
        #endregion
    }
}
