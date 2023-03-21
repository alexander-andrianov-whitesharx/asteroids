using System;
using Content.Scripts.GameCore.Models;
using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.GameCore.Controllers
{
    public class BigAsteroidController : IDisposable
    {
        public Action<BigAsteroidView, BigAsteroidController, Vector2, int> OnDestroyView;
        
        private readonly BigAsteroidView _bigAsteroidView;
        private readonly BigAsteroidModel _bigAsteroidModel;
        
        public BigAsteroidController(BigAsteroidView view, BigAsteroidModel model)
        {
            _bigAsteroidView = view;
            _bigAsteroidModel = model;

            InitializeListeners();
        }
        
        private void InitializeListeners()
        {
            _bigAsteroidView.OnMove += OnUpdateViewPosition;
            _bigAsteroidView.OnDestroy += OnDestroy;
            _bigAsteroidModel.OnPositionUpdate += OnUpdateModelPosition;
        }
        
        private void OnUpdateViewPosition(Vector2 position, float delta)
        {
            _bigAsteroidModel.MoveForward(position, delta);
        }

        private void OnUpdateModelPosition(Vector2 position)
        {
            _bigAsteroidView.SetPosition(position);
        }
        
        private void OnDestroy()
        {
            OnDestroyView?.Invoke(_bigAsteroidView, this, _bigAsteroidModel.Direction, _bigAsteroidModel.Reward);
        }

        public void Dispose()
        {
            _bigAsteroidView.OnMove -= OnUpdateViewPosition;
            _bigAsteroidView.OnDestroy -= OnDestroy;
            _bigAsteroidModel.OnPositionUpdate -= OnUpdateModelPosition;

            _bigAsteroidModel.Dispose();
        }
    }
}
