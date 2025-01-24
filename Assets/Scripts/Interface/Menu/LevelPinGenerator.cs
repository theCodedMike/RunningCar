using System.Collections.Generic;
using ScriptableObjects.Level;
using UnityEngine;

namespace Interface.Menu
{
    public class LevelPinGenerator : MonoBehaviour
    {
        [Header("组件")]
        public GameObject levelPinHolder;
        public LevelHolder levelHolder;

        [Header("值")]
        public bool isOn = true;
        public GameObject levelPin;
        public List<LevelPin> pins = new();
        public float distanceBetweenPins = 2f;

        private void Update()
        {
            if (isOn)
            {
                RefreshPins();
                UpdatePins();
            }
        }

        #region Generate Pins
        private void RefreshPins()
        {
            int levelCount = levelHolder.levels.Count;
            if (pins.Count > levelCount)
                RemoveLastPin();
            else if(levelCount > pins.Count)
                AddPin();
        }

        private void AddPin()
        {
            GameObject newPin = Instantiate(levelPin);
            newPin.transform.parent = levelPinHolder.transform;
            if(pins.Count == 0)
                newPin.transform.localPosition = Vector3.zero;
            else
            {
                LevelPin lastPin = GetLastPin();
                newPin.transform.localPosition = new(lastPin.transform.localPosition.x, lastPin.transform.localPosition.y, lastPin.transform.localPosition.z + distanceBetweenPins);
            }
            
            pins.Add(newPin.GetComponent<LevelPin>());
        }

        private void RemoveLastPin()
        {
            LevelPin lastPin = GetLastPin();

            pins.Remove(lastPin);
            DestroyImmediate(lastPin.gameObject);
        }

        private LevelPin GetLastPin() => pins[^1];
        #endregion




        #region Value on Pins

        private void UpdatePins()
        {
            int levelCount = levelHolder.levels.Count;
            int pinCount = pins.Count;
            if (levelCount == pinCount)
            {
                for (int i = 0; i < levelCount; i++)
                    pins[i].LoadLevel(i, levelHolder.levels[i]);
            }
        }
        #endregion
    }
}
