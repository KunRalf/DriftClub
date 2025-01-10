using CarStore;
using Helpers.Injector;
using Infrastructure.SaveLoad;

namespace Infrastructure
{
    public static class ClientProvider
    {
        public static PlayerDataController PlayerDataController { get; set; }
        public static CarSaveLoadController CarSaveLoadController { get; set; }
        public static ICarDataProvider CarDataProvider;
    }
}