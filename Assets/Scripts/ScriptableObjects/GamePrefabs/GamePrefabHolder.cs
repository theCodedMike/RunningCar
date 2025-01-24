using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.GamePrefabs
{
    [CreateAssetMenu(fileName = "GamePrefabHolder", menuName = "RunningCar/Level/GamePrefabHolder", order = 25)]
    public class GamePrefabHolder : ScriptableObject
    {
        public List<GamePrefab> basic;
        public List<GamePrefab> grounds;
        public List<GamePrefab> other;
    }
}
