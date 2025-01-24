using UnityEngine;

namespace Game.Objects.Triggers
{
    public class TriggerStartLine : MonoBehaviour
    {
        public ParticleSystem particleSystemLeft;
        public ParticleSystem particleSystemRight;


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                ParticleRun();
        }

        public void ParticleRun()
        {
            particleSystemLeft.Play();
            particleSystemRight.Play();
        }
    }
}
