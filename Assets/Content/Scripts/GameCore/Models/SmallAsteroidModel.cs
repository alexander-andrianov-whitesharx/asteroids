using Content.Scripts.Base.Interfaces;
using Content.Scripts.GameCore.Utils;

namespace Content.Scripts.GameCore.Models
{
    public class SmallAsteroidModel
    {
        private Pool<IView> _pool;
        
        public SmallAsteroidModel(Pool<IView> pool)
        {
            _pool = pool;
        }
    }
}
