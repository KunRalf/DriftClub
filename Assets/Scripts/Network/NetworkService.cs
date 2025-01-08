using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Network
{
    public class NetworkService : MonoBehaviour, INetworkRunnerCallbacks
    {
        private NetworkRunner _runner;
        private List<SessionInfo> _availableRooms = new List<SessionInfo>();
        
        public async void CreateRoom()
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;
            
            _runner.AddCallbacks(this);

            var startGameArgs = new StartGameArgs()
            {
                GameMode = GameMode.Host,
                SessionName = "Room_" + Random.Range(1000, 9999), 
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
                PlayerCount = 4
            };

            var result = await _runner.StartGame(startGameArgs);

            if (result.Ok)
            {
      
                Debug.Log("Room created successfully!");
            }
            else
            {
                Debug.LogError($"Failed to create room: {result.ErrorMessage}");
            }
        }

        public async void JoinRandomRoom()
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;
            
            _runner.AddCallbacks(this);
            
            Debug.Log("Searching for available rooms...");
            
            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Client,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }
        
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            _availableRooms = sessionList; 

            if (_availableRooms.Count > 0)
            {
                var session = _availableRooms[0];
                Debug.Log($"Joining room: {session.Name}");
                runner.StartGame(new StartGameArgs()
                {
                    GameMode = GameMode.Client,
                    SessionName = session.Name,
                    SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>() 
                });
            }
            else
            {
                Debug.Log("No available rooms found!");
            }
        }
        
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (player == runner.LocalPlayer)
            {
                Debug.Log("You joined the room!");
            }
            else
            {
                Debug.Log($"Player {player.PlayerId} joined the room!");
            }
        }
        
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"Player {player.PlayerId} left the room.");
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