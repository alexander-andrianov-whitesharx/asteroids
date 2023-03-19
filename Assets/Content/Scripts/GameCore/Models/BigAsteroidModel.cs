using Content.Scripts.Base.Interfaces;
using Content.Scripts.GameCore.Utils;

namespace Content.Scripts.GameCore.Models
{
    public class BigAsteroidModel
    {
        private Pool<IView> _pool;
        
        public BigAsteroidModel(Pool<IView> pool)
        {
            _pool = pool;
        }
    }
}
