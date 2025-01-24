using UnityEngine;

namespace System.InApp
{
    public enum InAppType
    {
        RemoveAds,
        Coin
    }
    
    [Serializable]
    public class InApp
    {
        public string id;
        public bool on = true;
        public InAppType type;
        public string title;
        public int value;
        public Sprite icon;
        public bool bestDealIndicator = false;
    }
}
