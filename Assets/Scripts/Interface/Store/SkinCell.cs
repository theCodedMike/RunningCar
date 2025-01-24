using System.Audio.Sound;
using System.Audio.Sound.Audio;
using Data;
using Interface.UI_Components;
using ScriptableObjects.Skins;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.Store
{
    public class SkinCell : MonoBehaviour
    {
        enum Type
        {
            Current, Wait, Unavailable
        }

        private Type _type = Type.Current;
        private SettingsHolder _settingsHolder;
        private ObjectsHolder _objectsHolder;
        private StoreController _storeController;

        public TextMeshProUGUI textName;
        public TextMeshProUGUI textCentral;
        public Text textPrice;
        public GameObject priceIndicator;

        public MeshFilter bodyMeshFilter;
        public MeshRenderer bodyMeshRenderer;

        public ProgressBar progressBarSpeed;
        public ProgressBar progressBarSteering;
        public ProgressBar progressBarVelocity;

        private int _id;
        private Skin _skin;
        private bool _isBody = true;

        public Image buttonSelector;
        public GameObject glowCurrent;
        public GameObject backgroundPrice;
        
        
        
        
        
        private void Awake()
        {
            _settingsHolder = SettingsHolder.Instance;
            _objectsHolder = GameObject.Find("ObjectsHolder").GetComponent<ObjectsHolder>();
            _storeController = GameObject.Find("Store").GetComponent<StoreController>();
        }

        public void Load(Skin skin, int id, bool isBody)
        {
            _id = id;
            _skin = skin;
            _isBody = isBody;

            bodyMeshFilter.mesh = skin.mesh;
            bodyMeshRenderer.material = skin.material;
            textName.text = skin.name;
            textPrice.text = skin.price.ToString();

            progressBarSpeed.SetValue(skin.speed);
            progressBarSteering.SetValue(skin.steering);
            progressBarVelocity.SetValue(skin.velocity);

            if (skin.IsUnlocked())
            {
                priceIndicator.SetActive(false);
                textCentral.gameObject.SetActive(true);
                backgroundPrice.SetActive(false);

                var selectedSkin = isBody ? _objectsHolder.skinHolder.GetSelectedSkinBody() : _objectsHolder.skinHolder.GetSelectedSkinWheels();
                if (selectedSkin == skin)
                {
                    _type = Type.Current;
                    glowCurrent.SetActive(true);
                }
                else
                {
                    _type = Type.Wait;
                    glowCurrent.SetActive(false);
                }
            }
            else
            {
                priceIndicator.SetActive(true);
                textCentral.gameObject.SetActive(false);
                backgroundPrice.SetActive(true);
                _type = Type.Unavailable;
                glowCurrent.SetActive(false);
            }
        }

        public void Interact()
        {
            switch (_type)
            {
                case Type.Current:
                    Sound.Play(SoundType.Smash1);
                    break;
                case Type.Wait:
                    if (_isBody)
                    {
                        _objectsHolder.skinHolder.SetSelectedSkinBody(_id);
                        _objectsHolder.skinPreview.PlayAnimationBody();
                    }
                    else
                    {
                        _objectsHolder.skinHolder.SetSelectedSkinWheels(_id);
                        _objectsHolder.skinPreview.PlayAnimationWheels();
                    }
                    _storeController.Refresh();
                    Sound.Play(SoundType.Equip);
                    break;
                case Type.Unavailable:
                    _storeController.Buy(_skin);
                    break;
            }
        }
    }
}
