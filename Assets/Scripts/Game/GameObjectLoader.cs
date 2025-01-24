using Data;
using Game.Camera;
using UnityEngine;

namespace Game
{
    public class GameObjectLoader : MonoBehaviour
    {
        public GameObject player;
        public GameCamera gameCamera;
        
        private ObjectsHolder _objectsHolder;


        public void Load()
        {
            _objectsHolder = GameObject.Find("ObjectsHolder").GetComponent<ObjectsHolder>();
            SetPlayer();
        }

        public void SetPlayer()
        {
            if (_objectsHolder.levelLoader.playerStartCurrent != null)
            {
                Transform startObject = _objectsHolder.levelLoader.playerStartCurrent.transform.Find("PlayerStart").transform;
                GameObject obj = Instantiate(player);
                obj.transform.position = startObject.position;
                obj.transform.eulerAngles = startObject.eulerAngles;
                
                gameCamera.SetTarget(obj);
                Destroy(startObject.gameObject);
            }
        }
    }
}
