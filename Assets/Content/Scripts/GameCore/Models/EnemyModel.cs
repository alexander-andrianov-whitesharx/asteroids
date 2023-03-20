using System;
using Content.Scripts.Base.Interfaces;
using Content.Scripts.GameCore.Utils;
using UnityEngine;

namespace Content.Scripts.GameCore.Models
{
    public class EnemyModel
    {
        private Pool<IView> _ufoPool;
        private Pool<IView> _smallAsteroidsPool;
        private Pool<IView> _bigAsteroidsPool;

        private Transform _ufoRoot;
        private Transform _smallAsteroidsRoot;
        private Transform _bigAsteroidsRoot;
        
        private Vector2 _playerPosition;

        private float _delta;

        public Transform UfoRoot => _ufoRoot;
        public Transform BigAsteroidRoot => _bigAsteroidsRoot;
        public Transform SmallAsteroidRoot => _smallAsteroidsRoot;
        public Pool<IView> UfoPool => _ufoPool;
        public Pool<IView> BigAsteroidsPool => _bigAsteroidsPool;
        public Pool<IView> SmallAsteroidsPool => _smallAsteroidsPool;
        
        public EnemyModel(Pool<IView> ufoPool, Pool<IView> smallAsteroidsPool, Pool<IView> bigAsteroidsPool,
            Transform ufoRoot, Transform smallRoot, Transform bigRoot)
        {
            _ufoPool = ufoPool;
            _smallAsteroidsPool = smallAsteroidsPool;
            _bigAsteroidsPool = bigAsteroidsPool;
            _ufoRoot = ufoRoot;
            _smallAsteroidsRoot = smallRoot;
            _bigAsteroidsRoot = bigRoot;
        }
    }
}