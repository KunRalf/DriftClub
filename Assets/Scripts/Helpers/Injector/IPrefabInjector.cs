using UnityEngine;

namespace Helpers.Injector
{
    public interface IPrefabInjector
    {
        void Inject(GameObject prefab);
    }
}