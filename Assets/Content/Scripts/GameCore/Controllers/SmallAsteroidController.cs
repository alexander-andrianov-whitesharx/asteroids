using System;
using Content.Scripts.GameCore.Models;
using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.GameCore.Controllers
{
    public class SmallAsteroidController
    {
        public Action<SmallAsteroidView, SmallAsteroidController, int> OnDestroyView;
        
        private readonly SmallAsteroidView _smallAsteroidView;
        private readonly SmallAsteroidModel _smallAsteroidModel;
        
        public SmallAsteroidController(SmallAsteroidView view, SmallAsteroidModel model)
        {
            _smallAsteroidView = view;
            _smallAsteroidModel = model;

            InitializeListeners();
        }
        
        private void InitializeListeners()
        {
            _smallAsteroidView.OnMove += OnUpdateViewPosition;
            _smallAsteroidView.OnDestroy += OnDestroy;
            _smallAsteroidModel.OnPositionUpdate += OnUpdateModelPosition;
        }
        
        private void OnUpdateViewPosition(Vector2 position, float delta)
        {
            _smallAsteroidModel.MoveForward(position, delta);
        }

        private void OnUpdateModelPosition(Vector2 position)
        {
            _smallAsteroidView.SetPosition(position);
        }
        
        private void OnDestroy()
        {
            OnDestroyView?.Invoke(_smallAsteroidView, this, _smallAsteroidModel.Reward);
        }

        public void Dispose()
        {
            _smallAsteroidView.OnMove -= OnUpdateViewPosition;
            _smallAsteroidView.OnDestroy -= OnDestroy;
            _smallAsteroidModel.OnPositionUpdate -= OnUpdateModelPosition;

            _smallAsteroidModel.Dispose();
        }
    }
}
