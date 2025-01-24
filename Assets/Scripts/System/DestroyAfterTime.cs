using UnityEngine;

namespace System
{
    public class DestroyAfterTime : MonoBehaviour
    {
        public bool isOn = true;

        public float time = 0.4f;


        private void Update()
        {
            ChangeTimer();
        }

        private void ChangeTimer()
        {
            time -= Time.deltaTime;
            if(time <= 0f)
                Destroy(gameObject);
        }
    }
}
