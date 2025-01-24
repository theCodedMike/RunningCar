using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Interface
{
    public class CanvasTutorial : MonoBehaviour
    {
        private ObjectsHolder _objectsHolder;

        public TextMeshProUGUI textTitle;
        public TextMeshProUGUI textMessage;
        public Image icon;

        [Header("Values")]
        public Sprite transparent;

        private void Start()
        {
            _objectsHolder = GameObject.Find("ObjectsHolder").GetComponent<ObjectsHolder>();
        }

        public void Load(string title, string message, Sprite sprite)
        {
            textTitle.text = title;
            textMessage.text = message;
            icon.sprite = sprite != null ? sprite : transparent;
        }

        public void Close() => _objectsHolder.gameController.CloseTutorial();
    }
}
