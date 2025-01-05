using System.Collections.Generic;
using System.Linq;
using Car;
using Zenject;

namespace Infrastructure.SaveLoad
{
    public class CarSaveLoadController
    {
        private readonly ISaveLoad _saveLoad;
        private const string SAVE_LOAD_PATH = "MyCarsData";

        public List<PlayerCarData> PlayerCarsData {get; private set;}
        
        [Inject]
        public CarSaveLoadController(ISaveLoad saveLoad)
        {
            _saveLoad = saveLoad;
            PlayerCarsData = saveLoad.Load<List<PlayerCarData>>(SAVE_LOAD_PATH) ?? new List<PlayerCarData>();
        }

        public void Save(PlayerCarData playerCarData)
        {
            if (PlayerCarsData.Any(_ => _.CarId == playerCarData.CarId))
            {
                var data = PlayerCarsData.First(_ => _.CarId == playerCarData.CarId);
                data.Data = playerCarData.Data;
            }
            else
            {
                PlayerCarsData.Add(playerCarData);
            }
            _saveLoad.Save(SAVE_LOAD_PATH, PlayerCarsData);
        }
    }
}