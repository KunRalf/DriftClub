using Infrastructure.SaveLoad;

namespace Zenject
{
    public class InfrastructureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISaveLoad>().To<JsonManager>().AsSingle();

            Container.Bind<CarSaveLoadController>().AsSingle();
        }
    }
}