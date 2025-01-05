namespace Infrastructure.SaveLoad
{
    public class PlayerDataSaveLoadController
    {
        private readonly ISaveLoad _saveLoad;
        private const string SAVE_LOAD_PATH = "PlayerData";

        public PlayerDataSaveLoadController(ISaveLoad saveLoad)
        {
            _saveLoad = saveLoad;
        }
    }
}