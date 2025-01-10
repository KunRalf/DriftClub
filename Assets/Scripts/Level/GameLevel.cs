using System;
using System.Collections.Generic;
using System.Linq;
using Car;
using CarStore;
using Cinemachine;
using Fusion;
using Helpers.Injector;
using Infrastructure;
using Infrastructure.SaveLoad;
using Network;
using PlayerHub;
using UnityEngine;
using Zenject;

namespace Level
{
    public class GameLevel : NetworkBehaviour
    {
        public static GameLevel CurLevel;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private PlayerInfo _playerInfoPrefab;
        private ICarDataProvider _carDataProvider;
        private PlayerDataController _playerDataController;
        private CarSaveLoadController _carSaveLoadController;
        private NetworkService _networkService;
        private IPrefabInjector _prefabInjector;


        [Inject]
        public void Construct(IPrefabInjector prefabInjector, ICarDataProvider carDataProvider, PlayerDataController playerDataController, CarSaveLoadController carSaveLoadController, NetworkService networkService)
        {
            _prefabInjector = prefabInjector;
            _carDataProvider = carDataProvider;
            _playerDataController = playerDataController;
            _carSaveLoadController = carSaveLoadController;
            _networkService = networkService;
            CurLevel = this;
            SpawnPlayers();
        }

        private void Start()
        {
            // var curCatId = _playerDataController.CurrentCar;
            // if (curCatId == null)
            // {
            //     return;
            // }
            // _mainData.CarsDataList.GetCarById(curCatId.Value);
            // var carData = _mainData.CarsDataList.GetCarById(curCatId.Value);
            // var car = Instantiate(_mainData.CarsDataList.GetCarById(curCatId.Value).CarPrefab, _spawnPoints.position, _spawnPoints.rotation);
            // car.Init(carData.CarStyle,carData.CarParams,car.Id);
            // car.OnUpdatePoints += ChangePlayerCash;
            // car.SetCarStyleParams(_carSaveLoadController.PlayerCarsData.First(_=> _.CarId == carData.Id).Data);
            // car.InitToGame();
            // _camera.Follow = car.CameraFollowTransform;
            // _camera.LookAt = car.CameraFollowTransform;
        }

        private void ChangePlayerCash(int cash)
        {
            var converted = cash / 10;
            _playerDataController.UpdateCash(_playerDataController.Cash + converted);
        }
        
   
        private void SpawnPlayers()
        {
            if(!_networkService.Runner.IsServer) return;
            foreach (var player in NetworkService.Players)
            {
                SpawnCar(player);
            }
          
        }

        private void InitializeObjBeforeSpawn(NetworkRunner runner, NetworkObject obj)
        {
            
        }

        private void SpawnCar(PlayerInfo player)
        {
            var index = NetworkService.Players.IndexOf(player);
            var point = _spawnPoints[index];
            
            var prefab = _carDataProvider.GetCarById(player.CurCarId).CarPrefab;

            // Spawn player
            var car = _networkService.Runner.Spawn(
                prefab,
                point.position,
                point.rotation,
                player.Object.InputAuthority,(NetworkRunner runner, NetworkObject obj) => SetToPlayer(runner, obj,player)
            );
           

           
            
            
            // int spawnIndex = player.PlayerId % _spawnPoints.Count;
            // var spawnPoint = _spawnPoints[spawnIndex];
            // var carData = _carDataProvider.GetCarById(CurCarId);
            // var carPrefab = Runner.Spawn(carData.CarPrefab, spawnPoint.position, spawnPoint.rotation,
            //     Object.InputAuthority);
            // carPrefab.Init(_carDataProvider.GetCarById(CurCarId));
            // carPrefab.InitToGame();
            // if (GetComponent<NetworkObject>().HasStateAuthority)
            // {
            //     _camera.Follow = carPrefab.CameraFollowTransform;
            //     _camera.LookAt = carPrefab.CameraFollowTransform;
            // }
        }

        private void SetToPlayer(NetworkRunner runner, NetworkObject obj, PlayerInfo player)
        {
            _prefabInjector.Inject(obj.gameObject);
            CarMainController car = obj.GetComponent<CarMainController>();
            player.SetCarController(car);
            car.SetCarId(player.CurCarId);
        }

        private void DespawnPlayer(PlayerRef player)
        {
            if (_networkService.Runner.TryGetPlayerObject(player, out var playerObject))
            {
                _networkService.Runner.Despawn(playerObject);
            }
        }

        public void SetPlayerCamera(Transform pivot)
        {
            _camera.Follow = pivot;
            _camera.LookAt = pivot;
        }
        
        public void GetSpawnPoints() => _spawnPoints.ToList();
        
        private void OnDestroy()
        {
            CurLevel = null;
        }
    }
}