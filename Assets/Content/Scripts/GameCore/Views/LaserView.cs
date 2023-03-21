using System;
using Content.Scripts.Base.Interfaces;
using UnityEngine;

namespace Content.Scripts.GameCore.Views
{
    public class LaserView : MonoBehaviour, IView
    {
        public Action OnDespawn;

        private float _timer;
        private bool _initialized;

        private void Update()
        {
            if (!_initialized)
            {
                return;
            }
            
            _timer -= Time.deltaTime;
            
            if (_timer <= 0)
            {
                _initialized = false;
                OnDespawn?.Invoke();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D trigger)
        {
            var enemy = trigger.GetComponent<IEnemy>();

            if (enemy == null)
            {
                return;
            }
            
            enemy.Destroy();
        }

        public void Initialize(float timerValue)
        {
            _timer = timerValue;
            _initialized = true;
        }
    }
}