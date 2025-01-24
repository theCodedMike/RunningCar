using System.Collections.Generic;
using Game.Objects.RagDoll;
using UnityEngine;

namespace Game.Objects.Player
{
    public class PlayerRagDoll : MonoBehaviour
    {
        public List<RagDollBone> bones;

        public void ActivateRagDollPhysics()
        {
            foreach (RagDollBone bone in bones)
                bone.rigidBody.isKinematic = false;
        }
    }
}
