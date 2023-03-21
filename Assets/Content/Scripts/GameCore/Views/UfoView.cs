using System;
using Content.Scripts.Base.Interfaces;
using UnityEngine;

namespace Content.Scripts.GameCore.Views
{
    public class UfoView : MonoBehaviour, IView, IEnemy
    {
        public Action OnDestroy;
        
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
