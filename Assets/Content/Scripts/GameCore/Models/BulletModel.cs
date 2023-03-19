using System;
using Content.Scripts.Configs;
using Content.Scripts.GameCore.Utils;
using UnityEngine;

namespace Content.Scripts.GameCore.Models
{
    public class BulletModel
    {
        public Action<Vector2> OnPositionUpdate;
        public Action OnBulletOutsideViewport;

        private TeleportService _teleportService;
        private Vector2 _direction;
        
        private readonly float _speed;
        
        public BulletModel(BulletConfig bulletConfig, Vector2 direction, TeleportService teleportService)
        {
            _speed = bulletConfig.Speed;
            _teleportService = teleportService;
            _direction = direction;
        }
        
        public void MoveForward(Vector2 position, float delta)
        {
            var nextPosition = position + _direction * (_speed * delta);
            var isOutsideViewport = _teleportService.IsNextPosOutsideViewport(nextPosition);

            if (isOutsideViewport)
            {
                OnBulletOutsideViewport?.Invoke();
                return;
            }
            
            OnPositionUpdate?.Invoke(nextPosition);
        }
    }
}
