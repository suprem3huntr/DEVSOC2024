using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DEVSOC2024
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField]
        TMP_InputField lobbyCode;
        ScenesController scenesController;
        void Awake()
        {
            scenesController = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<ScenesController>();
        }

        public void Host()
        {
            scenesController = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<ScenesController>();
            scenesController.Host();
        }
        public void Join()
        {
            scenesController.Join(lobbyCode.text);
        }
    }
}
