using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.Configs
{
    [CreateAssetMenu(fileName = "BigAsteroidConfig", menuName = "Game Configs/Big Asteroid")]
    public class BigAsteroidConfig : ScriptableObject
    {
        [SerializeField] private BigAsteroidView asteroidView;

        [SerializeField] private float speed;
        [SerializeField] private int rewardAmount;
        [SerializeField] private int poolSize;
        
        public BigAsteroidView AsteroidView => asteroidView;

        public float Speed => speed;
        public int RewardAmount => rewardAmount;
        public int PoolSize => poolSize;
    }
}