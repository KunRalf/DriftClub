using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Sockets;
using Helpers.Injector;
using Level;
using PlayerHub;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace Network
{
    public class NetworkService : MonoBehaviour, INetworkRunnerCallbacks
    {
        public event Action<PlayerRef> PlayerJoined;
        public event Action<PlayerRef> PlayerLeft; 
     
        [SerializeField] private PlayerInfo _playerInfo;
        
        [SerializeField] private SceneReference _garageScene;
        [SerializeField] private List<SceneReference> _levelScenes;
        
        public static List<PlayerInfo> Players = new List<PlayerInfo>();
        
        public NetworkRunner Runner { get; private set; }
        private List<SessionInfo> _availableRooms = new List<SessionInfo>();
        private IPrefabInjector _prefabInjector;

        [Inject]
        public void Construct(IPrefabInjector prefabInjector)
        {
            _prefabInjector = prefabInjector;
        }

        public async void CreateRoom()
        {
            Runner = gameObject.AddComponent<NetworkRunner>();
            Runner.ProvideInput = true;
            
            Runner.AddCallbacks(this);

            var startGameArgs = new StartGameArgs()
            {
                GameMode = GameMode.Host,
                SessionName = "Room_" + Random.Range(1000, 9999), 
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
                PlayerCount = 4
            };

            var res = await Runner.StartGame(startGameArgs);
            if (res.Ok)
            {
              
            }
        }

        public void NextScene()
        {
            Runner.LoadScene(_levelScenes[0]);
        }

        public async void JoinRandomRoom()
        {
            Runner = gameObject.AddComponent<NetworkRunner>();
            Runner.ProvideInput = true;
            
            Runner.AddCallbacks(this);
            
            var res = await Runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Client,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            });
        }
        
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            _availableRooms = sessionList; 

            if (_availableRooms.Count > 0)
            {
                var session = _availableRooms[0];
                var res = runner.StartGame(new StartGameArgs()
                {
                    GameMode = GameMode.Client,
                    SessionName = session.Name,
                    SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>() 
                });
            }
        }
        
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                var roomPlayer = runner.Spawn(_playerInfo, Vector3.zero, Quaternion.identity, player);
                _prefabInjector.Inject(roomPlayer.gameObject);
            }
        }
        
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                PlayerLeft?.Invoke(player); 
                var listPlayer = Players.FirstOrDefault(x => x.Object.InputAuthority == player);
                if (listPlayer != null)
                {
                    if (listPlayer.CarController != null)
                        runner.Despawn(listPlayer.CarController.Object);

                    Players.Remove(listPlayer);
                    runner.Despawn(listPlayer.Object);
                }
            }
        }
        
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }


        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key,
            ArraySegment<byte> data)
        {
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
          
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }
    }
}