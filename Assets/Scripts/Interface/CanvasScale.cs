using UnityEngine;
using UnityEngine.UI;

namespace Interface
{
    public class CanvasScale : MonoBehaviour
    {
        // Objects
        private Camera _mainCamera;
        private CanvasScaler _canvasScaler;
        // Values
        private Vector2 _resolution;
        
        [Header("位置")]
        public bool isFixWidth = false;
        public float fixedWidth = 1125;
        public bool isFixHeight = true;
        public float fixedHeight = 2436;

        [Header("百分比")]
        public bool percentWidth = true;
        [Range(0, 200)]
        public float percentWidthValue = 100;
        public bool percentHeight = true;
        [Range(0, 200)]
        public float percentHeightValue = 100;


        #region Standard system methods
        private void Awake()
        {
            _mainCamera = Camera.main;
            _canvasScaler = GetComponent<CanvasScaler>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            GetScreenSize();
            SetReferenceResolution();
        }

        // Update is called once per frame
        private void Update()
        {
            GetScreenSize();
            SetReferenceResolution();
        }
        #endregion

        /// <summary>
        /// 获取屏幕尺寸
        /// </summary>
        private void GetScreenSize()
        {
            float width = _mainCamera.pixelWidth;
            float height = _mainCamera.pixelHeight;
            
            if(isFixWidth)
                width = fixedWidth;
            if(isFixHeight)
                height = fixedHeight;
            
            if(percentWidth)
                width = width / 100f * percentWidthValue;
            if(percentHeight)
                height = height / 100f * percentHeightValue;

            _resolution = new Vector2(width, height);
        }

        /// <summary>
        /// 设置屏幕分辨率
        /// </summary>
        private void SetReferenceResolution()
        {
            _canvasScaler.referenceResolution = _resolution;
        }
    }
}
