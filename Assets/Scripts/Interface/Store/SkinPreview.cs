using Data;
using Interface.UI_Components;
using TMPro;
using UnityEngine;

namespace Interface.Store
{
    public class SkinPreview : MonoBehaviour
    {
        
        public ObjectsHolder objectsHolder;

        [Header("Animation")]
        public Animation body;
        public Animation[] wheels;

        [Header("Skin Holder")]
        public StoreMeshHolder bodyMesh;
        public StoreMeshHolder[] wheelsMesh;
        [Space()]
        public TextMeshProUGUI textBodyName;
        public TextMeshProUGUI textWheelsName;
        [Space()]
        public ProgressBar progressBarSpeed;
        public ProgressBar progressBarSteering;
        public ProgressBar progressBarVelocity;

        [HideInInspector]
        public bool isFirstUpdateSkin = true;

        public void PlayAnimationBody()
        {
            body.Play();
        }

        public void PlayAnimationWheels()
        {
            foreach (var wheel in wheels)
                wheel.Play();
        }

        public void UpdateBody()
        {
            var skinBody = objectsHolder.skinHolder.GetSelectedSkinBody();
            var skinWheels = objectsHolder.skinHolder.GetSelectedSkinWheels();

            bodyMesh.Apply(skinBody);
            foreach (var wheel in wheelsMesh)
                wheel.Apply(skinWheels);

            textBodyName.text = skinBody.name;
            textWheelsName.text = skinWheels.name;

            var statSpeed = (skinBody.speed + skinWheels.speed) / 2;
            var statSteering = (skinBody.steering + skinWheels.steering) / 2;
            var statVelocity = (skinBody.velocity + skinWheels.velocity) / 2;

            if (isFirstUpdateSkin)
            {
                isFirstUpdateSkin = false;

                progressBarSpeed.SetValueInstant(statSpeed);
                progressBarSteering.SetValueInstant(statSteering);
                progressBarVelocity.SetValueInstant(statVelocity);
            }
            else
            {
                progressBarSpeed.SetValue(statSpeed);
                progressBarSteering.SetValue(statSteering);
                progressBarVelocity.SetValue(statVelocity);
            }
        }
    }
}
