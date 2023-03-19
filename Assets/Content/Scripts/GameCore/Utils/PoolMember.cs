using Content.Scripts.Base.Interfaces;
using UnityEngine;

namespace Content.Scripts.GameCore.Utils
{
    public class PoolMember : MonoBehaviour
    {
        private Pool<IView> _pool;

        public Pool<IView> Pool
        {
            get => _pool;
            set => _pool = value;
        }
    }
}