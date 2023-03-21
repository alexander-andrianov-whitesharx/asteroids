using System;
using Content.Scripts.GameCore.Models;
using Content.Scripts.GameCore.Views;

namespace Content.Scripts.GameCore.Controllers
{
    public class LaserController : IDisposable
    {
        public Action<LaserController, LaserModel> OnLaserDespawn;
        
        private readonly LaserModel _laserModel;
        private readonly LaserView _laserView;
        
        public LaserView LaserView => _laserView;
        
        public LaserController(LaserModel model, LaserView view)
        {
            _laserModel = model;
            _laserView = view;
            
            InitializeListeners();
        }
        
        private void InitializeListeners()
        {
            _laserView.OnDespawn += OnDespawn;
        }
        
        private void OnDespawn()
        {
            _laserModel.UpdateLaserInfo();
            OnLaserDespawn?.Invoke(this, _laserModel);
        }
        
        public void Dispose()
        {
            _laserView.OnDespawn -= OnDespawn;
            _laserModel.Dispose();
        }
    }
}
