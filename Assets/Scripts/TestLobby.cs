using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.Netcode;
using UnityEngine;

namespace DEVSOC2024
{
    public class TestLobby : NetworkBehaviour
    {
        public void Host()
        {
            NetworkManager.Singleton.StartHost();
        }
        public void Scene()
        {
            NetworkManager.SceneManager.LoadScene("GautamProtoype",UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
