using System;
using Content.Scripts.GameCore.Configs;
using Content.Scripts.GameCore.Models;
using Content.Scripts.GameCore.Utils;
using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.GameCore.Controllers
{
    public class GunController
    {
        public Action<int, int> OnLaserAmountUpdated;
        public Action<float> OnRestoreTimeUpdated;
        
        private readonly GunModel _gunModel;
        private readonly BulletConfig _bulletConfig;
        private readonly LaserConfig _laserConfig;
        private readonly PortalService _portalService;

        private readonly float _laserCountdownLimit;
        private readonly int _laserLimit;
        
        private float _laserCountdown;
        private int _laserAmount;

        public float LaserCountdownLimit => _laserCountdownLimit;
        public int LaserLimit => _laserLimit;
        public int LaserAmount => _laserAmount;
        
        public GunController(GunModel gunModel, BulletConfig bulletConfig, LaserConfig laserConfig, PortalService portalService)
        {
            _gunModel = gunModel;
            _bulletConfig = bulletConfig;
            _laserConfig = laserConfig;
            _portalService = portalService;
            _laserLimit = laserConfig.LaserLimit;
            _laserAmount = _laserLimit;
            _laserCountdownLimit = laserConfig.RecoveryTime;
            _laserCountdown = _laserCountdownLimit;

            InitializeListeners();
        }

        private void InitializeListeners()
        {
            _gunModel.OnShootBullet += ShootBullet;
            _gunModel.OnShootLaser += ShootLaser;
        }

        public void UpdateLaserCountdown(float delta)
        {
            if (_laserAmount < _laserLimit)
            {
                _laserCountdown -= delta;
            
                if (_laserCountdown <= 0)
                {
                    _laserAmount++;
                    _laserCountdown = _laserCountdownLimit;
                    OnLaserAmountUpdated?.Invoke(_laserLimit, _laserAmount);
                }
                
                OnRestoreTimeUpdated?.Invoke(_laserCountdown);
            }
        }

        private void ShootBullet(Transform parent, Vector2 position, Vector2 direction)
        {
            var bulletModel = new BulletModel(_bulletConfig, direction, _portalService);
            var bulletView = _gunModel.BulletPool.Spawn(parent, position, Quaternion.identity);
            var bulletController = new BulletController(bulletModel, (BulletView)bulletView);

            bulletController.OnBulletDespawn += DespawnBullet;
        }
        
        private void DespawnBullet(BulletController bulletController)
        {
            _gunModel.BulletPool.Despawn(bulletController.BulletView);
            bulletController.Dispose();
            
            bulletController.OnBulletDespawn -= DespawnBullet;
        }
        
        private void ShootLaser(Transform parent, Vector2 laserSpawn, float rotation)
        {
            if (_laserAmount <= 0)
            {
                return;
            }
            
            var laserModel = new LaserModel(_laserConfig);
            var laserIView = _gunModel.LaserPool.Spawn(parent, laserSpawn, Quaternion.Euler(0f, 0f, rotation));
            var laserView = (LaserView)laserIView;
            laserView.Initialize(laserModel.Duration);
            
            var laserController = new LaserController(laserModel, laserView);

            laserModel.OnLaserAmountUpdated += UpdateLaserInfo;
            laserController.OnLaserDespawn += DespawnLaser;
            
            _laserAmount--;
        }
        
        private void DespawnLaser(LaserController laserController, LaserModel laserModel)
        {
            laserModel.OnLaserAmountUpdated -= UpdateLaserInfo;
            laserController.OnLaserDespawn -= DespawnLaser;
            
            _gunModel.LaserPool.Despawn(laserController.LaserView);
            laserController.Dispose();
        }

        private void UpdateLaserInfo()
        {
            OnLaserAmountUpdated?.Invoke(_laserLimit, _laserAmount);
        }
    }
}
