using Interface.Store.CanvasStoreAlert;
using ScriptableObjects.Skins;
using UnityEngine;

namespace Interface.Store
{
    public class StoreView : MonoBehaviour
    {
        private StoreModel _model;

        private void Awake()
        {
            _model = GetComponent<StoreModel>();
        }

        
        
        
        
        #region State
        public void UpdateButtonHolder()
        {
#if UNITY_IOS || UNITY_ANDROID
            if (!_model.settingsHolder.settingsInApps.inAppsOn)
            {
                _model.buttonHolder.SetActive(false);
                _model.buttonStore.SetActive(false);
            }
#elif UNITY_STANDALONE || UNITY_WEBGL
            _model.buttonHolder.SetActive(false);
            _model.buttonStore.SetActive(false);
#endif
        }

        public void UpdateState()
        {
            _model.pageGarage.SetActive(_model.state == StoreSceneState.Garage);
            _model.pageStore.SetActive(_model.state == StoreSceneState.Store);

            _model.labelTopBar.text = _model.state == StoreSceneState.Garage ? "GARAGE" : "STORE";
        }
        #endregion





        #region Alerts

        public void ShowAlert(StoreAlertData.Type type, Skin skin)
        {
            _model.canvasStoreAlert.Show(type, skin);
        }

        public void HideAlert()
        {
            _model.canvasStoreAlert.Hide();
        }
        #endregion
    }
}
