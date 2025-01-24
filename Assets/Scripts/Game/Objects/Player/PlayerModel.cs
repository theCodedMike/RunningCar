using System;
using System.Collections.Generic;
using System.Utility;
using Data;
using UnityEngine;

namespace Game.Objects.Player
{
    public enum PlayerInteractionType
    {
        Turn, Jump, Run
    }
    
    public enum PlayerSpeedMode
    {
        Slow, Normal, Fast
    }

    public enum PlayerParticleInteract
    {
        Left, Right, Double
    }
    
    public class PlayerModel : MonoBehaviour
    {
        [HideInInspector]
        public ObjectsHolder objectsHolder;
        [HideInInspector]
        public SettingsHolder settingsHolder = SettingsHolder.Instance;
        [HideInInspector]
        public Rigidbody rigidBody;
        [HideInInspector]
        public Animator animator;
        [HideInInspector]
        public GameObject body;
        [HideInInspector]
        public GameObject wheels;
        
        [Header("Values")]
        public float loseAtY = 0f;
        [Space()]
        [HideInInspector]
        public float speed;
        [HideInInspector]
        public float currentSpeed = 0f;
        [HideInInspector]
        public float steering = 0f;
        [HideInInspector]
        public float velocity;

        [HideInInspector]
        public bool isMakeTurn = false;
        [HideInInspector]
        public float currentTimeToMakeTurn = 0f;

        public float tickToChangeSpeed = 0.01f;
        [HideInInspector]
        public float currentTickToChangeSpeed = 0.01f;
        [HideInInspector]
        public bool isContinueRunAfterWin = false;
        [HideInInspector]
        public float turnBy = 0f;

        public float distanceToStopAfterLose = 2.5f;
        [HideInInspector]
        public Vector3 positionWhenTouchFinishLine;

        public bool randomAngleAfterTurn = true;
        [HideInInspector]
        public List<PlayerState> states = new(16);
        [HideInInspector]
        public PlayerSpeedMode speedMode = PlayerSpeedMode.Normal;
        [HideInInspector]
        public PlayerInteractionType playerInteractionType = PlayerInteractionType.Turn;

        [Header("Jump")]
        public float jumpForce = 10f;
        [HideInInspector]
        public bool inJump = false;
        [HideInInspector]
        public Vector3 jumpPoint1;
        [HideInInspector]
        public Vector3 jumpPoint2;
        [HideInInspector]
        public bool isFirstPoint = true;

        [Header("Particles")]
        public GameObject particleSystemExplosion;
        public ParticleSystem particleInteractLeft;
        public ParticleSystem particleInteractRight;


        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            objectsHolder = GameObject.Find("ObjectsHolder").GetComponent<ObjectsHolder>();
            settingsHolder = SettingsHolder.Instance;

            body = transform.Find("body").gameObject;
            wheels = transform.Find("wheels").gameObject;
            turnBy = RandomBool.GetRandom() ? 90f : -90f;
        }

        public void ReturnToState(PlayerState state)
        {
            transform.position = new Vector3(state.groundObject.transform.position.x, state.playerPositionY,
                state.groundObject.transform.position.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, state.playerAngleY, transform.eulerAngles.z);
        }
    }
}
