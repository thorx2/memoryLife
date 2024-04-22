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
        }

        private void OnPlayerActionDone(bool isCorrectAnswer, int finalScore)
        {
            scoreField.text = $"Score:{finalScore}";
        }
    }
}
