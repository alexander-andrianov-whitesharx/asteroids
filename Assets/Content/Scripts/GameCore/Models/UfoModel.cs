using Content.Scripts.Base.Interfaces;
using Content.Scripts.GameCore.Utils;

namespace Content.Scripts.GameCore.Models
{
    public class UfoModel
    {
        private Pool<IView> _pool;
        
        public UfoModel(Pool<IView> pool)
        {
            _pool = pool;
        }
    }
}
