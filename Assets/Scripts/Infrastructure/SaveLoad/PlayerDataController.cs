using System;
using System.Collections.Generic;
using System.Linq;
using Car;
using PlayerHub;
using Zenject;

namespace Infrastructure.SaveLoad
{
    public class PlayerDataController
    {
        public event Action<float> OnCashChanged; 
        
        private readonly ISaveLoad _saveLoad;
        private const string SAVE_LOAD_PATH = "PlayerData";

        private PlayerData _playerData;

        public string Name => _playerData.PlayerName;
        public int Cash => _playerData.PlayerCash;
        public int? CurrentCar => _playerData.PlayerCurrentCarId;
        public List<int> OpenedCars => _playerData.OpenedCars.ToList();
        
        [Inject]
        public PlayerDataController(ISaveLoad saveLoad)
        {
            _saveLoad = saveLoad;
            _playerData = saveLoad.Load<PlayerData>(SAVE_LOAD_PATH) ?? new PlayerData();
            ClientProvider.PlayerDataController = this;
        }

        public void AddCar(int carId)
        {
            if(!_playerData.OpenedCars.Contains(carId))
                _playerData.OpenedCars.Add(carId);
            Save();
        }

        public void SetCurrentCar(int carId)
        {
            _playerData.PlayerCurrentCarId = carId;
            Save();
        }

        public void UpdateName(string name)
        {
            _playerData.PlayerName = name;
            Save();
        }
        
        public void UpdateCash(int cash)
        {
            if (cash < 0)
                cash = 0;
            _playerData.PlayerCash = cash;
            OnCashChanged?.Invoke(cash);
            Save();
        }
        
        private void Save()
        {
            _saveLoad.Save(SAVE_LOAD_PATH, _playerData);
            
        }
    }
}