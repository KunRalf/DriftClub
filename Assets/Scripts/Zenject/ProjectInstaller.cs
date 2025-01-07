using Services;
using UnityEngine;

namespace Zenject
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private MainData _mainData;
        [SerializeField] private MessagesService _messagesService;

        public override void InstallBindings()
        {
            Container.Bind<MainData>().FromInstance(_mainData).AsSingle().NonLazy();
            Container.Bind<MessagesService>().FromInstance(_messagesService).AsSingle().NonLazy();
        }
    }
}