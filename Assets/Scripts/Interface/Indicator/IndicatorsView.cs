using System.Utility;
using UnityEngine;

namespace Interface.Indicator
{
    public class IndicatorsView : MonoBehaviour
    {
        private IndicatorsModel _model;

        private void Awake()
        {
            _model = GetComponent<IndicatorsModel>();
        }

        #region Update values

        public void UpdateTextScore()
        {
            _model.textScore.text = _model.defaultStringScore + _model.gameData.score;
        }

        public void UpdateTextResultScore()
        {
            int value = CustomPlayerPrefs.GetInt("scoreResult", 0);
            
            _model.textScoreResult.text = value == 0 ? "" : _model.defaultStringScoreResult + value;
            
        }

        public void UpdateTextResultBestScore()
        {
            int highScore = CustomPlayerPrefs.GetInt("highScore", 0);
            _model.textScoreResultBest.text = _model.defaultStringScoreResultBest + highScore;
        }

        public void UpdateTextCoin()
        {
            int coin = CustomPlayerPrefs.GetInt("coin", 0);
            _model.textCoin.text = _model.defaultStringCoin + coin;
        }
        #endregion
    }
}
