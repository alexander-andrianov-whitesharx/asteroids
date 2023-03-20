using System.Collections.Generic;
using System.Linq;
using Content.Scripts.Base.Enums;
using Content.Scripts.Base.Interfaces;
using Content.Scripts.GameCore.Views;
using Unity.VisualScripting;
using UnityEngine;

namespace Content.Scripts.GameCore.Utils
{
    public class Pool<T>
    {
        private Stack<T> inactive;
        private List<T> active;
        private T prefab;

        public Pool(T prefab, int initialQty)
        {
            this.prefab = prefab;
            inactive = new Stack<T>(initialQty);
            active = new List<T>();
        }

        public T Spawn(Transform parent, Vector3 position, Quaternion rotation)
        {
            T currentObject;
            GameObject currentGameObject;
            
            if (inactive.Count == 0)
            {
                var currentMonoBehaviour = Object.Instantiate(prefab as MonoBehaviour, position, rotation, parent);

                currentGameObject = currentMonoBehaviour.gameObject;
                currentObject = currentGameObject.GetComponent<T>();
                
                var poolMember = currentMonoBehaviour.gameObject.AddComponent<PoolMember>();
                poolMember.Pool = this as Pool<IView>;

                var memberType = GetMemberType(currentObject);
                poolMember.MemberType = memberType;
            }
            else
            {
                currentObject = inactive.Pop();
                active.Add(currentObject);

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
            inactive.Push(currentObject);
            active.Remove(currentObject);
        }
        
        public void DespawnAll()
        {
            foreach (var poolMember in active.ToArray())
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