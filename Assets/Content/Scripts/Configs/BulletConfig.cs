using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.Configs
{
    [CreateAssetMenu(fileName = "BulletConfig", menuName = "Game Configs/Bullet Config")]
    public class BulletConfig : ScriptableObject
    {
        [SerializeField] private BulletView bulletView;

        [SerializeField] private float speed;
        [SerializeField] private int poolSize;

        public BulletView BulletView => bulletView;

        public float Speed => speed;
        public int PoolSize => poolSize;
    }
}