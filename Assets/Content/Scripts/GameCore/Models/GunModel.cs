using System;
using Content.Scripts.Base.Interfaces;
using Content.Scripts.GameCore.Utils;
using UnityEngine;

namespace Content.Scripts.GameCore.Models
{
    public class GunModel
    {
        public Action<Transform, Vector2, Vector2> OnShootBullet;
        public Action<Transform, Vector2, float> OnShootLaser;
        
        private readonly Pool<IView> _bulletPool;
        private readonly Pool<IView> _laserPool;
        private readonly Transform _bulletsRoot;
        private readonly Transform _lasersRoot;

        public Pool<IView> BulletPool => _bulletPool;
        public Pool<IView> LaserPool => _laserPool;

        public GunModel(Pool<IView> bulletPool, Pool<IView> laserPool, Transform bulletsRoot, Transform lasersRoot)
        {
            _bulletPool = bulletPool;
            _laserPool = laserPool;

            _bulletsRoot = bulletsRoot;
            _lasersRoot = lasersRoot;
        }

        public void ShootBullet(Vector2 position, Vector2 direction)
        {
            OnShootBullet?.Invoke(_bulletsRoot, position, direction);
        }
        
        public void ShootLaser(Vector2 laserSpawn, float rotation)
        {
            OnShootLaser?.Invoke(_lasersRoot, laserSpawn, rotation);
        }
    }
}
