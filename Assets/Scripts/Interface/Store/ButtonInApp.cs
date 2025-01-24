using System.Audio.Sound;
using System.Audio.Sound.Audio;
using System.InApp;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace Interface.Store
{
    public class ButtonInApp : MonoBehaviour
    {
        public InAppsManager inAppsManager;
        public TextMeshProUGUI textTitle;
        public TextMeshProUGUI textValue;
        public TextMeshProUGUI textPrice;

        public Image icon;
        public GameObject indicatorBestDeal;

        [Header("Values")]
        public Product currentProduct;
        public InApp inApp;

        private void Awake()
        {
            inAppsManager = GameObject.Find("InAppsManager").GetComponent<InAppsManager>();
        }

        public void Load(Product product, InApp inApp)
        {
            this.inApp = inApp;
            currentProduct = product;

            textTitle.text = this.inApp.title;
            if (textValue != null)
                textValue.text = $"{this.inApp.value}";
            textPrice.SetText($"{product.metadata.localizedPriceString}" + $"{product.metadata.isoCurrencyCode}");
            indicatorBestDeal.SetActive(inApp.bestDealIndicator);
            icon.sprite = inApp.icon;
        }

        public void Purchase()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
            inAppsManager.Purchase(currentProduct);
#endif
            Sound.Play(SoundType.ButtonClick);
        }
    }
}
