using UnityEngine;
using TMPro;

namespace MemDub
{
    public class TopHUD : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text scoreField;

        protected void Start()
        {
            MasterEventBus.GetMasterEventBus.OnPlayerActionDone += OnPlayerActionDone;
            scoreField.text = "Score:0";
        }

        private void OnPlayerActionDone(bool isCorrectAnswer, int finalScore)
        {
            scoreField.text = $"Score:{finalScore}";
        }
    }
}
