using Content.Scripts.Configs;
using Content.Scripts.GameCore.Models;
using Content.Scripts.GameCore.Utils;
using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.GameCore.Controllers
{
    public class GunController
    {
        private readonly GunModel _gunModel;
        private readonly BulletConfig _bulletConfig;
        private readonly TeleportService _teleportService;
        
        public GunController(GunModel gunModel, BulletConfig bulletConfig, TeleportService teleportService)
        {
            _gunModel = gunModel;
            _bulletConfig = bulletConfig;
            _teleportService = teleportService;

            InitializeListeners();
        }

        private void InitializeListeners()
        {
            _gunModel.OnShootBullet += ShootBullet;
            _gunModel.OnShootLaser += ShootLaser;
        }

        private void ShootBullet(Transform parent, Vector2 position, Vector2 direction)
        {
            var bulletModel = new BulletModel(_bulletConfig, direction, _teleportService);
            var bulletView = _gunModel.BulletPool.Spawn(parent, position, Quaternion.identity);
            var bulletController = new BulletController(bulletModel, (BulletView)bulletView);

            bulletController.OnBulletDespawn += DespawnBullet;
        }
        
        private void DespawnBullet(BulletController bulletController)
        {
            _gunModel.BulletPool.Despawn(bulletController.BulletView);
            
            bulletController.OnBulletDespawn -= DespawnBullet;
        }
        
        private void ShootLaser(Vector2 laserSpawn, float laserRotation)
        {
            
        }
    }
}
