using System;
using Content.Scripts.Base.Interfaces;
using Content.Scripts.Configs;
using Content.Scripts.GameCore.Utils;
using UnityEngine;

namespace Content.Scripts.GameCore.Models
{
    public class UfoModel : IDisposable
    {
        public Action OnPositionUpdate;
        
        private UfoConfig _ufoConfig;
        private Pool<IView> _pool;
        private Vector2 _position;

        private float _speed;

        public Vector2 Position => _position;
        public float Speed => _speed;
        
        public UfoModel(UfoConfig ufoConfig, Pool<IView> pool)
        {
            _ufoConfig = ufoConfig;
            _pool = pool;
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
