#if UNITY_EDITOR
using UnityEditor;
#endif
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class CustomMapGrid : MonoBehaviour
    {
        public SettingsHolder settingsHolder;
        
        [Header("Steps")]
        public Vector3 grid = new(4f, 4f, 4f);
        public Vector3 angle = new(90f, 90f, 90f);

#if UNITY_EDITOR
        private void OnValidate()
        {
            Selection.selectionChanged = UpdateAngles;
        }

        private void OnDisable()
        {
            Selection.selectionChanged = null;
            UpdateAngles();
        }

        private void OnDrawGizmos()
        {
            SortPositions();
        }

        private void UpdateAngles()
        {
            string scene = SceneManager.GetActiveScene().name;
            if (scene == "LevelEditorScene" || scene == "TutorialScene")
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    if (child.CompareTag("Ground"))
                        child.transform.eulerAngles = RoundEulerAngle(child.transform.eulerAngles);
                }
            }
        }

        private void SortPositions()
        {
            string scene = SceneManager.GetActiveScene().name;
            if (scene == "LevelEditorScene" || scene == "TutorialScene")
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    if (child.CompareTag("Ground"))
                    {
                        Vector3 rounded = RoundPosition(child.transform.position);
                        child.transform.position = new Vector3(rounded.x, -1.5f, rounded.z);
                    }
                }
            }
        }
        

        private Vector3 RoundPosition(Vector3 value)
        {
            return new Vector3(
                Mathf.Round(value.x / grid.x) * grid.x, 
                Mathf.Round(value.y / grid.y) * grid.y, 
                Mathf.Round(value.z / grid.z) * grid.z
            );
        }

        private Vector3 RoundEulerAngle(Vector3 value)
        {
            return new Vector3(
                Mathf.Round(value.x / angle.x) * angle.x, 
                Mathf.Round(value.y / angle.y) * angle.y, 
                Mathf.Round(value.z / angle.z) * angle.z
                );
        }
#endif
    }
}
