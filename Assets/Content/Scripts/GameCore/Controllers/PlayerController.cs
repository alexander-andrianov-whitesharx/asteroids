using System;
using Content.Scripts.Base.Enums;
using Content.Scripts.GameCore.Entry;
using Content.Scripts.GameCore.Models;
using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.GameCore.Controllers
{
    public class PlayerController
    {
        public Action OnDeath;
        
        private PlayerModel _playerModel;
        private PlayerView _playerView;
        private GunModel _gunModel;

        public PlayerController(PlayerModel playerModel, PlayerView playerView, GunModel gunModel)
        {
            _playerModel = playerModel;
            _playerView = playerView;
            _gunModel = gunModel;

            SubscribeOnListeners();
        }

        private void SubscribeOnListeners()
        {
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
            _gunModel.ShootLaser(laserSpawnPosition, _playerModel.Rotation/* - LaserRotationOffset*/);
        }

        private void ModelPositionChanged()
        {
            _playerView.UpdatePosition(_playerModel.Position);
        }
        
        private void ModelRotationChanged()
        { 
            _playerView.UpdateRotation(_playerModel.Rotation);
        }

        private void OnViewDeath(MemberType memberType)
        {
            OnDeath?.Invoke();
        }
    }
}