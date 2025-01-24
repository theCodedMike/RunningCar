using UnityEngine;

namespace System.Components
{
    public class SwipeInput : MonoBehaviour
    {
        private Vector3 _inputPositionFirst;
        private Vector3 _inputPositionSecond;
        private float _inputDragDistance;
        private bool _inputPossible;
        private float _inputTimer;
        private const float InputTimerDefault = 0.5f;

        private void Start()
        {
            CalculateInputSwipeValues();
        }

        private void Update()
        {
            InputSwipe();
        }

        private void InputSwipe()
        {
            if (Input.touchCount == 1)
            {
                //Process input
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    _inputPossible = true;
                    _inputTimer = InputTimerDefault;
                    _inputPositionFirst = touch.position;
                    _inputPositionSecond = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended && _inputPossible)
                {
                    _inputPositionSecond = touch.position;

                    if (Mathf.Abs(_inputPositionSecond.x - _inputPositionFirst.x) > _inputDragDistance || Mathf.Abs(_inputPositionSecond.y - _inputPositionFirst.y) > _inputDragDistance)
                    {
                        if (Mathf.Abs(_inputPositionSecond.x - _inputPositionFirst.x) > Mathf.Abs(_inputPositionSecond.y - _inputPositionFirst.y))
                        {
                            if ((_inputPositionSecond.x > _inputPositionFirst.x))
                                InputSwipeRight();
                            else
                                InputSwipeLeft();
                        }
                        else
                        {
                            if (_inputPositionSecond.y > _inputPositionFirst.y)
                                InputSwipeUp();
                            else
                                InputSwipeDown();
                        }
                    }
                    else
                        InputTap();
                }

                //Timer countdown
                _inputTimer -= Time.deltaTime;
                if (_inputTimer <= 0.0f)
                {
                    _inputTimer = 0.0f;
                    _inputPossible = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
                InputSwipeUp();
            if (Input.GetKeyDown(KeyCode.DownArrow))
                InputSwipeDown();
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                InputSwipeLeft();
            if (Input.GetKeyDown(KeyCode.RightArrow))
                InputSwipeRight();
        }

        private void CalculateInputSwipeValues()
        {
            _inputDragDistance = (float)Screen.width * 15 / 100;
        }

        private void InputSwipeRight()
        {
            print("s right");
        }

        private void InputSwipeLeft()
        {
            print("s left");
        }

        private void InputSwipeUp()
        {
            print("s up");
        }

        private void InputSwipeDown()
        {
            print("s down");
        }

        private void InputTap()
        {
            print("tap");
        }
    }
}
