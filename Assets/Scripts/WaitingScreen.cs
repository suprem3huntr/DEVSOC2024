using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace DEVSOC2024
{
    public class WaitingScreen : MonoBehaviour
    {
        
        ScenesController scenesController;
        [SerializeField]
        TMP_Text LobbyCode;
        void Awake()
        {
            
        }
        void Start()
        {
            scenesController = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<ScenesController>();
            Debug.Log(scenesController);
            LobbyCode.text = scenesController.joinCode;
        }
        // Update is called once per frame
        void Update()
        {
        
        }

        public void StartGame()
        {
            
            scenesController.StartGame();
        }
    }
}
