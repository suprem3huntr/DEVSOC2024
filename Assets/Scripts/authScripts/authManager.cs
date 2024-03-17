using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;

public class authManager : MonoBehaviour
{
    public TMP_Text loginText;

    async void Start()
    {
        await UnityServices.InitializeAsync();
    }



    public async void signIn()
    {
        await anonymousSignIn();
    }

    async Task anonymousSignIn()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            print("Sign in successful.");
            print("Player ID : " + AuthenticationService.Instance.PlayerId);
            loginText.text = "Player ID : " + AuthenticationService.Instance.PlayerId;
        }
        catch (AuthenticationException ex)
        {
            print("Sign in Failed.");
            Debug.LogException(ex);
        }
    }
}
