// // using Photon.Pun;
// using UnityEngine;
//
// namespace Zenject
// {
//     public class NetworkFactory : IFactory<GameObject, Vector3, Quaternion,GameObject> 
//     {
//         private DiContainer _container;
//
//         [Inject]
//         public void Construct(DiContainer container)
//         {
//             _container = container;
//         }
//         
//         // public GameObject Create(GameObject obj, Vector3 position, Quaternion quaternion)
//         // {
//         //     // GameObject prefab = PhotonNetwork.Instantiate(obj.name, position, quaternion);
//         //     _container.InjectGameObject(prefab);
//         //
//         //     return prefab;
//         // }
//     }
// }