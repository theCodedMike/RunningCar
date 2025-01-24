using System.Collections.Generic;
using System.Utility;
using UnityEngine;

namespace Interface.Menu
{
    public class MenuCamera : MonoBehaviour
    {
        [Header("组件")]
        public LevelPinGenerator levelPinGenerator;

        [Header("值")]
        public float limitFrom;
        public float limitTo;
        [Space()]
        public float shiftFrom;
        public float shiftTo;

        [Header("Input")]
        public bool isInput;
        public float positionBeforePress;
        public float cameraBeforePress;
        public bool isInner;
        public float inner;
        public float innerCurrent;
        public float innerPower = 1f;
        public bool innerPositive = true;
        public float innerDistanceCountdown = 0.025f;
        public float innerDistanceCountdownCurrent = 0.025f;
        public float innerPositionCacheOne = 0f;
        public float innerPositionCacheSecond = 0f;

        [Space()]
        public float spaceFrom = 0f;
        public float spaceTo = 5f;


        private void Start()
        {
            PrepareLimits();
            LoadPosition();
        }

        private void Update()
        {
            MoveCamera();
        }

        private void FixedUpdate()
        {
            ApplyInner();
        }

        public void MoveCamera()
        {
            if (Input.touchCount == 1 || isInput || Input.GetMouseButtonDown(0))
            {
                var positionPress = Input.touchCount == 1 ? Input.GetTouch(0).position.y : Input.mousePosition.y;
                if (Input.touchCount == 1 ? Input.GetTouch(0).phase == TouchPhase.Began : Input.GetMouseButtonDown(0))
                {
                    isInput = true;
                    isInner = false;
                    positionBeforePress = positionPress;
                    cameraBeforePress = transform.position.z;
                }
                if (isInput)
                {
                    var diff = -(positionPress - positionBeforePress) / 100f;
                    SetPosition(cameraBeforePress + diff);
                    ApplyLimits();

                    innerDistanceCountdownCurrent -= Time.deltaTime;
                    if (innerDistanceCountdownCurrent <= 0)
                    {
                        innerDistanceCountdownCurrent = innerDistanceCountdown;
                        innerPositionCacheSecond = innerPositionCacheOne;
                        innerPositionCacheOne = diff;
                    }
                }
                if (Input.touchCount == 1 ? Input.GetTouch(0).phase == TouchPhase.Canceled : Input.GetMouseButtonUp(0))
                {
                    isInput = false;
                    isInner = true;
                    SavePosition();
                    ApplyLimits();

                    inner = innerPositionCacheOne - innerPositionCacheSecond;
                    innerCurrent = float.IsNaN(inner) ? 0 : inner;
                    innerPositive = inner < 0;
                }
            }
        }

        public void ApplyInner()
        {
            if (isInner)
            {
                float changeBy = innerPositive ? Time.fixedDeltaTime : -Time.fixedDeltaTime;
                float prop = 100f / inner * innerCurrent;
                changeBy = changeBy / 100f * prop;
                innerCurrent += changeBy;
                if (innerPositive)
                {
                    if (innerCurrent >= 0)
                        isInner = false;
                }
                else
                {
                    if (innerCurrent <= 0)
                        isInner = false;
                }
                SetPosition(transform.position.z + (-changeBy * innerPower));
                ApplyLimits();
            }
        }

        public void PrepareLimits()
        {
            List<LevelPin> pins = levelPinGenerator.pins;
            if (pins != null)
            {
                limitFrom = pins[0].transform.position.z + shiftFrom;
                limitTo = pins[^1].transform.position.z + shiftTo;
                if(limitTo < limitFrom)
                    limitTo = limitFrom;

                limitFrom += spaceFrom;
                limitTo += spaceTo;
            }
        }

        public void LoadPosition()
        {
            float positionZ = CustomPlayerPrefs.GetFloat("MenuCamera_positionZ", transform.position.z);
            SetPosition(positionZ);
        }

        public void SavePosition()
        {
            CustomPlayerPrefs.SetFloat("MenuCamera_positionZ", transform.position.z);
        }

        public void SetPosition(float value)
        {
            if (!float.IsNaN(value))
                transform.position = new Vector3(transform.position.x, transform.position.y, value);
        }

        public void ApplyLimits()
        {
            float currentPositionZ = transform.position.z;
            if(currentPositionZ < limitFrom)
                SetPosition(limitFrom);
            if(currentPositionZ > limitTo)
                SetPosition(limitTo);
        }
    }
}
