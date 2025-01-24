using System.Audio.Sound;
using ScriptableObjects.Skins;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.Store.CanvasStoreAlert
{
    public class CanvasStoreAlert : MonoBehaviour
    {
        [Header("Objects")]
        public TextMeshProUGUI textTitle;
        public TextMeshProUGUI textContext;
        public Image icon;

        [Header("Values")]
        public StoreAlertData[] datas;
        public StoreMeshHolder body;

        #region State
        public void Show(StoreAlertData.Type type, Skin skin)
        {
            foreach (var data in datas)
            {
                if (data.type == type)
                {
                    Show(data);
                    body.Apply(skin);
                }
            }
        }

        private void Show(StoreAlertData data)
        {
            textTitle.text = data.title;
            textContext.text = data.context;
            icon.sprite = data.icon;
            
            gameObject.SetActive(true);
            
            Sound.Play(data.audio);
        }

        public void Hide() => gameObject.SetActive(false);
        #endregion
    }
}
