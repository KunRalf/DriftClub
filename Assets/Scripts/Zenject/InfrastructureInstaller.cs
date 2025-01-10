using Helpers.Injector;
using Infrastructure.SaveLoad;

namespace Zenject
{
    public class InfrastructureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISaveLoad>().To<JsonManager>().AsSingle();
            Container.Bind<IPrefabInjector>().To<PrefabInjector>().AsSingle();
            
            Container.Bind<PlayerDataController>().AsSingle().NonLazy();
            Container.Bind<CarSaveLoadController>().AsSingle();
        }
    }
}