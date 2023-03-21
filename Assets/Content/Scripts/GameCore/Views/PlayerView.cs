using System;
using Content.Scripts.Base.Enums;
using Content.Scripts.Base.Interfaces;
using Content.Scripts.GameCore.Utils;
using UnityEngine;

namespace Content.Scripts.GameCore.Views
{
    public class PlayerView : MonoBehaviour, IView
    {
        public Action<float, float, Vector2> OnRotate;
        public Action<float> OnAccelerate;
        public Action<float> OnDecelerate;
        public Action<Vector2> OnLaserShoot;
        public Action OnBulletShoot;
        public Action<Vector2, float> OnUpdatePosition;
        public Action<MemberType> OnDeath;

        private float _axisValue;
        private bool _decelerate = true;
        private bool _rotate;

        private void Update()
        {
            OnUpdatePosition?.Invoke(transform.position, Time.deltaTime);
            
            if (!_decelerate)
            {
                OnAccelerate?.Invoke(Time.deltaTime);
            }
            else
            {
                OnDecelerate?.Invoke(Time.deltaTime);
            }

            if (_rotate)
            {
                OnRotate?.Invoke(_axisValue, Time.deltaTime, transform.up);
            }
        }

        private void OnTriggerEnter2D(Collider2D targetCollider)
        {
            var enemy = targetCollider.GetComponent<PoolMember>();
            
            if (enemy.MemberType != MemberType.None)
            {
                OnDeath?.Invoke(enemy.MemberType);
            }
        }

        public void Initialize()
        {
            var playerControls = new PlayerControls();
            playerControls.PlayerMap.Enable();

            playerControls.PlayerMap.Move.started += _ => { _decelerate = false; };
            playerControls.PlayerMap.Move.canceled += _ => { _decelerate = true; };

            playerControls.PlayerMap.ShootLaser.started += _ => { OnLaserShoot?.Invoke(transform.position); };
            playerControls.PlayerMap.ShootBullet.started += _ => { OnBulletShoot?.Invoke(); };

            playerControls.PlayerMap.Rotate.started += _ => { _rotate = true; };
            playerControls.PlayerMap.Rotate.canceled += _ => { _rotate = false; };
            playerControls.PlayerMap.Rotate.performed += _ => { _axisValue = -_.ReadValue<float>(); };
        }
        
        public void UpdatePosition(Vector2 position)
        { 
            transform.position = position;
        }

        public void UpdateRotation(float rotation)
        { 
            transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        }
    }
}