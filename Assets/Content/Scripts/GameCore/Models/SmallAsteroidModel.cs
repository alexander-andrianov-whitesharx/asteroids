using System;
using Content.Scripts.GameCore.Configs;
using Content.Scripts.GameCore.Utils;
using UnityEngine;

namespace Content.Scripts.GameCore.Models
{
    public class SmallAsteroidModel : IDisposable
    {
        public Action<Vector2> OnPositionUpdate;

        private readonly PortalService _portalService;
        private readonly Vector2 _direction;

        private readonly int _reward;
        private readonly float _speed;

        private bool _wasInsideViewport;
        
        public int Reward => _reward;

        public SmallAsteroidModel(SmallAsteroidConfig config, PortalService portalService, Vector2 direction)
        {
            _reward = config.RewardAmount;
            _speed = config.Speed;
            _portalService = portalService;
            _direction = direction;
        }

        public void MoveForward(Vector2 position, float delta)
        {
            var nextPosition = position + _direction * (_speed * delta);
            var isOutsideViewport = _portalService.IsNextPosOutsideViewport(nextPosition);

            if (!isOutsideViewport)
            {
                _wasInsideViewport = true;
            }

            if (_wasInsideViewport)
            {
                nextPosition = _portalService.CalculateNextPosition(nextPosition);
            }

            OnPositionUpdate?.Invoke(nextPosition);
        }

        public void Dispose() { }
    }
}
