using System.Audio.Sound;
using System.Audio.Sound.Audio;
using Game.Objects.Triggers;
using UnityEngine;

namespace Game.Objects.Player.Controllers
{
    public class PlayerCollisionController : MonoBehaviour
    {
        private PlayerModel _model;
        private PlayerView _view;
        private PlayerController _controller;


        private void Start()
        {
            _model = GetComponent<PlayerModel>();
            _view = GetComponent<PlayerView>();
            _controller = GetComponent<PlayerController>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if(!_controller.IsPlay())
                return;

            switch (other.transform.tag)
            {
                case "Enemy": 
                    _view.SetParticleSystemExplosion();
                    _view.Hide();
                    _controller.Kill();
                    Sound.Play(SoundType.Smash1);
                    break;
                case "Ground": 
                    SaveStateAt(other.gameObject); 
                    _model.objectsHolder.gameController.IncreaseGroundCount(); 
                    break;
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!_controller.IsPlay())
                return;

            switch (other.transform.tag)
            {
                case "Coin": other.GetComponent<Coin>().Collect(); break;
                case "RatingStar": other.GetComponent<RatingStar>().Collect(); break;
                case "Finish":
                    _model.isContinueRunAfterWin = true;
                    _model.positionWhenTouchFinishLine = transform.position;
                    _model.objectsHolder.gameController.Win();
                    break;
                case "Gate":
                    Gate gate = other.GetComponent<Gate>();
                    if (gate.IsActive())
                        _model.turnBy = gate.GetTurn();
                    break;
                case "Trigger":
                    Trigger trigger = other.GetComponent<Trigger>();
                    if (trigger.IsActive())
                    {
                        switch (trigger.type)
                        {
                            case TriggerType.Jump: _model.playerInteractionType = PlayerInteractionType.Jump; break;
                            case TriggerType.Wait: _model.playerInteractionType = PlayerInteractionType.Run; Sound.Play(SoundType.Brake);
                                break;
                            case TriggerType.SpeedUp: _controller.ChangeSpeed(TriggerType.SpeedUp); break;
                            case TriggerType.SlowDown: _controller.ChangeSpeed(TriggerType.SlowDown); break;
                        }
                    }
                    break;
            }
        }

        private void SaveStateAt(GameObject groundObject)
        {
            PlayerState state = new(groundObject, transform.eulerAngles.y, transform.position.y);
            _model.states.Add(state);
        }
    }
}
