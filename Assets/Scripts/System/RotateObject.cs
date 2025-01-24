using UnityEngine;

namespace System
{
    public enum RotationDirection
    {
        Up, Down, Left, Right, Forward, Backward
    }
    public class RotateObject : MonoBehaviour
    {
        public float speed = 50f;
        public RotationDirection rotationDirection = RotationDirection.Up;

        private void Update()
        {
            transform.Rotate(GetRotationDirection(), Time.deltaTime * speed);
        }

        private Vector3 GetRotationDirection() => rotationDirection switch
        {
            RotationDirection.Up => Vector3.up,
            RotationDirection.Down => Vector3.down,
            RotationDirection.Left => Vector3.left,
            RotationDirection.Right => Vector3.right,
            RotationDirection.Forward => Vector3.forward,
            RotationDirection.Backward => Vector3.back,
            _ => Vector3.zero
        };
    }
}
