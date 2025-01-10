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

        private List<PlayerCarData> PlayerCarsData;
        
        [Inject]
        public CarSaveLoadController(ISaveLoad saveLoad)
        {
            _saveLoad = saveLoad;
            PlayerCarsData = saveLoad.Load<List<PlayerCarData>>(SAVE_LOAD_PATH) ?? new List<PlayerCarData>();
            ClientProvider.CarSaveLoadController = this;
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

        public PlayerCarData GetPlayerCarById(int id)
        {
            var car = PlayerCarsData.FirstOrDefault(_ => _.CarId == id);
            return car;
        }

        public void AddPurchasedDetails(int carId, CarDetailsEnum detail)
        {
            var carData = PlayerCarsData.FirstOrDefault(_ => _.CarId == carId);
            if (carData == default) return;
            if (!carData.Data.PurchasedDetails.Contains(detail))
            {
                carData.Data.PurchasedDetails.Add(detail);
            }

            Save(carData);
        } 
    }
}