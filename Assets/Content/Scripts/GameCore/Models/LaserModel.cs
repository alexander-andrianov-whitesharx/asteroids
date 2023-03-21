using System;
using Content.Scripts.GameCore.Configs;

namespace Content.Scripts.GameCore.Models
{
    public class LaserModel : IDisposable
    {
        public Action OnLaserAmountUpdated;
        
        private readonly float _duration;

        public float Duration => _duration;
        
        public LaserModel(LaserConfig laserConfig)
        {
            _duration = laserConfig.Duration;
        }

        public void UpdateLaserInfo()
        {
            OnLaserAmountUpdated?.Invoke();
        }

        public void Dispose() { }
    }
}
