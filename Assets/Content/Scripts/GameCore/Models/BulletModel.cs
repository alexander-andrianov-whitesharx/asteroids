using System;
using Content.Scripts.Configs;
using Content.Scripts.GameCore.Utils;
using UnityEngine;

namespace Content.Scripts.GameCore.Models
{
    public class BulletModel : IDisposable
    {
        public Action<Vector2> OnPositionUpdate;
        public Action OnBulletOutsideViewport;

        private readonly PortalService _portalService;
        private readonly Vector2 _direction;
        
        private readonly float _speed;
        
        public BulletModel(BulletConfig bulletConfig, Vector2 direction, PortalService portalService)
        {
            _speed = bulletConfig.Speed;
            _portalService = portalService;
            _direction = direction;
        }
        
        public void MoveForward(Vector2 position, float delta)
        {
            var nextPosition = position + _direction * (_speed * delta);
            var isOutsideViewport = _portalService.IsNextPosOutsideViewport(nextPosition);

            if (isOutsideViewport)
            {
                OnBulletOutsideViewport?.Invoke();
                return;
            }
            
            OnPositionUpdate?.Invoke(nextPosition);
        }

        public void Dispose() { }
    }
}
