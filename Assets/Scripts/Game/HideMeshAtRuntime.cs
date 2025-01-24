using UnityEngine;

namespace Game
{
    public class HideMeshAtRuntime : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
