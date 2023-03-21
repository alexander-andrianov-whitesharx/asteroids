using System.Text;
using TMPro;
using UnityEngine;

namespace Content.Scripts.GameCore.Layouts
{
    public class StatisticsLayout : MonoBehaviour
    {
        private const string PositionPrefix = "player position: ";
        private const string RotationPrefix = "player rotation: ";
        private const string SpeedPrefix = "player speed: ";
        private const string LaserBulletsPrefix = "laser bullets: ";
        private const string LaserRestoreTimePrefix = "laser recovery time: ";

        [SerializeField] private TextMeshProUGUI positionText;
        [SerializeField] private TextMeshProUGUI rotationText;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private TextMeshProUGUI laserBulletsText;
        [SerializeField] private TextMeshProUGUI restoreTimeText;

        public void Initialize(string limitText, string amountText, string restoreText)
        {
            UpdateLaserText(limitText, amountText);
            UpdateRestoreTimeText(restoreText);
        }

        public void UpdatePositionText(string text)
        {
            UpdateText(positionText, PositionPrefix, text);
        }
            
        public void UpdateRotationText(string text)
        {
            UpdateText(rotationText, RotationPrefix, text);
        }
        
        public void UpdateSpeedText(string text)
        {
            UpdateText(speedText, SpeedPrefix, text);
        }
        
        public void UpdateRestoreTimeText(string text)
        {
            UpdateText(restoreTimeText, LaserRestoreTimePrefix, text);
        }
        
        public void UpdateLaserText(string limitText, string amountText)
        {
            var initialString = new StringBuilder(amountText);
            initialString.Append(", ");
            initialString.Append(limitText);
            
            UpdateText(laserBulletsText, LaserBulletsPrefix, initialString.ToString());
        }

        private void UpdateText(TextMeshProUGUI text, string prefix, string value)
        {
            var updatedScoreText = new StringBuilder(prefix);
            updatedScoreText.Append(value);

            text.text = updatedScoreText.ToString();
        }
    }
}
