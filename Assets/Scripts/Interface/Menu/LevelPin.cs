using Game.Interface;
using ScriptableObjects.Level;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.Menu
{
    public class LevelPin : MonoBehaviour
    {
        [Header("组件")]
        public TextMeshProUGUI textNumber;
        private LevelPinGenerator _levelPinGenerator;
        private MenuScene _menuScene;
        public Button button;

        [Space()]
        public IndicatorStars stars;
        public Image lockIndicator;
        [Space()]
        public Animation animationPanelLocked;

        private int _id;
        private bool _isUnlocked;

        public Sprite starActive;
        public Sprite starInactive;

        private Level _level;


        private void Awake()
        {
            _levelPinGenerator = GameObject.Find("LevelPinGenerator").GetComponent<LevelPinGenerator>();
            _menuScene = GameObject.Find("MenuScene").GetComponent<MenuScene>();

            button.onClick.AddListener(RunLevel);
        }


        public void LoadLevel(int currentId, Level level)
        {
            _level = level;
            _id = currentId;
            Refresh();
        }

        public void Refresh()
        {
            _isUnlocked = _levelPinGenerator.levelHolder.IsLevelUnlocked(_id);

            textNumber.text = $"{_id + 1}";
            stars.SetStar(_level.GetResult());
            lockIndicator.gameObject.SetActive(!_isUnlocked);
        }

        public void RunLevel()
        {
            if (!_menuScene.RunLevel(_id))
                animationPanelLocked.Play();
        }
    }
}
