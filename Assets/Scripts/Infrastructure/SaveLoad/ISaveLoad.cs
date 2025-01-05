namespace Infrastructure.SaveLoad
{
    public interface ISaveLoad
    {
        T Load<T>(string name);
        void Save<T>(string name, T obj);
    }
}