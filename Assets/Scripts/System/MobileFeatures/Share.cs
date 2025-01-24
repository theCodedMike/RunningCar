
#if UNITY_IPHONE
using UnityEngine.iOS;
#elif UNITY_ANDROID
using UnityEngine.Android;
#endif
using System.Utility;
using UnityEngine;




namespace System.MobileFeatures
{
    public class Share : MonoBehaviour
    {
        //Values
        public string appLink = "http://bit.ly/taptapstudio";

        /// <summary>
        /// Call to share high score.
        /// </summary>
        public void Make()
        {
            var text = "Beat my  score: " + CustomPlayerPrefs.GetInt("highScore") + " " + appLink;

#if UNITY_IPHONE
            // Construct the share URL with the text to share
            var shareURL = "mailto:?subject=Share&body=" + UnityWebRequest.EscapeURL(text);
            Application.OpenURL(shareURL);
#elif UNITY_ANDROID
            ShareTextInAndroid(text);
#endif
        }

        private void ShareTextInAndroid(string text)
        {
#if UNITY_ANDROID
            // Create a share intent and set the text to share
            string shareIntent = "android.intent.action.SEND";
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>(shareIntent));
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), text);

            // Get the current activity and start the share intent
            AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("startActivity", intentObject);
#endif
        }
    }
}
