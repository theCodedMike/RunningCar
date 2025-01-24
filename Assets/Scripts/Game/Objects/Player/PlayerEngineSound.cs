using UnityEngine;

namespace Game.Objects.Player
{
    public class PlayerEngineSound : MonoBehaviour
    {
        public AudioSource audioSource;
        public PlayerModel playerModel;

        [Range(1f, 3f)]
        public float maxPitch = 2f;


        private void Update()
        {
            ApplyPitch();
        }

        private void ApplyPitch()
        {
            float percentage = 1f / playerModel.speed * playerModel.currentSpeed;
            float diff = maxPitch - 1;
            float newPitch = 1f + (diff / 1f * percentage);

            audioSource.pitch = newPitch;
        }
    }
}
