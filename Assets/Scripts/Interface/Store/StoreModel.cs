using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;

namespace Interface.Store
{
    public enum StoreSceneState
    {
        Garage, Store
    }
    public class StoreModel : MonoBehaviour
    {
        [HideInInspector]
        public Canvas canvas;
        public CanvasStoreAlert.CanvasStoreAlert canvasStoreAlert;

        [HideInInspector]
        public ObjectsHolder objectsHolder;
        [HideInInspector]
        public SettingsHolder settingsHolder;

        public GameObject skinCellSpace;
        public SkinCell skinCell;
        [HideInInspector]
        public List<SkinCell> skinCellsBody;
        [HideInInspector]
        public List<SkinCell> skinCellsWheels;
        [Space()]
        public GameObject skinCellsContentBody;
        public GameObject skinCellsContentWheels;

        [Header("Interface")]
        public GameObject buttonHolder;
        [Space()]
        public GameObject buttonGarage;
        public GameObject buttonStore;
        [Space()]
        public TextMeshProUGUI labelTopBar;

        [Header("Pages")]
        public GameObject pageGarage;
        public GameObject pageStore;

        public StoreSceneState state;

        private void Awake()
        {
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            settingsHolder = SettingsHolder.Instance;
            objectsHolder = GameObject.Find("ObjectsHolder").GetComponent<ObjectsHolder>();

            skinCellsBody = new();
        }
    }
}
