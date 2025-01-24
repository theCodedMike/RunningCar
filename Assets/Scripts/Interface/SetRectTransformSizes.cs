using UnityEngine;
using UnityEngine.UI;

namespace Interface
{
    public class SetRectTransformSizes : MonoBehaviour
    {
        private RectTransform _rectTransform;

        public RectTransform rectTransformCanvas;
        public CanvasScaler canvasScaler;
        public bool applySizeWidth = true;
        [Range(0f, 100f)]
        public float spaceWidth = 20f;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            
            SetSizes();
        }

        private void Update()
        {
            SetSizes();
        }

        public void SetSizes()
        {
            float sizeOfScreen = rectTransformCanvas.sizeDelta.x;
            float space = rectTransformCanvas.sizeDelta.x / 100f * spaceWidth;
            sizeOfScreen -= space;

            _rectTransform.sizeDelta = new Vector2(sizeOfScreen, _rectTransform.sizeDelta.y);
        }
    }
}
