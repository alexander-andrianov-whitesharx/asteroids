using System;
using Content.Scripts.Base.Interfaces;
using Content.Scripts.GameCore.Configs;
using Content.Scripts.GameCore.Utils;
using UnityEngine;

namespace Content.Scripts.GameCore.Models
{
    public class UfoModel : IDisposable
    {
        public Action OnPositionUpdate;
        
        private readonly int _reward;
        private readonly float _speed;
        
        private UfoConfig _ufoConfig;
        private Pool<IView> _pool;
        private Vector2 _position;

        public Vector2 Position => _position;
        public int Reward => _reward;
        
        public UfoModel(UfoConfig ufoConfig, Pool<IView> pool)
        {
            _ufoConfig = ufoConfig;
            _pool = pool;
            _reward = ufoConfig.RewardAmount;
            _speed = ufoConfig.Speed;
        }

        public void UpdateUfoPosition(Vector2 ufoPosition, Vector2 playerPosition, float delta)
        {
            var direction = (playerPosition - ufoPosition).normalized;
            var nextPosition = ufoPosition + direction * (_speed * delta);
            _position = nextPosition;
            
            OnPositionUpdate?.Invoke();
        }

        public void Dispose() { }
    }
}
