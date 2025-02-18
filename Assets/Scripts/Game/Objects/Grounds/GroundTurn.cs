using UnityEngine;

namespace Game.Objects.Grounds
{
    public class GroundTurn : MonoBehaviour
    {
        public GameObject gateFirst;
        public GameObject gateSecond;

        public bool isActive = true;

        private void Start()
        {
            gateFirst = transform.Find("GateFirst").gameObject;
            gateSecond = transform.Find("GateSecond").gameObject;
        }

        public float GetTurn(bool fromFirst) => GetTurnForGate(fromFirst);

        public float GetTurnForGate(bool fromFirst)
        {
            Vector3 gateF = fromFirst ? gateFirst.transform.position : gateSecond.transform.position;
            Vector3 gateS = fromFirst ? gateSecond.transform.position : gateFirst.transform.position;

            int xFDiff = GetNormalized(transform.position.x - gateF.x);
            int zFDiff = GetNormalized(transform.position.z - gateF.z);
            int xSDiff = GetNormalized(transform.position.x - gateS.x);
            int zSDiff = GetNormalized(transform.position.z - gateS.z);

            float turnType1 = -90f;
            float turnType2 = 90f;
            if(xFDiff == 1)
                return zSDiff < 0 ? turnType1 : turnType2;
            if(xFDiff == -1)
                return zSDiff > 0 ? turnType1 : turnType2;
            if(zFDiff == 1)
                return xSDiff > 0 ? turnType1 : turnType2;
            if(zFDiff == -1)
                return xSDiff < 0 ? turnType1 : turnType2;

            return 0f;
        }

        public int GetNormalized(float value)
        {
            if (value > 0 && value < 1)
                value = 0;
            if (value < 0 && value > -1)
                value = 0;

            if (value == 0)
                return (int)value;
            return value > 0 ? 1 : -1;
        }
    }
}
