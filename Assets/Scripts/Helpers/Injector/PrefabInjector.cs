using UnityEngine;
using Zenject;

namespace Helpers.Injector
{
    public class PrefabInjector : IPrefabInjector
    {
        private readonly DiContainer _container;


        public PrefabInjector(DiContainer container)
        {
            _container = container;
        }
        
        public void Inject(GameObject prefab)
        {
            _container.InjectGameObject(prefab);
        }
    }
}