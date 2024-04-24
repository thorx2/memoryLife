using UnityEngine;
using TMPro;

namespace MemDub
{
    public class TopHUD : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text scoreField;

        private string displayText;

        protected void Awake()
        {
            MasterEventBus.GetMasterEventBus.OnPlayerActionDone += OnPlayerActionDone;
            displayText = "Score:0";
        }

        protected void OnEnable()
        {
            scoreField.text = displayText;
        }

        private void OnPlayerActionDone(bool isCorrectAnswer, int finalScore)
        {
            displayText = $"Score:{finalScore}";
            scoreField.text = displayText;
        }
    }
}
