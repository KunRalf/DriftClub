using Services;
using UnityEngine;

namespace Zenject
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private MainData _mainData;

        public override void InstallBindings()
        {
            Container.Bind<MainData>().FromInstance(_mainData).AsSingle().NonLazy();
        }
    }
}