using System.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Interface.UI_Components
{
    public class Switch : MonoBehaviour
    {
        private Text _text;
        private Image _background;
        private Image _pin;
        private Animation _pinAnimation;

        private bool _isInitializationState = true;

        enum Mode
        {
            On, Off
        }

        private Mode _mode = Mode.On;
        private float _positionPinDefault = 23.17f;
        private float _positionPin = 25.0f;
        
        public float speed = 5.0f;
        public string value = string.Empty;
        public UnityEvent valueChanged;

        public Sprite spriteBackgroundOn;
        public Sprite spriteBackgroundOff;


        private void Start()
        {
            GetComponents();
            LoadState();
        }

        private void FixedUpdate()
        {
            MovePin();
        }

        private void GetComponents()
        {
            _text = transform.Find("Text").GetComponent<Text>();
            _background = GetComponent<Image>();
            _pin = transform.Find("Pin").GetComponent<Image>();
            _pinAnimation = _pin.gameObject.GetComponent<Animation>();

            _positionPinDefault = _pin.GetComponent<RectTransform>().localPosition.x;
        }


        
        #region Work with state
        private void LoadState()
        {
            _mode = CustomPlayerPrefs.GetBool(value, true) ? Mode.On : Mode.Off;
            RunPinAnimation(_mode);
            ChangeStateTo(_mode);
            MoveInstant();
        }

        private void SaveState()
        {
            CustomPlayerPrefs.SetBool(value, _mode == Mode.On);
        }

        public void ChangeState()
        {
            ChangeStateTo(_mode == Mode.On ? Mode.Off : Mode.On);
        }

        private void ChangeStateTo(Mode newMode)
        {
            if (_mode != newMode || _isInitializationState)
            {
                _mode = newMode;
                RunPinAnimation(_mode);
                switch (_mode)
                {
                    case Mode.On:
                        _positionPin = _positionPinDefault;
                        _background.sprite = spriteBackgroundOn;
                        break;
                    case Mode.Off:
                        _positionPin = -_positionPinDefault;
                        _background.sprite = spriteBackgroundOff;
                        break;
                }

                SaveState();
                if (!_isInitializationState)
                    if (valueChanged != null)
                        valueChanged.Invoke();

                _isInitializationState = false;
            }
        }

        #endregion

        #region Animation

        private void RunPinAnimation(Mode withMode)
        {
            switch (withMode)
            {
                case Mode.On:
                    _pinAnimation.clip = _pinAnimation.GetClip("SwitchPin_AnimationFadeOn");
                    break;
                case Mode.Off:
                    _pinAnimation.clip = _pinAnimation.GetClip("SwitchPin_AnimationFadeOff");
                    break;
            }
            _pinAnimation.Play();
        }

        private void MovePin()
        {
            _pin.transform.localPosition = Vector3.MoveTowards(_pin.transform.localPosition,
                new Vector3(_positionPin, 0.0f, 0.0f),
                Time.deltaTime * speed);
        }

        private void MoveInstant()
        {
            _pin.transform.localPosition = new Vector3(_positionPin, 0.0f, 0.0f);
        }

        #endregion
    }
}
