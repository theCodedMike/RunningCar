using System;
using UnityEngine;

namespace Game.Objects.Player
{
    [Serializable]
    public class PlayerState
    {
        public GameObject groundObject;
        public float playerAngleY;
        public float playerPositionY;

        public PlayerState(GameObject groundObject, float playerAngleY, float playerPositionY)
        {
            this.groundObject = groundObject;
            this.playerAngleY = playerAngleY;
            this.playerPositionY = playerPositionY;
        }
    }
}
