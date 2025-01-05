using Zenject;

namespace Infrastructure.SaveLoad
{
    public class CarSaveLoadController
    {
        private readonly ISaveLoad _saveLoad;
        private const string SAVE_LOAD_PATH = "MyCarsData";
        
        [Inject]
        public CarSaveLoadController(ISaveLoad saveLoad)
        {
            _saveLoad = saveLoad;
        }
    }
}