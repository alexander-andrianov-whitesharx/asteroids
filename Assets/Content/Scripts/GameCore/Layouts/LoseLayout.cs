using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Scripts.GameCore.Layouts
{
    public class LoseLayout : MonoBehaviour
    {
        private const string ScorePrefix = "your score: ";

        public Action OnRetryClicked;
        
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Button retryButton;

        public void Initialize()
        {
            retryButton.onClick.AddListener(HandleRetryButton);
        }
        
        public void SwitchLayout(bool value)
        {
            var alpha = value ? 1 : 0;

            canvasGroup.alpha = alpha;
            SwitchButtons(value);
        }

        public void UpdateText(int score)
        {
            var updatedScoreText = new StringBuilder(ScorePrefix);
            updatedScoreText.Append(score);

            scoreText.text = updatedScoreText.ToString();
        }

        private void SwitchButtons(bool value)
        {
            retryButton.interactable = value;
        }

        private void HandleRetryButton()
        {
            OnRetryClicked?.Invoke();
        }
    }
}
