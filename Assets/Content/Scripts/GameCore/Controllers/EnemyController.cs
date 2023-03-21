using System;
using System.Threading;
using System.Threading.Tasks;
using Content.Scripts.GameCore.Configs;
using Content.Scripts.GameCore.Models;
using Content.Scripts.GameCore.Utils;
using Content.Scripts.GameCore.Views;
using UnityEngine;

namespace Content.Scripts.GameCore.Controllers
{
    public class EnemyController
    {
        public Action<Vector2, float> OnUpdatePlayerPosition;
        public Action<int> OnUpdateScore;

        private readonly EnemyModel _enemyModel;
        private readonly UfoConfig _ufoConfig;
        private readonly BigAsteroidConfig _bigAsteroidConfig;
        private readonly SmallAsteroidConfig _smallAsteroidConfig;
        private readonly PortalService _portalService;

        private readonly CancellationTokenSource _cancellationTokenSource;

        private int _generatedUfos = 0;
        private int _generatedAsteroids = 0;

        public CancellationTokenSource TokenSource => _cancellationTokenSource;

        public EnemyController(EnemyModel enemyModel, PlayerView playerView, UfoConfig ufoConfig,
            BigAsteroidConfig bigConfig, SmallAsteroidConfig smallConfig, PortalService portalService)
        {
            _enemyModel = enemyModel;
            _ufoConfig = ufoConfig;
            _bigAsteroidConfig = bigConfig;
            _smallAsteroidConfig = smallConfig;
            _portalService = portalService;

            playerView.OnUpdatePosition += OnUpdatePosition;

            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;

            UfoSpawnRoutine(cancellationToken);
            BigAsteroidsSpawnRoutine(cancellationToken);
        }

        private void OnUpdatePosition(Vector2 playerPosition, float deltaTime)
        {
            OnUpdatePlayerPosition?.Invoke(playerPosition, deltaTime);
        }

        private void GenerateUfo()
        {
            var spawnPoint = _portalService.GenerateRandomSpawnPoint();
            var ufoModel = new UfoModel(_ufoConfig, _enemyModel.UfoPool);
            var ufoView = _enemyModel.UfoPool.Spawn(_enemyModel.UfoRoot, spawnPoint,
                Quaternion.identity);
            var ufoController = new UfoController((UfoView)ufoView, ufoModel, this);

            ufoController.OnDestroyView += DestroyUfo;
        }

        private void GenerateBigAsteroid()
        {
            var spawnPoint = _portalService.GenerateRandomSpawnPoint();
            var direction = _portalService.GenerateDirectionFromSpawnPoint(spawnPoint);
            var asteroidModel = new BigAsteroidModel(_bigAsteroidConfig, _portalService, direction);
            var asteroidView =
                _enemyModel.BigAsteroidsPool.Spawn(_enemyModel.BigAsteroidRoot, spawnPoint, Quaternion.identity);
            var asteroidController = new BigAsteroidController((BigAsteroidView)asteroidView, asteroidModel);
            
            asteroidController.OnDestroyView += DestroyBigAsteroid;
        }
        
        private void ExplodeBigAsteroid(Vector3 upperAxis, Vector2 spawnPoint, Vector2 direction, float angle)
        {
            var axis = Vector3.Cross(direction, upperAxis);
            var leftDirection = Quaternion.AngleAxis(angle, axis) * direction;
            var rightDirection = Quaternion.AngleAxis(-angle, axis) * direction;

            GenerateSmallAsteroid(spawnPoint, leftDirection);
            GenerateSmallAsteroid(spawnPoint, rightDirection);
        }

        private void GenerateSmallAsteroid(Vector2 spawnPoint, Vector2 direction)
        {
            var asteroidModel = new SmallAsteroidModel(_smallAsteroidConfig, _portalService, direction);
            var asteroidView = _enemyModel.SmallAsteroidsPool.Spawn(_enemyModel.SmallAsteroidRoot, spawnPoint, Quaternion.identity);
            var asteroidController = new SmallAsteroidController((SmallAsteroidView)asteroidView, asteroidModel);
            
            asteroidController.OnDestroyView += DestroySmallAsteroid;
        }

        private async void UfoSpawnRoutine(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(TimeSpan.FromSeconds(_ufoConfig.SpawnDelay), token);

                    if (_generatedUfos < _ufoConfig.SpawnLimit)
                    {
                        GenerateUfo();
                        _generatedUfos++;
                    }
                }
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                return;
            }
            catch (Exception)
            {
                return;
            }
            finally
            {
                _cancellationTokenSource?.Dispose();
            }
        }

        private async void BigAsteroidsSpawnRoutine(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                    
                    if (_generatedAsteroids < _bigAsteroidConfig.SpawnLimit)
                    {
                        GenerateBigAsteroid();
                        _generatedAsteroids++;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(_bigAsteroidConfig.SpawnDelay), token);
                }
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                return;
            }
            catch (Exception)
            {
                return;
            }
            finally
            {
                _cancellationTokenSource?.Dispose();
            }
        }

        private void DestroyUfo(UfoView view, UfoController controller, int reward)
        {
            controller.OnDestroyView -= DestroyUfo;
            controller.Dispose();
            
            _enemyModel.UfoPool.Despawn(view);
            _generatedUfos--;
            
            OnUpdateScore?.Invoke(reward);
        }
        
        private void DestroyBigAsteroid(BigAsteroidView view, BigAsteroidController controller, Vector2 direction, int reward)
        {
            var asteroidTransform = view.transform;
            var asteroidPosition = asteroidTransform.position;
            ExplodeBigAsteroid(-asteroidTransform.up,asteroidPosition, direction, _smallAsteroidConfig.Angle);

            controller.OnDestroyView -= DestroyBigAsteroid;
            controller.Dispose();
            
            _enemyModel.BigAsteroidsPool.Despawn(view);
            _generatedAsteroids--;
            
            OnUpdateScore?.Invoke(reward);
        }
        
        private void DestroySmallAsteroid(SmallAsteroidView view, SmallAsteroidController controller, int reward)
        {
            controller.OnDestroyView -= DestroySmallAsteroid;
            controller.Dispose();
            
            _enemyModel.SmallAsteroidsPool.Despawn(view);
            
            OnUpdateScore?.Invoke(reward);
        }
    }
}