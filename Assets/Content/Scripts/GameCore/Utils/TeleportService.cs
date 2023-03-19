using UnityEngine;

namespace Content.Scripts.GameCore.Utils
{
    public class TeleportService
    {
        private readonly float _xViewportLimit;
        private readonly float _yViewportLimit;

        public TeleportService(Camera localCamera)
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
    }
}