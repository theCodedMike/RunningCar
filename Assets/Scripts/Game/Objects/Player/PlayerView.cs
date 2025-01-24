using UnityEngine;

namespace Game.Objects.Player
{
    public class PlayerView : MonoBehaviour
    {
        private PlayerModel _model;

        private void Awake()
        {
            _model = GetComponent<PlayerModel>();
        }

        public void Hide()
        {
            _model.body.SetActive(false);
            _model.wheels.SetActive(false);
        }

        public void Show()
        {
            _model.body.SetActive(true);
            _model.wheels.SetActive(true);
        }

        public void SetParticleSystemExplosion()
        {
            GameObject particle = Instantiate(_model.particleSystemExplosion);
            particle.transform.position = transform.position;
            particle.transform.localScale = Vector3.one;
        }
    }
}
