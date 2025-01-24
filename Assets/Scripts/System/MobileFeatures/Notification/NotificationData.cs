namespace System.MobileFeatures.Notification
{
    [Serializable]
    public class NotificationData
    {
        public string title;

        public string[] text;

        public int delayHours;
        
        public string GetRandomText() => text[UnityEngine.Random.Range(0, text.Length)];
    }
}
