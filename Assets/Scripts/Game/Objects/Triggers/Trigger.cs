using UnityEngine;

namespace Game.Objects.Triggers
{
    public enum TriggerType
    {
        Jump, Wait, SpeedUp, SlowDown
    }
    public class Trigger : MonoBehaviour
    {
        public TriggerType type;

        private bool _isActive = true;

        public bool IsActive()
        {
            if (_isActive)
            {
                _isActive = false;
                return true;
            }

            return false;
        }
    }
}
