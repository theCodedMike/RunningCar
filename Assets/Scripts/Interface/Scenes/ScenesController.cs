using System.Collections;
using System.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interface.Scenes
{
    [RequireComponent(typeof(ScenesModel))]
    public class ScenesController : MonoBehaviour
    {
        private ScenesModel _model;

        private void Awake()
        {
            _model = GetComponent<ScenesModel>();
            RunScenarios();
        }

        
        #region Change Scenes
        public void ChangeScene(string sceneName)
        {
            CustomPlayerPrefs.SetString("previousScene", SceneManager.GetActiveScene().name);
            MarkTutorialAsOpened(sceneName);
            SceneManager.LoadScene(sceneName);
        }

        public void ChangeSceneWithDelay(string sceneName)
        {
            if(_model.transactions != null)
                _model.transactions.TransactionSceneOff();

            StartCoroutine(RunSceneWithDelay(sceneName));
        }

        private IEnumerator RunSceneWithDelay(string sceneName)
        {
            yield return new WaitForSeconds(_model.delay);
            ChangeScene(sceneName);
        }

        public void BackToPreviousScene() => ChangeScene(_model.GetPreviousScene());

        public void BackToPreviousSceneWithDelay() => ChangeSceneWithDelay(_model.GetPreviousScene());
        #endregion



        #region Scenarios
        private void RunScenarios()
        {
            if (_model.scenario == Scenario.Launch)
                StartCoroutine(ScenarioLaunchScreen());
        }

        private IEnumerator ScenarioLaunchScreen()
        {
            yield return new WaitForSeconds(_model.launchScreenDelay);
            ChangeSceneWithDelay("MenuScene");
        }

        private void MarkTutorialAsOpened(string sceneName)
        {
            if (sceneName == "TutorialScene")
                CustomPlayerPrefs.SetBool("tutorialWasOpened", true);
        }
        #endregion
    }
}
