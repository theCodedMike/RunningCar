using Data;
using Game.Game;
using UnityEngine;

namespace Game.Camera
{
    public class GameCamera : MonoBehaviour
    {
        [Header("Objects")]
        public GameObject target;
        [Space()]
        public GameObject[] attachedObjects;

        [Header("Following Options")]
        public bool followX = false;
        public bool followY = true;
        public bool followZ = false;
        public float speed = 90f;

        private ObjectsHolder _objectsHolder;
        private Vector3 _defaultTargetPosition;
        private Vector3 _defaultCameraPosition;

        #region System
        private void Start()
        {
            _objectsHolder = GameObject.Find("ObjectsHolder").GetComponent<ObjectsHolder>();
            if (_objectsHolder.gameModel.isTutorial) 
                SetTarget(target);
        }
        private void FixedUpdate()
        {
            Move();
        }
        #endregion


        public void SetTarget(GameObject target)
        {
            this.target = target;
            _defaultTargetPosition = target.transform.position;
            _defaultCameraPosition = transform.position;
        }

        private void Move()
        {
            if (target == null)
                return;
            if (_objectsHolder.gameModel.state != GameState.Game && _objectsHolder.gameModel.state != GameState.PreGamePause)
                return;

            if (followX || followY || followZ)
            {
                float x = followX
                    ? target.transform.position.x - _defaultTargetPosition.x + _defaultCameraPosition.x
                    : transform.position.x;
                float y = followY
                    ? target.transform.position.y - _defaultTargetPosition.y + _defaultCameraPosition.y
                    : transform.position.y;
                float z = followX
                    ? target.transform.position.z - _defaultTargetPosition.z + _defaultCameraPosition.z
                    : transform.position.z;
                Vector3 moveTo = new Vector3(x, y, z);

                transform.position = Vector3.MoveTowards(transform.position, moveTo, Time.fixedDeltaTime * speed);
                MoveAttached();
            }
        }

        private void MoveAttached()
        {
            foreach (var obj in attachedObjects)
            {
                obj.transform.position = transform.position;
            }
        }
    }
}
