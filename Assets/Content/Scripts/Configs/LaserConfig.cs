using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.Configs
{
    [CreateAssetMenu(fileName = "LaserConfig", menuName = "Game Configs/Laser Config")]
    public class LaserConfig : ScriptableObject
    {
        [SerializeField] private LaserView laserView;
        
        [SerializeField] private float duration;
        [SerializeField] private int poolSize;

        public LaserView LaserView => laserView;

        public float Duration => duration;
        public int PoolSize => poolSize;
    }
}