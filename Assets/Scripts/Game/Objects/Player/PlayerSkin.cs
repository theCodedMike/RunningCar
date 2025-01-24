using Data;
using Interface.Store;
using ScriptableObjects.Skins;
using UnityEngine;

namespace Game.Objects.Player
{
    public class PlayerSkin : MonoBehaviour
    {
        private PlayerModel _model;
        private SettingsHolder _settingsHolder;

        public StoreMeshHolder body;
        public StoreMeshHolder[] wheels;

        private void Start()
        {
            _model = GetComponent<PlayerModel>();
            _settingsHolder = SettingsHolder.Instance;
            
            LoadSkin();
        }

        private void LoadSkin()
        {
            Skin skinBody = _model.objectsHolder.skinHolder.GetSelectedSkinBody();
            Skin skinWheels = _model.objectsHolder.skinHolder.GetSelectedSkinWheels();
            
            body.Apply(skinBody);
            foreach (StoreMeshHolder wheel in wheels)
                wheel.Apply(skinWheels);

            float percentageSpeed = (skinBody.speed + skinWheels.speed) / 2f;
            float percentageSteering = (skinBody.steering + skinWheels.steering) / 2f;
            float percentageVelocity = (skinBody.velocity + skinWheels.velocity) / 2f;

            _model.speed = _settingsHolder.settingsGame.speed.GetValueByPercentage(percentageSpeed);
            _model.steering = _settingsHolder.settingsGame.speed.GetValueByPercentage(percentageSteering);
            _model.velocity = _settingsHolder.settingsGame.speed.GetValueByPercentage(percentageVelocity);
        }
    }
}
