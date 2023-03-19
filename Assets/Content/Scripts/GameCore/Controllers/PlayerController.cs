using Content.Scripts.Base.Interfaces;
using Content.Scripts.GameCore.Models;
using Content.Scripts.GameCore.Utils;
using Content.Scripts.GameCore.Views;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Content.Scripts.GameCore.Controllers
{
    public class PlayerController
    {
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
            _playerView.OnDead += OnViewDeath;
            
            _playerModel.OnPositionUpdate += ModelPositionChanged;
            _playerModel.OnRotationUpdate += ModelRotationChanged;
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
        
        private void OnViewDeath()
        {
            //_game.GameOver();
        }
    }
}