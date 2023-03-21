using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.GameCore.Configs
{
    [CreateAssetMenu(fileName = "SmallAsteroidConfig", menuName = "Game Configs/Small Asteroid")]
    public class SmallAsteroidConfig : ScriptableObject
    {
        [SerializeField] private SmallAsteroidView asteroidView;

        [SerializeField] private float speed;
        [SerializeField] private float angle;
        [SerializeField] private int rewardAmount;
        [SerializeField] private int poolSize;
        
        public SmallAsteroidView AsteroidView => asteroidView;

        public float Speed => speed;
        public float Angle => angle;
        public int RewardAmount => rewardAmount;
        public int PoolSize => poolSize;
    }
}