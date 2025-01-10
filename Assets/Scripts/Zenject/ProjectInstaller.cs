using CarStore;
using Infrastructure;
using Network;
using Services;
using UnityEngine;

namespace Zenject
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private CarsDataListSO _carsDataList;
        [SerializeField] private MessagesService _messagesService; 
        [SerializeField] private NetworkService _networkService; 

        public override void InstallBindings()
        {
            Container.Bind<ICarDataProvider>().FromInstance(_carsDataList).AsSingle().NonLazy();
            Container.Bind<MessagesService>().FromInstance(_messagesService).AsSingle().NonLazy();
            Container.Bind<NetworkService>().FromInstance(_networkService).AsSingle().NonLazy();
        }
    }
}