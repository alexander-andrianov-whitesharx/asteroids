using System;
using Content.Scripts.Base.Interfaces;
using UnityEngine;

namespace Content.Scripts.GameCore.Views
{
    public class BulletView : MonoBehaviour, IView
    {
        public Action<Vector2, float> OnMove;
        public Action OnDestroy;

        private void Update()
        { 
            OnMove?.Invoke(transform.position, Time.deltaTime);
        }
        
        private void OnTriggerEnter2D(Collider2D trigger)
        {
            var enemy = trigger.GetComponent<IEnemy>();

            if (enemy == null)
            {
                return;
            }
            
            enemy.Destroy();
            OnDestroy?.Invoke();
        }
        
        public void SetPosition(Vector2 position)
        { 
            transform.position = position;
        }
    }
}
