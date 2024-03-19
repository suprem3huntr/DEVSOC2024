using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DEVSOC2024
{
    public class uiManager : MonoBehaviour
    {
        public static uiManager instance;

    //Screen object variables
        public GameObject loginUI;
        public GameObject registerUI;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != null)
            {
                Debug.Log("Instance already exists, destroying object!");
                Destroy(this);
            }
        }

        //Functions to change the login screen UI
        public void LoginScreen() //Back button
        {
            loginUI.SetActive(true);
            registerUI.SetActive(false);
        }
        public void RegisterScreen() // Regester button
        {
            loginUI.SetActive(false);
            registerUI.SetActive(true);
        }

        public void LobbyScreen()
        {
            loginUI.SetActive(false);
            SceneManager.LoadScene("Lobby");
        }
    }
}
