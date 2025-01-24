using System.Audio.Sound;
using System.Audio.Sound.Audio;
using System.DailyTask;
using Data;
using UnityEngine;

namespace Game.Objects
{
    public class Coin : MonoBehaviour
    {
        private ObjectsHolder _objectsHolder;
        private bool _isActive = true;
        
        public GameObject particleCollect;


        private void Start()
        {
            _objectsHolder = GameObject.Find("ObjectsHolder").GetComponent<ObjectsHolder>();
        }

        public void Collect()
        {
            if (!_isActive)
                return;
            _isActive = false;
            
            _objectsHolder.gameData.IncrementCoin(1);
            _objectsHolder.dailyTaskManager.IncreaseProgress(DailyTaskType.CollectCoins, 1);
            CreateParticleCollect();
            Sound.Play(SoundType.Coin);
            
            Destroy(gameObject);
        }

        public void CreateParticleCollect()
        {
            if (particleCollect == null)
                return;

            GameObject particle = Instantiate(particleCollect);
            particle.transform.position = transform.position;
            particle.transform.localScale = Vector3.one;
        }
    }
}
