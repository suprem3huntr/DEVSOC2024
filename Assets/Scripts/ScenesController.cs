using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Networking.Transport.Relay;

namespace DEVSOC2024
{
    public class ScenesController : NetworkBehaviour
    {
        public string joinCode;

        void Awake()
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Data");

            if (objs.Length > 1)
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);
        }
        async void Start()
        {
            NetworkManager.Singleton.OnServerStarted+=HandleServerStarted;
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        private void HandleServerStarted() 
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Waiting",LoadSceneMode.Single);
        }

        public async void Host()
        {
            try
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);

                joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                Debug.Log(joinCode);
                
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                    allocation.RelayServer.IpV4,
                    (ushort)allocation.RelayServer.Port,
                    allocation.AllocationIdBytes,
                    allocation.Key,
                    allocation.ConnectionData

                );
                NetworkManager.Singleton.StartHost();
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
            }
        }

        public async void Join(string joinCodeinp)
        {
            joinCode = joinCodeinp;
            try
            {
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                    joinAllocation.RelayServer.IpV4,
                    (ushort) joinAllocation.RelayServer.Port,
                    joinAllocation.AllocationIdBytes,
                    joinAllocation.Key,
                    joinAllocation.ConnectionData,
                    joinAllocation.HostConnectionData
                );

                NetworkManager.Singleton.StartClient();
            }
            catch(RelayServiceException e)
            {
                Debug.LogError(e);
            }
        }

        public void StartGame()
        {
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene",LoadSceneMode.Single);
        }
    }
}
