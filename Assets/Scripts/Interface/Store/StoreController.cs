using System.Utility;
using Data;
using Interface.Indicator;
using Interface.Store.CanvasStoreAlert;
using ScriptableObjects.Skins;
using UnityEngine;

namespace Interface.Store
{
    public class StoreController : MonoBehaviour
    {
        private StoreModel _model;
        private StoreView _view;

        private void Awake()
        {
            _model = GetComponent<StoreModel>();
            _view = GetComponent<StoreView>();
        }

        private void Start()
        {
            _view.HideAlert();
            LoadSkins();
            _model.objectsHolder.skinPreview.UpdateBody();
            _view.UpdateButtonHolder();
            LoadState();
        }
        
        
        
        

       
        #region Work with indicators

        public void Refresh()
        {
            for (int i = 0; i < _model.skinCellsBody.Count; i++)
                _model.skinCellsBody[i].Load(_model.objectsHolder.skinHolder.skinsBody[i], i, true);
            for (int i = 0; i < _model.skinCellsWheels.Count; i++)
                _model.skinCellsWheels[i].Load(_model.objectsHolder.skinHolder.skinsWheels[i], i, false);

            _model.objectsHolder.skinPreview.UpdateBody();
        }

        private void LoadSkins()
        {
            SetSkinCellSpace(true);
            SetSkinCellSpace(false);
            int id = 0;
            _model.settingsHolder = SettingsHolder.Instance;
            foreach (var skin in _model.objectsHolder.skinHolder.skinsBody)
            {
                SetSkinCell(skin, id, true);
                id++;
            }
            id = 10000;
            foreach (var skin in _model.objectsHolder.skinHolder.skinsWheels)
            {
                SetSkinCell(skin, id, false);
                id++;
            }
            SetSkinCellSpace(true);
            SetSkinCellSpace(false);
        }

        private void SetSkinCellSpace(bool toBodyContent)
        {
            Instantiate(_model.skinCellSpace, toBodyContent ? _model.skinCellsContentBody.transform : _model.skinCellsContentWheels.transform);
        }

        private void SetSkinCell(Skin skin, int id, bool toBodyContent)
        {
            SkinCell cell = Instantiate(_model.skinCell, toBodyContent ? _model.skinCellsContentBody.transform : _model.skinCellsContentWheels.transform);
            cell.Load(skin, id, toBodyContent);
            if (toBodyContent)
                _model.skinCellsBody.Add(cell);
            else
                _model.skinCellsWheels.Add(cell);
        }

        #endregion

        #region Buy

        public void Buy(Skin skin)
        {
            int coins = CustomPlayerPrefs.GetInt("coin");
            if (skin.price <= coins)
            {
                coins -= skin.price;
                CustomPlayerPrefs.SetInt("coin", coins);
                IndicatorsController.Instance.UpdateIndicatorCoins();

                skin.Unlock();
                _view.ShowAlert(StoreAlertData.Type.Success, skin);
                Refresh();
            }
            else
                _view.ShowAlert(StoreAlertData.Type.Failed, skin);
        }

        #endregion

        #region State

        public void SetStateToGarage()
        {
            SetState(StoreSceneState.Garage);
        }

        public void SetStateToStore()
        {
            SetState(StoreSceneState.Store);
        }

        private void LoadState()
        {
            int state = CustomPlayerPrefs.GetInt("storeState", 0);
            SetState((StoreSceneState)state);
        }

        private void SetState(StoreSceneState state)
        {
            _model.state = state;
            CustomPlayerPrefs.SetInt("storeState", (int)_model.state);
            _view.UpdateState();
        }

        #endregion
    }
}
