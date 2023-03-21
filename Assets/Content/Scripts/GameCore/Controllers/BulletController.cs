using System;
using Content.Scripts.GameCore.Models;
using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.GameCore.Controllers
{
    public class BulletController : IDisposable
    {
        public Action<BulletController> OnBulletDespawn;
        
        private readonly BulletModel _bulletModel;
        private readonly BulletView _bulletView;

        public BulletView BulletView => _bulletView;
        
        public BulletController(BulletModel model, BulletView view)
        {
            _bulletModel = model;
            _bulletView = view;
            
            InitializeListeners();
        }

        private void InitializeListeners()
        {
            _bulletView.OnMove += OnUpdateViewPosition;
            _bulletView.OnDestroy += OnBulletOutsideViewport;
            _bulletModel.OnPositionUpdate += OnUpdateModelPosition;
            _bulletModel.OnBulletOutsideViewport += OnBulletOutsideViewport;
        }

        private void OnUpdateViewPosition(Vector2 position, float delta)
        {
            _bulletModel.MoveForward(position, delta);
        }

        private void OnUpdateModelPosition(Vector2 position)
        { 
            _bulletView.SetPosition(position);
        }
        
        private void OnBulletOutsideViewport()
        { 
            OnBulletDespawn?.Invoke(this);
        }

        public void Dispose()
        {
            _bulletView.OnMove -= OnUpdateViewPosition;
            _bulletView.OnDestroy -= OnBulletOutsideViewport;
            _bulletModel.OnPositionUpdate -= OnUpdateModelPosition;
            _bulletModel.OnBulletOutsideViewport -= OnBulletOutsideViewport;

            _bulletModel.Dispose();
        }
    }
}
