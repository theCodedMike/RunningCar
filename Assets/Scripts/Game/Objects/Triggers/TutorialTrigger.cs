using Data;
using UnityEngine;

namespace Game.Objects.Triggers
{
    public class TutorialTrigger : MonoBehaviour
    {
        public string title;
        public string message;
        public Sprite icon;
        
        private ObjectsHolder _objectsHolder;
        private bool _isActive = true;


        private void Start()
        {
            _objectsHolder = GameObject.Find("ObjectsHolder").GetComponent<ObjectsHolder>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isActive)
                return;
            if (other.CompareTag("Player"))
            {
                _isActive = false;
                _objectsHolder.gameController.LoadTutorial(title, message, icon);
            }
        }
    }
}
