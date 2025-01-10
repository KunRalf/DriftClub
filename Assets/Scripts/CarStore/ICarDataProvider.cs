using System.Collections.Generic;

namespace CarStore
{
    public interface ICarDataProvider
    {
        CarData GetCarById(int carId);
        List<CarData> GetAllCars();
    }
}