using Game.Objects.Grounds;
using UnityEngine;

namespace Game.Objects
{
    public class Gate : MonoBehaviour
    {
        public GroundTurn groundTurn;

        public bool isFirst;


        private void Start()
        {
            groundTurn = transform.parent.GetComponent<GroundTurn>();
        }

        public float GetTurn() => groundTurn.GetTurn(isFirst);

        public bool IsActive()
        {
            if (groundTurn.isActive)
            {
                groundTurn.isActive = false;
                return true;
            }

            return false;
        }
    }
}
