using UnityEngine;

namespace Interface.Transactions
{
    public class TransactionsModel : MonoBehaviour
    {
        [Header("Transactions")]
        public GameObject canvasTransaction;

        [Space()]
        public AnimationClip animationHide;
        public AnimationClip animationShow;

        [Header("Scenarios")]
        public bool showAtStartScene = true;
    }
}
