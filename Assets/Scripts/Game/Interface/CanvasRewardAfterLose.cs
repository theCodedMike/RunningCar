//using System.MobileFeatures.Ad;
using Data;
using Game.Game;
using UnityEngine;

namespace Game.Interface
{
    public class CanvasRewardAfterLose : MonoBehaviour
    {
        public bool testMode;
        
        private ObjectsHolder _objectsHolder;


        private void Start()
        {
            _objectsHolder = GameObject.Find("ObjectsHolder").GetComponent<ObjectsHolder>();
        }

        
        #region Usage
        
        public void Watch()
        {
            if(testMode)
                GameObject.Find("Game").GetComponent<GameController>().ContinueGame();
            /*else
                _objectsHolder.adsInterface.ShowReward(AdRewardType.RewardAfterLose);*/
        }

        public void Cancel() => _objectsHolder.gameController.GameOver();
        #endregion
    }
}
