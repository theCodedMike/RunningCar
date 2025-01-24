using System.Utility;
using Data;
using Game.Game;
using UnityEngine;

namespace Game.Objects.Grounds
{
    public class GroundSaw : MonoBehaviour
    {
        private ObjectsHolder _objectsHolder;

        [Header("Objects")]
        public GameObject sawPositionFirst;
        public GameObject sawPositionSecond;
        [Space()]
        public GameObject saw;

        [Header("Values")]
        public float speed = 50f;
        [HideInInspector]
        public bool isMoveToFirst = true;

        private void Awake()
        {
            _objectsHolder = GameObject.Find("ObjectsHolder").GetComponent<ObjectsHolder>();

            speed = _objectsHolder.levelHolder.ReturnRunnedLevel().sawSpeed;
            isMoveToFirst = RandomBool.GetRandom();
        }

        private void FixedUpdate()
        {
            Move();
            CheckIsReashDistance();
        }

        private void Move()
        {
            if (_objectsHolder.gameModel.state != GameState.Game)
                return;
            
            saw.transform.position = Vector3.MoveTowards(saw.transform.position, GetMoveTo(), speed * Time.fixedDeltaTime);
        }

        private void CheckIsReashDistance()
        {
            if (saw.transform.position == GetMoveTo())
                isMoveToFirst = !isMoveToFirst;
        }

        private Vector3 GetMoveTo() =>
            isMoveToFirst ? sawPositionFirst.transform.position : sawPositionSecond.transform.position;
    }
}
