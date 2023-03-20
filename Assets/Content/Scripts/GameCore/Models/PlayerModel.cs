using System;
using Content.Scripts.Configs;
using Content.Scripts.GameCore.Utils;
using UnityEngine;

namespace Content.Scripts.GameCore.Models
{
    public class PlayerModel
    {
        public Action OnPositionUpdate;
        public Action OnRotationUpdate;

        private PortalService _portalService;
        
        private Vector2 _position;
        private Vector2 _accelerationDirection;
        private Vector2 _localAcceleration;
        
        private float _movementSpeed;
        private float _rotation;
        private float _rotationSpeed;
        private float _slowdownTime;
        private float _accelerateDuration;
        private float _accelerationMax;
        private int _laserLimit;
        
        public Vector2 AccelerationDirection
        {
            set => _accelerationDirection = value;
        }
        
        public Vector2 Position => _position;

        public float Rotation => _rotation;
        
        public PlayerModel(PlayerConfig config, PortalService portalService)
        {
            _portalService = portalService;
            _movementSpeed = config.MovementSpeed;
            _rotationSpeed = config.RotationSpeed;
            _slowdownTime = config.SlowdownTime;
            _laserLimit = config.MaxLaserAmount;
            _accelerateDuration = config.AccelerationTime;
            _accelerationMax = config.AccelerationLimit;

            _accelerationDirection = new Vector2(0, 1);
        }
        
        public void Accelerate(float deltaTime)
        {
            _localAcceleration += _accelerationDirection * (_accelerateDuration * deltaTime);
            _localAcceleration = _localAcceleration.magnitude >= _accelerationMax
                ? _localAcceleration.normalized * _accelerationMax
                : _localAcceleration;
            
            Move(deltaTime);
        }

        public void Decelerate(float deltaTime)
        {
            _localAcceleration -= _localAcceleration * (deltaTime / _slowdownTime);
            _localAcceleration = _localAcceleration.magnitude < 0 ? _localAcceleration = new Vector2() : _localAcceleration;
            
            Move(deltaTime);
        }

        public void Rotate(float horizontalAxis, float deltaTime)
        {
            var delta = horizontalAxis * _rotationSpeed * deltaTime;
            var rotation = _rotation + delta;
            _rotation = rotation;
            
            OnRotationUpdate?.Invoke();
        }

        private void Move(float deltaTime)
        {
            var newPosition = _position + _localAcceleration * (_movementSpeed * deltaTime);
            _position = newPosition;
            _position = _portalService.CalculateNextPosition(_position);
            
            OnPositionUpdate?.Invoke();
        }
    }
}
