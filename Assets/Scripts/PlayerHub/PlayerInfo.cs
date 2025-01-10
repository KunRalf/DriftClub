using Car;
using CarStore;
using Cinemachine;
using Fusion;
using Infrastructure;
using Infrastructure.SaveLoad;
using Network;
using UnityEngine;
using Zenject;

namespace PlayerHub
{
    public class PlayerInfo : NetworkBehaviour
    {
        private PlayerDataController _playerDataController;
        [Networked] public int CurCarId { get; set; } 
        [Networked] public string Name { get; set; } 
        [Networked] public CarMainController CarController { get; set; }


        [Inject]
        public void Construct(PlayerDataController playerDataController)
        {
            _playerDataController = playerDataController;
        }
        
        public override void Spawned()
        {
            NetworkService.Players.Add(this);

            if (Object.HasInputAuthority)
            {
                RPC_SetPlayerStats(_playerDataController.Name, _playerDataController.CurrentCar.Value);
            }
            DontDestroyOnLoad(this);
        }

        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        private void RPC_SetPlayerStats(string username, int kartId)
        {
            Name = username;
            CurCarId = kartId;
        }

        public void SetCarController(CarMainController carController)
        {
            CarController = carController;
        }
    }
}