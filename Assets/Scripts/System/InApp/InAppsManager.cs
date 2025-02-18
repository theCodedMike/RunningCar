using System.Audio.Sound;
using System.Audio.Sound.Audio;
using System.Collections.Generic;
using System.MobileFeatures.Ad;
using System.Utility;
using Data;
using Interface.Indicator;
using Interface.Store;
using ScriptableObjects.Settings;
using UnityEngine;
#if !UNITY_STANDALONE && !UNITY_WEBGL
using UnityEngine.Purchasing;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
#endif

namespace System.InApp
{
    public class InAppsManager : MonoBehaviour
#if !UNITY_STANDALONE && !UNITY_WEBGL
        , IStoreListener
#endif
    {
#if !UNITY_STANDALONE && !UNITY_WEBGL
        private IStoreController storeController;
        private IExtensionProvider extensionProvider;
#endif

        public SettingsHolder settingsHolder;

        public List<ButtonInApp> buttons;
        public GameObject canvasInAppLoading;

        [Header("Buttons")]
        public ButtonInApp buttonRemoveAds;
        public ButtonInApp buttonCoins;

        [Space()]
        public GameObject scrollViewRemoveAds;
        public GameObject textRemoveAds;
        public GameObject containerRemoveAds;
        public GameObject containerCoins;

        private async void Awake()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
            InitializationOptions options = new InitializationOptions()
                .SetEnvironmentName("production");

            await UnityServices.InitializeAsync(options);
            ResourceRequest operation = Resources.LoadAsync<TextAsset>("IAPProductCatalog");
            operation.completed += HandleIAPCatalogLoaded;
#endif
        }

        private void HandleIAPCatalogLoaded(AsyncOperation operation)
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
            ResourceRequest request = operation as ResourceRequest;

            if (settingsHolder.settingsInApps.inAppsDebugMessagesOn)
                Debug.Log($"Loaded Asset: {request.asset}");
            ProductCatalog catalog = JsonUtility.FromJson<ProductCatalog>((request.asset as TextAsset).text);
            if (settingsHolder.settingsInApps.inAppsDebugMessagesOn)
                Debug.Log($"Loaded catalog with {catalog.allProducts.Count} items");

            ConfigurationBuilder builder = ConfigurationBuilder.Instance(
#if UNITY_IOS
                StandardPurchasingModule.Instance(AppStore.AppleAppStore)
#elif UNITY_ANDROID || UNITY_EDITOR
                StandardPurchasingModule.Instance(AppStore.GooglePlay)
#endif
                );
            string receipt = builder.Configure<IAppleConfiguration>().appReceipt;
            bool canMakePayments = builder.Configure<IAppleConfiguration>().canMakePayments;

            foreach (ProductCatalogItem item in catalog.allProducts)
            {
                builder.AddProduct(item.id, item.type);
            }

            if (settingsHolder.settingsInApps.inAppsDebugMessagesOn)
                Debug.Log($"Initializing Unity IAP with {builder.products.Count} products");
            UnityPurchasing.Initialize(this, builder);
#endif
        }

        public void Purchase(
#if !UNITY_STANDALONE && !UNITY_WEBGL
            Product product
#endif
            )
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
            canvasInAppLoading.SetActive(true);
            storeController.InitiatePurchase(product);
#endif
        }

        
        
        
        
        
        
        
        
        #region Listeners

#if !UNITY_STANDALONE && !UNITY_WEBGL
        void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            storeController = controller;
            extensionProvider = extensions;
            if (settingsHolder.settingsInApps.inAppsDebugMessagesOn)
                Debug.Log($"Successfully Initialized Unity IAP. Store Controller has {controller.products.all.Length} products");
            PrepareUI();
        }

        void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
        {
            canvasInAppLoading.SetActive(false);
            if (settingsHolder.settingsInApps.inAppsDebugMessagesOn)
                Debug.LogError($"Error initialising IAP because of {error}.");
        }

        void IStoreListener.OnInitializeFailed(InitializationFailureReason error, string message)
        {
            canvasInAppLoading.SetActive(false);
            if (settingsHolder.settingsInApps.inAppsDebugMessagesOn)
                Debug.LogError($"Error initialising IAP because of {error}. " + message);
        }

        void IStoreListener.OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            canvasInAppLoading.SetActive(false);
            if (settingsHolder.settingsInApps.inAppsDebugMessagesOn)
                Debug.LogError($"Purchase failed, reason: {failureReason}.");
        }

        PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var product = purchaseEvent.purchasedProduct;
            var payouts = product.definition.payouts;

            if (settingsHolder.settingsInApps.inAppsDebugMessagesOn)
                Debug.Log($"Successfully purchased {product.definition.id}");

            foreach (var button in buttons)
            {
                if (product.definition.id == button.inApp.id)
                {
                    var inApp = button.inApp;
                    switch (inApp.type)
                    {
                        case InAppType.Coin:
                            AddCoins(inApp.value);
                            break;
                        case InAppType.RemoveAds:
                            RemoveAds();
                            break;
                    }
                    break;
                }
            }

            canvasInAppLoading.SetActive(false);

            return PurchaseProcessingResult.Complete;
        }
#endif
        #endregion

        
        
        
        
        
        
        
        #region View
        public void PrepareUI()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
            LoadButtons(buttonRemoveAds, settingsHolder.settingsInApps.inAppsBasic, containerRemoveAds.transform);
            LoadButtons(buttonCoins, settingsHolder.settingsInApps.inAppCoins, containerCoins.transform);
            TryToDestroyRemoveAdsScrollView();

            canvasInAppLoading.SetActive(false);
#endif
        }

        public void LoadButtons(ButtonInApp buttonPrefab, InApp[] inAppData, Transform container)
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
            var products = storeController.products.all;
            var settingsInApps = settingsHolder.settingsInApps;
            foreach (var product in products)
            {
                var inApp = settingsInApps.GetInAppForID(product.definition.id, inAppData);
                if (inApp != null)
                {
                    if (inApp.on)
                    {
                        var button = GameObject.Instantiate(buttonPrefab, container);
                        button.GetComponent<ButtonInApp>().Load(product, inApp);
                        buttons.Add(button);
                    }
                }
            }
#endif
        }

        public void TryToDestroyRemoveAdsScrollView()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
            SettingsAds settingsAds = settingsHolder.settingsAds;
            if ((settingsAds.adMobOn == false && settingsAds.unityAdsOn == false) || CustomPlayerPrefs.GetBool("ads_removedByInApp", false))
            {
                Destroy(scrollViewRemoveAds.gameObject);
            }
#endif
        }
        #endregion
        
        
        
        
        
        
        
        
        #region Rewards
        public void AddCoins(int value)
        {
            var coin = CustomPlayerPrefs.GetInt("coin");
            CustomPlayerPrefs.SetInt("coin", coin + value);

            if (GameObject.Find("Indicators"))
                GameObject.Find("Indicators").GetComponent<IndicatorsController>().UpdateIndicatorCoins();
            Sound.Play(SoundType.Coin);
        }

        public void RemoveAds()
        {
            CustomPlayerPrefs.SetBool("ads_removedByInApp", true);
            TryToDestroyRemoveAdsScrollView();

            /*
            if (GameObject.Find("Ad"))
                GameObject.Find("Ad").GetComponent<AdsInterface>().HideBanner();*/
        }
        #endregion
    }
}
