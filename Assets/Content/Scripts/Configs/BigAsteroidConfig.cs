using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.Configs
{
    [CreateAssetMenu(fileName = "BigAsteroidConfig", menuName = "Game Configs/Big Asteroid")]
    public class BigAsteroidConfig : ScriptableObject
    {
        [SerializeField] private BigAsteroidView asteroidView;

        [SerializeField] private float speed;
        [SerializeField] private float spawnDelay;
        [SerializeField] private int rewardAmount;
        [SerializeField] private int spawnLimit;
        [SerializeField] private int poolSize;
        
        public BigAsteroidView AsteroidView => asteroidView;

        public float SpawnDelay => spawnDelay;
        public float Speed => speed;
        public int SpawnLimit => spawnLimit;
        public int RewardAmount => rewardAmount;
        public int PoolSize => poolSize;
    }
}