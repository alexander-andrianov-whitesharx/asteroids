using System.Collections.Generic;
using Content.Scripts.Base.Interfaces;
using UnityEngine;

namespace Content.Scripts.GameCore.Utils
{
    public class Pool<T>
    {
        private Stack<T> inactive;
        private T prefab;

        public Pool(T prefab, int initialQty)
        {
            this.prefab = prefab;
            inactive = new Stack<T>(initialQty);
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
                
                currentMonoBehaviour.gameObject.AddComponent<PoolMember>().Pool = this as Pool<IView>;
            }
            else
            {
                currentObject = inactive.Pop();
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
        }
        
        public void DespawnAll()
        {
            foreach (var poolMember in inactive)
            {
                var currentGameObject = poolMember as GameObject;

                if (currentGameObject == null)
                {
                    continue;
                }

                Despawn(poolMember);
                //currentGameObject.SetActive(false);
            }
        }
    }
}