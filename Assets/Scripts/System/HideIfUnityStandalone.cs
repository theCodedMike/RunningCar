using UnityEngine;

namespace System
{
    public class HideIfUnityStandalone : MonoBehaviour
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        private void Start()
        {
            Hide();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
#endif
    }
}
