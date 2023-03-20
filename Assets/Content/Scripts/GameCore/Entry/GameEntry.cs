using System;
using Content.Scripts.Base.Interfaces;
using Content.Scripts.Configs;
using Content.Scripts.GameCore.Controllers;
using Content.Scripts.GameCore.Layouts;
using Content.Scripts.GameCore.Models;
using Content.Scripts.GameCore.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private PoolService<IView> _localPoolService;
        private EnemyController _localEnemyController;
        private PlayerController _localPlayerController;

        private LoseLayout _loseLayout;
        private StatisticsLayout _statisticsLayout;

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
            var smallAsteroidConfig =
                configLoader.LoadConfig<SmallAsteroidConfig>(ResourcesPathService.SmallAsteroidConfigPath);
            var bigAsteroidConfig =
                configLoader.LoadConfig<BigAsteroidConfig>(ResourcesPathService.BigAsteroidConfigPath);
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

            var portalService = new PortalService(localCamera);
            _localPoolService = new PoolService<IView>();

            var ufoPool = _localPoolService.Preload(ufoConfig.UfoView, ufoRoot, ufoConfig.PoolSize);
            var smallAsteroidPool = _localPoolService.Preload(smallAsteroidConfig.AsteroidView, smallAsteroidRoot,
                smallAsteroidConfig.PoolSize);
            var bigAsteroidPool = _localPoolService.Preload(bigAsteroidConfig.AsteroidView, bigAsteroidRoot,
                bigAsteroidConfig.PoolSize);
            var bulletPool = _localPoolService.Preload(bulletConfig.BulletView, bulletsRoot, bulletConfig.PoolSize);
            var laserPool = _localPoolService.Preload(laserConfig.LaserView, lasersRoot, laserConfig.PoolSize);

            var playerModel = new PlayerModel(playerConfig, portalService);
            var gunModel = new GunModel(bulletPool, laserPool, bulletsRoot, lasersRoot);
            var gunController = new GunController(gunModel, bulletConfig, portalService);
            var playerView = Instantiate(playerConfig.PlayerView);
            playerView.Initialize();

            _localPlayerController = new PlayerController(playerModel, playerView, gunModel);

            var enemyModel = new EnemyModel(ufoPool, smallAsteroidPool, bigAsteroidPool, ufoRoot, smallAsteroidRoot,
                bigAsteroidRoot);
            _localEnemyController = new EnemyController(enemyModel, playerView, ufoConfig, bigAsteroidConfig,
                smallAsteroidConfig, portalService);

            InitializeGameListeners(_localPlayerController);
        }

        private Camera InitializeSceneComponents()
        {
            var eventSystemPrefab = Resources.Load<GameObject>(ResourcesPathService.EventSystemPath);
            Instantiate(eventSystemPrefab);

            var cameraPrefab = Resources.Load<GameObject>(ResourcesPathService.CameraPath);
            var cameraObject = Instantiate(cameraPrefab, new Vector3(0, 0, -10f), Quaternion.identity);

            var canvasPrefab = Resources.Load<GameObject>(ResourcesPathService.CanvasPath);
            var canvasObject = Instantiate(canvasPrefab);
            var loseLayout = canvasObject.transform.GetComponentInChildren<LoseLayout>();
            var statisticsLayout = canvasObject.transform.GetComponentInChildren<StatisticsLayout>();

            var localCamera = cameraObject.GetComponent<Camera>();
            var canvas = canvasObject.GetComponent<Canvas>();

            canvas.worldCamera = localCamera;
            _loseLayout = loseLayout;
            _statisticsLayout = statisticsLayout;

            _loseLayout.Initialize();
            _loseLayout.OnRetryClicked += RestartGame;

            return localCamera;
        }

        private void InitializeGameListeners(PlayerController player)
        {
            player.OnDeath += ShowDeath;
        }

        private void RestartGame()
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        private void ShowDeath()
        {
            _loseLayout.SwitchLayout(true);
            _localPlayerController.UnsubscribeFromListeners();
            _localEnemyController.TokenSource.Cancel();
            _localPoolService.DespawnAll();
        }

        private void OnApplicationQuit()
        {
            try
            {
                _localPoolService.DespawnAll();
                _localEnemyController.TokenSource?.Cancel();
            }
            catch (ObjectDisposedException) { }
        }
    }
}