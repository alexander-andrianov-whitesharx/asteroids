using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.GameCore.Configs
{
    [CreateAssetMenu(fileName = "LaserConfig", menuName = "Game Configs/Laser Config")]
    public class LaserConfig : ScriptableObject
    {
        [SerializeField] private LaserView laserView;
        
        [SerializeField] private float duration;
        [SerializeField] private float recoveryTime;
        [SerializeField] private int laserLimit;
        [SerializeField] private int poolSize;

        public LaserView LaserView => laserView;

        public float Duration => duration;
        public float RecoveryTime => recoveryTime;
        public int LaserLimit => laserLimit;
        public int PoolSize => poolSize;
    }
}