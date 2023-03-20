using UnityEngine;

namespace Content.Scripts.GameCore.Utils
{
    public class PortalService
    {
        private const float MinViewportOffset = 1f;
        private const float MaxViewportOffset = 3f;
        
        private readonly float _xViewportLimit;
        private readonly float _yViewportLimit;

        public PortalService(Camera localCamera)
        {
            var xTopPoint = localCamera.ViewportToWorldPoint(new Vector3(1, 0, localCamera.nearClipPlane));
            var yTopPoint = localCamera.ViewportToWorldPoint(new Vector3(0, 1, localCamera.nearClipPlane));

            _xViewportLimit = xTopPoint.x;
            _yViewportLimit = yTopPoint.y;
        }

        public Vector2 CalculateNextPosition(Vector2 currentPosition)
        {
            if (currentPosition.x >= _xViewportLimit)
            {
                currentPosition.x = -_xViewportLimit;
            }
            else if (currentPosition.x <= -_xViewportLimit)
            {
                currentPosition.x = _xViewportLimit;
            }

            if (currentPosition.y >= _yViewportLimit)
            {
                currentPosition.y = -_yViewportLimit;
            }
            else if (currentPosition.y <= -_yViewportLimit)
            {
                currentPosition.y = _yViewportLimit;
            }

            return currentPosition;
        }

        public bool IsNextPosOutsideViewport(Vector2 currentPosition)
        {
            if (currentPosition.x >= _xViewportLimit)
            {
                return true;
            }

            if (currentPosition.x <= -_xViewportLimit)
            {
                return true;
            }

            if (currentPosition.y >= _yViewportLimit)
            {
                return true;
            }

            return currentPosition.y <= -_yViewportLimit;
        }

        public Vector2 GenerateRandomSpawnPoint()
        {
            var xRandom = Random.Range(-_xViewportLimit - MaxViewportOffset, _xViewportLimit + MaxViewportOffset);
            var yUpperRandom = Random.Range(_yViewportLimit + MinViewportOffset, _yViewportLimit + MaxViewportOffset);
            var yLowerRandom = Random.Range(-_yViewportLimit - MinViewportOffset, -_yViewportLimit - MaxViewportOffset);
            
            var tempValue = Random.Range(0, 2);
            var yRandom = tempValue == 0 ? yLowerRandom : yUpperRandom;

            return new Vector2(xRandom, yRandom);
        }
        
        public Vector2 GenerateDirectionFromSpawnPoint(Vector2 spawnPoint)
        {
            var x = spawnPoint.x;
            var y = spawnPoint.y;

            var xValue = x >= 0 ? Random.Range(-_xViewportLimit - MaxViewportOffset, 0) : Random.Range(0, _xViewportLimit + MaxViewportOffset);
            var yValue = y >= 0 ? Random.Range(-_yViewportLimit - MaxViewportOffset, 0) : Random.Range(0, _yViewportLimit + MaxViewportOffset);

            return new Vector2(xValue, yValue).normalized;
        }
    }
}