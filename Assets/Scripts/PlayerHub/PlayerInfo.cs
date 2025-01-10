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
        [Networked] public int CurCarId { get; set; } 
        [Networked] public string Name { get; set; } 
        [Networked] public CarMainController CarController { get; set; }

        
        public override void Spawned()
        {
            NetworkService.Players.Add(this);

            if (Object.HasInputAuthority)
            {
                RPC_SetPlayerStats(ClientProvider.PlayerDataController.Name, ClientProvider.PlayerDataController.CurrentCar.Value);
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