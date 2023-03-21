using System.Collections.Generic;
using Content.Scripts.Base.Enums;
using Content.Scripts.Base.Interfaces;
using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.GameCore.Utils
{
    public class Pool<T>
    {
        private readonly Stack<T> _inactive;
        private readonly List<T> _active;
        private readonly T _prefab;

        public Pool(T prefab, int initialQty)
        {
            _prefab = prefab;
            _inactive = new Stack<T>(initialQty);
            _active = new List<T>();
        }

        public T Spawn(Transform parent, Vector3 position, Quaternion rotation)
        {
            T currentObject;
            GameObject currentGameObject;
            
            if (_inactive.Count == 0)
            {
                var currentMonoBehaviour = Object.Instantiate(_prefab as MonoBehaviour, position, rotation, parent);

                currentGameObject = currentMonoBehaviour.gameObject;
                currentObject = currentGameObject.GetComponent<T>();
                
                var poolMember = currentMonoBehaviour.gameObject.AddComponent<PoolMember>();
                poolMember.Pool = this as Pool<IView>;

                var memberType = GetMemberType(currentObject);
                poolMember.MemberType = memberType;
            }
            else
            {
                currentObject = _inactive.Pop();
                _active.Add(currentObject);

                currentGameObject = (currentObject as MonoBehaviour)!.gameObject;

                if (currentObject == null)
                {
                    return Spawn(parent, position, rotation);
                }
            }
            
            currentGameObject!.transform.position = position;
            currentGameObject!.transform.rotation = rotation;
            currentGameObject!.SetActive(true);

            return currentObject;
        }

        public void Despawn(T currentObject)
        {
            var currentGameObject = currentObject as MonoBehaviour;

            if (currentGameObject == null)
            {
                return;
            }
            
            currentGameObject.gameObject.SetActive(false);
            _inactive.Push(currentObject);
            _active.Remove(currentObject);
        }
        
        public void DespawnAll()
        {
            foreach (var poolMember in _active.ToArray())
            {
                Despawn(poolMember);
            }
        }

        private MemberType GetMemberType(T currentView)
        {
            var isSmall = currentView as SmallAsteroidView;
            var isBig = currentView as BigAsteroidView;
            var isUfo = currentView as UfoView;
            
            if (isSmall != null)
            {
                return MemberType.SmallAsteroid;
            }
            
            if (isBig != null)
            {
                return MemberType.BigAsteroid;
            }
            
            return isUfo != null ? MemberType.Ufo : MemberType.None;
        }
    }
}