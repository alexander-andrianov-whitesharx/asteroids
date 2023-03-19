using System;
using Content.Scripts.GameCore.Models;
using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.GameCore.Controllers
{
    public class BulletController
    {
        public Action<BulletController> OnBulletDespawn;
        
        private BulletModel _bulletModel;
        private BulletView _bulletView;

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
    }
}
