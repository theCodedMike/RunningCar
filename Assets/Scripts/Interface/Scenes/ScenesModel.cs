using System.Utility;
using Interface.Transactions;
using UnityEngine;

namespace Interface.Scenes
{
    public enum Scenario
    {
        None, Launch
    }
    
    public class ScenesModel : MonoBehaviour
    {
        [HideInInspector]
        public TransactionsController transactions;
        public float delay = 0.85f;
        
        [Header("Scenarios")]
        public Scenario scenario = Scenario.None;

        public float launchScreenDelay = 1.6f;


        private void Awake()
        {
            transactions = GameObject.Find("Transactions").GetComponent<TransactionsController>();
        }
        
        public string GetPreviousScene() => CustomPlayerPrefs.GetString("previousScene", "MenuScene");
    }
}
