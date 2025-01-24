using System.Audio.Sound;
using System.Audio.Sound.Audio;
using System.Collections;
using Data;
using TMPro;
using UnityEngine;

namespace Game.Interface
{
    public class CanvasPreGamePause : MonoBehaviour
    {
        private ObjectsHolder _objectsHolder;

        [Header("组件")]
        public TextMeshProUGUI textIndicator;
        [Header("值")]
        public float timeStep = 1f;
        public float currentTimeStep = 1f;
        public int step = 3;
        
        // Start is called before the first frame update
        private void Start()
        {
            _objectsHolder = GameObject.Find("ObjectsHolder").GetComponent<ObjectsHolder>();
            currentTimeStep = timeStep;

            StartCoroutine(ProcessStep());
        }

        private IEnumerator ProcessStep()
        {
            while (step > 0)
            {
                yield return new WaitForSeconds(currentTimeStep);
                DecreaseStep();
            }

            yield return null;
        }

        public void DecreaseStep()
        {
            step--;
            if (step <= 0)
            {
                _objectsHolder.gameController.GameStart();
                Sound.Play(SoundType.StartLineRun);
            }
            else
                Sound.Play(SoundType.StartLinePrepare);
            
            RefreshIndicator();
        }

        public void RefreshIndicator() => textIndicator.text = step.ToString();
        
        public void Restart()
        {
            currentTimeStep = timeStep;
            step = 3;
            
            RefreshIndicator();
            StartCoroutine(ProcessStep());
        }
    }
}
