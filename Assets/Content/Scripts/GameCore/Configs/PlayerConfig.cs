using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.GameCore.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Game Configs/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private PlayerView playerView;
        
        [SerializeField] private float speed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float slowdownTime;
        [SerializeField] private float accelerationTime;
        [SerializeField] private float accelerationLimit;
        [SerializeField] private float laserReload;
        [SerializeField] private int maxLaserAmount;

        public PlayerView PlayerView => playerView;
        
        public float MovementSpeed => speed;
        public float RotationSpeed => rotationSpeed;
        public float SlowdownTime => slowdownTime;
        public float AccelerationTime => accelerationTime;
        public float AccelerationLimit => accelerationLimit;
        public float LaserReload => laserReload;
        public int MaxLaserAmount => maxLaserAmount;
    }
}