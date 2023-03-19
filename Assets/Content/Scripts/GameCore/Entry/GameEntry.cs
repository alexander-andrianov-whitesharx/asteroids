using Content.Scripts.Base.Interfaces;
using Content.Scripts.Configs;
using Content.Scripts.GameCore.Controllers;
using Content.Scripts.GameCore.Models;
using Content.Scripts.GameCore.Utils;
using UnityEngine;

namespace Content.Scripts.GameCore.Entry
{
    public class GameEntry : MonoBehaviour
    {
        private const string PoolsRootName = "PoolsRoot";
        private const string UfoRootName = "UfoRoot";
        private const string SmallAsteroidsRootName = "SmallAsteroidsRoot";
        private const string BigAsteroidsRootName = "BigAsteroidsRoot";
        private const string BulletsRootName = "BulletsRoot";
        private const string LasersRootName = "LasersRoot";
        
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            var localCamera = InitializeSceneComponents();
            var configLoader = new ConfigLoader();
            
            var playerConfig = configLoader.LoadConfig<PlayerConfig>(ResourcesPathService.PlayerConfigPath);
            var ufoConfig = configLoader.LoadConfig<UfoConfig>(ResourcesPathService.UfoConfigPath);
            var smallAsteroidConfig = configLoader.LoadConfig<SmallAsteroidConfig>(ResourcesPathService.SmallAsteroidConfigPath);
            var bigAsteroidConfig = configLoader.LoadConfig<BigAsteroidConfig>(ResourcesPathService.BigAsteroidConfigPath);
            var bulletConfig = configLoader.LoadConfig<BulletConfig>(ResourcesPathService.BulletConfigPath);
            var laserConfig = configLoader.LoadConfig<LaserConfig>(ResourcesPathService.LaserConfigPath);

            var poolsRoot = new GameObject(PoolsRootName).transform;
            var ufoRoot = new GameObject(UfoRootName).transform;
            var smallAsteroidRoot = new GameObject(SmallAsteroidsRootName).transform;
            var bigAsteroidRoot = new GameObject(BigAsteroidsRootName).transform;
            var bulletsRoot = new GameObject(BulletsRootName).transform;
            var lasersRoot = new GameObject(LasersRootName).transform;

            ufoRoot.parent = poolsRoot;
            smallAsteroidRoot.parent = poolsRoot;
            bigAsteroidRoot.parent = poolsRoot;
            bulletsRoot.parent = poolsRoot;
            lasersRoot.parent = poolsRoot;

            var teleportService = new TeleportService(localCamera);
            var poolService = new PoolService<IView>();

            var ufoPool = poolService.Preload(ufoConfig.UfoView, ufoRoot, ufoConfig.PoolSize);
            var smallAsteroidPool = poolService.Preload(smallAsteroidConfig.AsteroidView, smallAsteroidRoot, smallAsteroidConfig.PoolSize);
            var bigAsteroidPool = poolService.Preload(bigAsteroidConfig.AsteroidView, bigAsteroidRoot, bigAsteroidConfig.PoolSize);
            var bulletPool = poolService.Preload(bulletConfig.BulletView, bulletsRoot, bulletConfig.PoolSize);
            var laserPool = poolService.Preload(laserConfig.LaserView, lasersRoot, laserConfig.PoolSize);

            var playerModel = new PlayerModel(playerConfig, teleportService);
            var gunModel = new GunModel(bulletPool, laserPool, bulletsRoot, lasersRoot);
            var gunController = new GunController(gunModel, bulletConfig, teleportService);
            var playerView = Instantiate(playerConfig.PlayerView);
            playerView.Initialize();
            
            var playerController = new PlayerController(playerModel, playerView, gunModel);

            var ufoModel = new UfoModel(ufoPool);
            var smallAsteroidModel = new SmallAsteroidModel(smallAsteroidPool);
            var bigAsteroidModel = new BigAsteroidModel(bigAsteroidPool);
            var enemyModel = new EnemyModel(ufoPool, smallAsteroidPool, bigAsteroidPool, ufoRoot, smallAsteroidRoot, bigAsteroidRoot);
            var enemyController = new EnemyController(ufoModel, smallAsteroidModel, bigAsteroidModel);
        }

        private Camera InitializeSceneComponents()
        {
            var eventSystemPrefab = Resources.Load<GameObject>(ResourcesPathService.EventSystemPath);
            Instantiate(eventSystemPrefab);
            
            var cameraPrefab = Resources.Load<GameObject>(ResourcesPathService.CameraPath);
            var cameraObject = Instantiate(cameraPrefab, new Vector3(0, 0, -10f), Quaternion.identity);

            var canvasPrefab = Resources.Load<GameObject>(ResourcesPathService.CanvasPath);
            var canvasObject = Instantiate(canvasPrefab);
            
            var localCamera = cameraObject.GetComponent<Camera>();
            var canvas = canvasObject.GetComponent<Canvas>();
            
            canvas.worldCamera = localCamera;

            return localCamera;
        }
    }
}
