using Content.Scripts.Base.Enums;
using Content.Scripts.Base.Interfaces;
using UnityEngine;

namespace Content.Scripts.GameCore.Utils
{
    public class PoolMember : MonoBehaviour
    {
        private Pool<IView> _pool;
        private MemberType _memberType;

        public Pool<IView> Pool
        {
            get => _pool;
            set => _pool = value;
        }

        public MemberType MemberType
        {
            get => _memberType;
            set => _memberType = value;
        }
    }
}