using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.Configs
{
    [CreateAssetMenu(fileName = "UfoConfig", menuName = "Game Configs/Ufo Config")]
    public class UfoConfig : ScriptableObject
    {
        [SerializeField] private UfoView ufoView;

        [SerializeField] private float speed;
        [SerializeField] private int rewardAmount;
        [SerializeField] private int poolSize;

        public UfoView UfoView => ufoView;

        public float Speed => speed;
        public int RewardAmount => rewardAmount;
        public int PoolSize => poolSize;
    }
}