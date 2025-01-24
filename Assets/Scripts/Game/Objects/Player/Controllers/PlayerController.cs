using System;
using System.Audio.Sound;
using System.Audio.Sound.Audio;
using System.DailyTask;
using System.Utility;
using Game.Game;
using Game.Objects.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Objects.Player.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerModel _model;
        private PlayerView _view;


        #region System
        private void Start()
        {
            _model = GetComponent<PlayerModel>();
            _view = GetComponent<PlayerView>();
        }
        
        private void Update()
        {
            if (!IsPlay())
                return;

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if(Input.mousePosition.y > (float)Screen.height / 100 * 85)
                    return;
                if(touch.phase == TouchPhase.Began)
                    Interact();
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if(Input.mousePosition.y < (float)Screen.height / 100 * 85)
                    Interact();
            }
            
            if(Input.GetKeyDown(KeyCode.Space))
                Interact();
        }

        private void FixedUpdate()
        {
            Move();
            IncreaseSpeed();

            if (!IsPlay())
                return;
            
            TryToMakeTurn();
            CheckIsLose();
            IncreaseSpeed();
            MakeJump();
        }
        #endregion


        #region State
        public void RestorePlayer()
        {
            FindObjectOfType<PlayerView>().Show();
            PlayerModel playerModel = FindObjectOfType<PlayerModel>();
            PlayerState playerState = playerModel.states.Count == 1 ? playerModel.states[^1] : playerModel.states[^2];
            playerModel.objectsHolder.gameModel.completedGrounds -= playerModel.states.Count == 1 ? 1 : 2;
            playerModel.currentSpeed = 0f;

            playerModel.states = new();
            playerModel.ReturnToState(playerState);
        }

        public void Kill() => _model.objectsHolder.gameController.Lose();

        private void CheckIsLose()
        {
            if(transform.position.y < _model.loseAtY)
                _model.objectsHolder.gameController.Lose();
        }

        public bool IsPlay()
        {
            if (SceneManager.GetActiveScene().name == "GameScene")
                return _model.objectsHolder.gameController.GetState() == GameState.Game;

            return true;
        }
        #endregion



        #region Actions
        private void Interact()
        {
            switch (_model.playerInteractionType)
            {
                case PlayerInteractionType.Turn: Turn(); break;
                case PlayerInteractionType.Jump: Jump(); break;
                case PlayerInteractionType.Run: ContinueRunning(); break;
            }
        }

        private void Move()
        {
            bool move = true;
            if (!IsPlay())
            {
                if (_model.isContinueRunAfterWin &&
                    Vector3.Distance(transform.position, _model.positionWhenTouchFinishLine) >=
                    _model.distanceToStopAfterLose)
                    move = false;
            }

            if (_model.playerInteractionType == PlayerInteractionType.Run)
            {
                _model.currentSpeed = 0f;
                move = false;
            }

            if (_model.inJump)
                move = false;
            if (_model.objectsHolder.gameModel.state == GameState.Tutorial)
                move = false;
            if (_model.objectsHolder.gameModel.state == GameState.PreGamePause)
                move = false;
            if (!move)
                return;

            Vector3 movement = transform.forward * (_model.currentSpeed * Time.fixedDeltaTime);
            _model.rigidBody.MovePosition(_model.rigidBody.position + (movement * GetSpeedMultiplier()));
        }

        private void IncreaseSpeed()
        {
            _model.currentTickToChangeSpeed -= Time.fixedDeltaTime;
            if (_model.currentTickToChangeSpeed > 0f)
                return;

            _model.currentTickToChangeSpeed = _model.tickToChangeSpeed;
            if (IsPlay())
            {
                _model.currentSpeed += _model.velocity;
                if (_model.currentSpeed >= _model.speed)
                    _model.currentSpeed = _model.speed;
            }
            else
            {
                bool speedIncreased = GetSpeedMultiplier() > 1f;
                _model.currentSpeed -= speedIncreased ? _model.velocity * 25 : _model.velocity;
                if (_model.currentSpeed < 0)
                    _model.currentSpeed = 0;
            }
        }

        private void Turn()
        {
            if (_model.isMakeTurn || _model.objectsHolder.gameModel.state != GameState.Game || _model.inJump)
                return;

            _model.currentTimeToMakeTurn = _model.steering;
            _model.isMakeTurn = true;
            
            _model.objectsHolder.dailyTaskManager.IncreaseProgress(DailyTaskType.MakeTurns, 1);
            Sound.Play(SoundType.Turn);
        }

        private void TryToMakeTurn()
        {
            if (!_model.isMakeTurn)
                return;
            
            _model.currentTimeToMakeTurn -= Time.fixedDeltaTime;
            if(_model.currentTimeToMakeTurn > 0)
                return;

            _model.isMakeTurn = false;

            float angle = transform.eulerAngles.y + _model.turnBy;
            _model.animator.Play(_model.turnBy < 0 ? "TurnL" : "TurnR");
            PlayParticles(_model.turnBy < 0 ? PlayerParticleInteract.Left : PlayerParticleInteract.Right);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
            DecreaseSpeedAtTurn();

            if (_model.randomAngleAfterTurn)
                _model.turnBy = RandomBool.GetRandom() ? 90 : -90;
        }

        private void DecreaseSpeedAtTurn()
        {
            float maxSpeed = _model.speed;
            float percentageToLeave = 1f - _model.settingsHolder.settingsGame.decreaseSpeedAtTurnTo;
            float speedAtTurn = maxSpeed / 1f * percentageToLeave;
            if(_model.currentSpeed > speedAtTurn)
                _model.currentSpeed = speedAtTurn;
        }

        private void ContinueRunning() => _model.playerInteractionType = PlayerInteractionType.Turn;

        private void Jump()
        {
            _model.playerInteractionType = PlayerInteractionType.Turn;
            _model.animator.Play("Jump");

            float rotation = transform.eulerAngles.y;
            _model.jumpPoint1 = transform.position;
            _model.jumpPoint2 = transform.position;
            float shift = _model.settingsHolder.settingsGame.jumpDistance;
            float jumpHeight = _model.settingsHolder.settingsGame.jumpHeight;

            Vector3 jumpP1 = _model.jumpPoint1;
            Vector3 jumpP2 = _model.jumpPoint2;
            switch (rotation)
            {
                case 90: 
                    _model.jumpPoint1 = new Vector3(jumpP1.x + shift / 1.75f, jumpP1.y + jumpHeight, jumpP1.z);
                    _model.jumpPoint2 = new Vector3(jumpP2.x + shift, jumpP2.y, jumpP2.z);
                    break;
                case 180: 
                    _model.jumpPoint1 = new Vector3(jumpP1.x, jumpP1.y + jumpHeight, jumpP1.z - shift / 1.75f);
                    _model.jumpPoint2 = new Vector3(jumpP2.x, jumpP2.y, jumpP2.z - shift);
                    break;
                case 270: 
                    _model.jumpPoint1 = new Vector3(jumpP1.x - shift / 1.75f, jumpP1.y + jumpHeight, jumpP1.z);
                    _model.jumpPoint2 = new Vector3(jumpP2.x - shift, jumpP2.y, jumpP2.z);
                    break;
                default: 
                    _model.jumpPoint1 = new Vector3(jumpP1.x, jumpP1.y + jumpHeight, jumpP1.z + shift / 1.75f);
                    _model.jumpPoint2 = new Vector3(jumpP2.x, jumpP2.y, jumpP2.z + shift);
                    break;
            }

            _model.isFirstPoint = true;
            _model.rigidBody.useGravity = false;
            _model.inJump = true;
            
            PlayParticles(PlayerParticleInteract.Double);
            _model.objectsHolder.dailyTaskManager.IncreaseProgress(DailyTaskType.MakeJumps, 1);
            Sound.Play(SoundType.Jump);
        }

        private void MakeJump()
        {
            if (!_model.inJump)
                return;
            Vector3 moveTo = _model.isFirstPoint ? _model.jumpPoint1 : _model.jumpPoint2;
            transform.position = Vector3.MoveTowards(transform.position, moveTo,
                Time.fixedDeltaTime * (_model.currentSpeed * GetSpeedMultiplier()));

            if (transform.position == moveTo)
            {
                if (_model.isFirstPoint)
                    _model.isFirstPoint = false;
                else
                {
                    _model.inJump = false;
                    _model.rigidBody.useGravity = true;
                    _model.isFirstPoint = true;
                }
            }
        }
        #endregion



        public void ChangeSpeed(TriggerType triggerType)
        {
            int currentSpeed = (int)_model.speedMode;

            switch (triggerType)
            {
                case TriggerType.SpeedUp:
                    currentSpeed++;
                    if (currentSpeed > 2)
                        currentSpeed = 2;
                    Sound.Play(SoundType.SpeedUp);
                    break;
                case TriggerType.SlowDown:
                    currentSpeed--;
                    if (currentSpeed < 0)
                        currentSpeed = 0;
                    Sound.Play(SoundType.SlowDown);
                    break;
            }

            _model.speedMode = (PlayerSpeedMode)currentSpeed;
        }

        public float GetSpeedMultiplier()
        {
            switch (_model.speedMode)
            {
                case PlayerSpeedMode.Slow: return _model.settingsHolder.settingsGame.speedDown;
                case PlayerSpeedMode.Fast: return _model.settingsHolder.settingsGame.speedUp;
                default: return 1f;
            }
        }

        public void PlayParticles(PlayerParticleInteract type)
        {
            switch (type)
            {
                case PlayerParticleInteract.Left: _model.particleInteractLeft.Play(); break;
                case PlayerParticleInteract.Right: _model.particleInteractRight.Play(); break;
                case PlayerParticleInteract.Double: _model.particleInteractLeft.Play(); _model.particleInteractRight.Play(); break;
            }
        }
    }
}
