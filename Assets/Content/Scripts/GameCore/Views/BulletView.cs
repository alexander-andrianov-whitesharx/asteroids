using System;
using Content.Scripts.Base.Interfaces;
using UnityEngine;

namespace Content.Scripts.GameCore.Views
{
    public class BulletView : MonoBehaviour, IView
    {
        public Action<Vector2, float> OnMove;

        private Vector2 _direction;

        private void Update()
        { 
            OnMove?.Invoke(transform.position, Time.deltaTime);
        }
        
        public void SetPosition(Vector2 position)
        { 
            transform.position = position;
        }
    }
}
