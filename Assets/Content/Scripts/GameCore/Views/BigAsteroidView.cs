using System;
using Content.Scripts.Base.Interfaces;
using UnityEngine;

namespace Content.Scripts.GameCore.Views
{
    public class BigAsteroidView : MonoBehaviour, IView, IEnemy
    {
        public Action<Vector2, float> OnMove;
        public Action OnDestroy;
        
        private void Update()
        { 
            OnMove?.Invoke(transform.position, Time.deltaTime);
        }
        
        public void SetPosition(Vector2 position)
        { 
            transform.position = position;
        }
        
        public void Destroy()
        {
            OnDestroy?.Invoke();
        }
    }
}
