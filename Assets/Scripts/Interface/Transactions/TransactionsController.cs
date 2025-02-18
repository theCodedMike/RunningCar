using UnityEngine;

namespace Interface.Transactions
{
    [RequireComponent(typeof(TransactionsModel))]
    public class TransactionsController : MonoBehaviour
    {
        private TransactionsModel _model;

        private void Awake()
        {
            _model = GetComponent<TransactionsModel>();
            if (_model.showAtStartScene)
                TransactionSceneOn();
        }

        public void TransactionSceneOn() => SetCanvas(_model.animationShow);

        public void TransactionSceneOff() => SetCanvas(_model.animationHide);

        private void SetCanvas(AnimationClip anim)
        {
            GameObject canvas = Instantiate(_model.canvasTransaction);
            Animation currAnim = canvas.transform.Find("Tint").GetComponent<Animation>();
            currAnim.clip = anim;
            currAnim.Play(anim.name);
        }
    }
}
