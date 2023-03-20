using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.Configs
{
    [CreateAssetMenu(fileName = "SmallAsteroidConfig", menuName = "Game Configs/Small Asteroid")]
    public class SmallAsteroidConfig : ScriptableObject
    {
        [SerializeField] private SmallAsteroidView asteroidView;

        [SerializeField] private float speed;
        [SerializeField] private int rewardAmount;
        [SerializeField] private int poolSize;
        
        public SmallAsteroidView AsteroidView => asteroidView;

        public float Speed => speed;
        public int RewardAmount => rewardAmount;
        public int PoolSize => poolSize;
    }
}