using System;
using Content.Scripts.Base.Enums;
using Content.Scripts.GameCore.Models;
using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.GameCore.Controllers
{
    public class PlayerController
    {
        public Action OnDeath;
        public Action<Vector2> OnPositionUpdate;
        public Action<float> OnRotationUpdate;
        public Action<float> OnSpeedUpdate;
        
        private readonly PlayerModel _playerModel;
        private readonly PlayerView _playerView;
        private readonly GunModel _gunModel;
        private readonly GunController _gunController;
        
        private Vector2 _lastPlayerPosition;
        
        public PlayerController(PlayerModel playerModel, PlayerView playerView, GunModel gunModel, GunController gunController)
        {
            _playerModel = playerModel;
            _playerView = playerView;
            _gunModel = gunModel;
            _gunController = gunController;

            SubscribeOnListeners();
        }

        private void SubscribeOnListeners()
        {
            _playerView.OnUpdatePosition += OnUpdate;
            _playerView.OnBulletShoot += OnUpdateModelBullet;
            _playerView.OnLaserShoot += OnUpdateModelLaser;
            _playerView.OnAccelerate += OnUpdateModelAcceleration;
            _playerView.OnDecelerate += OnUpdateModelDeceleration;
            _playerView.OnRotate += OnUpdateModelRotation;
            _playerView.OnDeath += OnViewDeath;
            
            _playerModel.OnPositionUpdate += ModelPositionChanged;
            _playerModel.OnRotationUpdate += ModelRotationChanged;
        }
        
        public void UnsubscribeFromListeners()
        {
            _playerView.OnBulletShoot -= OnUpdateModelBullet;
            _playerView.OnLaserShoot -= OnUpdateModelLaser;
            _playerView.OnAccelerate -= OnUpdateModelAcceleration;
            _playerView.OnDecelerate -= OnUpdateModelDeceleration;
            _playerView.OnRotate -= OnUpdateModelRotation;
            _playerView.OnDeath -= OnViewDeath;
            
            _playerModel.OnPositionUpdate -= ModelPositionChanged;
            _playerModel.OnRotationUpdate -= ModelRotationChanged;
        }

        private void OnUpdate(Vector2 position, float delta)
        {
            _gunController.UpdateLaserCountdown(delta);
        }

        private void OnUpdateModelAcceleration(float deltaTime)
        {
            _playerModel.Accelerate(deltaTime);
        }

        private void OnUpdateModelDeceleration(float deltaTime)
        {
            _playerModel.Decelerate(deltaTime);
        }

        private void OnUpdateModelRotation(float horizontalAxis, float deltaTime, Vector2 moveDirection)
        {
            _playerModel.Rotate(horizontalAxis, deltaTime);
            _playerModel.AccelerationDirection = moveDirection;
        }

        private void OnUpdateModelBullet()
        {
            _gunModel.ShootBullet(_playerModel.Position, _playerView.transform.up);
        }

        private void OnUpdateModelLaser(Vector2 laserSpawnPosition)
        {
            _gunModel.ShootLaser(laserSpawnPosition, _playerModel.Rotation);
        }

        private void ModelPositionChanged(float delta)
        {
            _playerView.UpdatePosition(_playerModel.Position);
            
            var speed = Vector3.Distance(_playerModel.Position, _lastPlayerPosition) / delta;
            _lastPlayerPosition = _playerModel.Position;
            
            OnSpeedUpdate?.Invoke(speed);
            OnPositionUpdate?.Invoke(_playerModel.Position);
        }
        
        private void ModelRotationChanged()
        { 
            _playerView.UpdateRotation(_playerModel.Rotation);
            OnRotationUpdate?.Invoke(_playerModel.Rotation);
        }

        private void OnViewDeath(MemberType memberType)
        {
            OnDeath?.Invoke();
        }
    }
}