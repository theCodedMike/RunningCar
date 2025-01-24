using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.Indicator
{
    public class IndicatorsModel : MonoBehaviour
    {
        public ObjectsHolder objectsHolder;
        public GameData gameData;

        [HideInInspector]
        public Text textScore;
        [HideInInspector]
        public Text textScoreResult;
        [HideInInspector]
        public Text textScoreResultBest;
        [HideInInspector]
        public Text textCoin;

        [HideInInspector]
        public string defaultStringScore;
        [HideInInspector]
        public string defaultStringScoreResult;
        [HideInInspector]
        public string defaultStringScoreResultBest;
        [HideInInspector]
        public string defaultStringCoin;


        private void Awake()
        {
            GetComponents();
        }

        private void GetComponents()
        {
            if (GameObject.Find("TextScore"))
            {
                textScore = GameObject.Find("TextScore").GetComponent<Text>();
                defaultStringScore = textScore.text;
            }
            if (GameObject.Find("TextScoreResult"))
            {
                textScoreResult = GameObject.Find("TextScoreResult").GetComponent<Text>();
                defaultStringScoreResult = textScoreResult.text;
            }
            if (GameObject.Find("TextScoreResultBest"))
            {
                textScoreResultBest = GameObject.Find("TextScoreResultBest").GetComponent<Text>();
                defaultStringScoreResultBest = textScoreResultBest.text;
            }
            if (GameObject.Find("TextCoin"))
            {
                textCoin = GameObject.Find("TextCoin").GetComponent<Text>();
                defaultStringCoin = textCoin.text;
            }
        }
    }
}
