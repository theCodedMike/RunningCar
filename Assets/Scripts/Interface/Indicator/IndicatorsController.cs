using UnityEngine;

namespace Interface.Indicator
{
    public class IndicatorsController : MonoBehaviour
    {
        public static IndicatorsController Instance { get; private set;}
        
        private IndicatorsModel _model;
        private IndicatorsView _view;

        private void Awake()
        {
            _model = GetComponent<IndicatorsModel>();
            _view = GetComponent<IndicatorsView>();
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            UpdateIndicators();
        }

        private void UpdateIndicators()
        {
            UpdateIndicatorCoins();
            UpdateIndicatorScore();
            if(_model.textScoreResultBest != null)
                _view.UpdateTextResultBestScore();
            if(_model.textCoin != null)
                _view.UpdateTextCoin();
        }

        public void UpdateIndicatorCoins()
        {
            if (_model.textCoin != null)
                _view.UpdateTextCoin();
        }

        public void UpdateIndicatorScore()
        {
            if(_model.textScore != null)
                _view.UpdateTextScore();
        }
    }
}
