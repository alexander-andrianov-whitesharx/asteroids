using Content.Scripts.GameCore.Models;

namespace Content.Scripts.GameCore.Controllers
{
    public class EnemyController
    {
        private UfoModel _ufoModel;
        private SmallAsteroidModel _smallAsteroidModel;
        private BigAsteroidModel _bigAsteroidModel;
        
        public EnemyController(UfoModel ufoModel, SmallAsteroidModel smallAsteroidModel, BigAsteroidModel bigAsteroidModel)
        {
            _ufoModel = ufoModel;
            _smallAsteroidModel = smallAsteroidModel;
            _bigAsteroidModel = bigAsteroidModel;
        }
    }
}
