namespace System.MobileFeatures.Ad
{
    public enum AdSceneShow
    {
        GameScene,
        MenuScene,
        SettingsScene,
        StoreScene,
        DailyRewardScene,
        DailyTaskScene,
    }
    
    [Serializable]
    public class AdShowOption
    {
        public AdSceneShow scene;
        public bool bannerShowAtStart;
        public bool interstitialShowAtStart;

        public string GetSceneString() => scene switch
        {
            AdSceneShow.GameScene => "GameScene",
            AdSceneShow.MenuScene => "MenuScene",
            AdSceneShow.SettingsScene => "SettingsScene",
            AdSceneShow.StoreScene => "StoreScene",
            AdSceneShow.DailyRewardScene => "DailyRewardScene",
            AdSceneShow.DailyTaskScene => "DailyTaskScene",
            _ => ""
        };
    }
}
