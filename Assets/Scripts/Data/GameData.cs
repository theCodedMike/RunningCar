using System;
using System.Utility;
using Interface.Indicator;
using UnityEngine;

namespace Data
{
    public class GameData : MonoBehaviour
    {
        [Header("分数")]
        public int score = 0;
        public int rating = 0;

        [Header("游戏属性")]
        public GameVariable playerSpeed;
        
        private ObjectsHolder _objectsHolder;
        private IndicatorsController _indicatorsController;


        private void Awake()
        {
            _objectsHolder = GameObject.Find("ObjectsHolder").GetComponent<ObjectsHolder>();
            _indicatorsController = _objectsHolder.indicatorsController;
        }

        public void ChangeGameValues() => playerSpeed.ChangeValue();

        public void IncrementScore(int value)
        {
            score += value;
            if(_indicatorsController != null)
                _indicatorsController.UpdateIndicatorScore();
        }

        public void IncrementCoin(int value)
        {
            DataOperations.ChangeCoin(value);
            if(_indicatorsController != null)
                _indicatorsController.UpdateIndicatorCoins();
        }

        public void IncrementRating()
        {
            rating++;
            _objectsHolder.indicatorStars.SetStar(rating);
        }
    }
}
