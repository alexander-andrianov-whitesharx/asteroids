using System;
using Content.Scripts.Base.Interfaces;
using Content.Scripts.Configs;
using Content.Scripts.GameCore.Utils;
using UnityEngine;

namespace Content.Scripts.GameCore.Models
{
    public class SmallAsteroidModel : IDisposable
    {
        public Action<Vector2> OnPositionUpdate;

        private PortalService _portalService;
        private Vector2 _direction;

        private float _speed;

        private bool _wasInsideViewport;

        public SmallAsteroidModel(SmallAsteroidConfig config, PortalService portalService, Vector2 direction)
        {
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
