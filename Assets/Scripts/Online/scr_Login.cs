using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class scr_Login : MonoBehaviour {
    public UnityEngine.UI.InputField loginEmailField;
    public UnityEngine.UI.InputField loginPasswordField;
    public UnityEngine.UI.Button loginButton;

    public GameObject accountCreationPanel;
    public UnityEngine.UI.InputField createDisplayNameField;
    public UnityEngine.UI.InputField createEmailField;
    public UnityEngine.UI.InputField createPasswordField;
    public UnityEngine.UI.Button createButton;
    
    public void Start()
    {

    }

    public void OnLoginInputChanged()
    {
        //Check if email structure is correct
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(loginEmailField.text);
        if(match.Success && loginPasswordField.text.Length >= 6)
        {
            loginButton.interactable = true;
            return; //All good
        }
        loginButton.interactable = false;
    }

    public void OnCreateInputChanged()
    {
        //Check if email structure is correct
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(createEmailField.text);

        if (match.Success && createPasswordField.text.Length >= 6 && createDisplayNameField.text != "")
        {
            createButton.interactable = true;
            return; //All good
        }
        createButton.interactable = false;
    }

    public void ToggleCreateAcc()
    {
        accountCreationPanel.SetActive(!accountCreationPanel.activeSelf);
    }

    public void CreateAccount()
    {
        Online_RegisterUserData data = new Online_RegisterUserData {
            displayName = createDisplayNameField.text,
            email = createEmailField.text,
            password = createPasswordField.text
        };

        Online.Instance.RegisterUser(data, Callback_RegisterUserSuccess, Callback_RegisterUserFailure);
    }
    
    private void Callback_RegisterUserSuccess(PlayFab.ClientModels.RegisterPlayFabUserResult result)
    {
        Debug.Log("Playfab ID: " + result.PlayFabId);
        Debug.Log("Congratulations, you created an account!");
        ToggleCreateAcc();
    }

    private void Callback_RegisterUserFailure(PlayFab.PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with Account Creation");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public void LoginButton()
    {
        Online_LoginData data = new Online_LoginData()
        {
            email = loginEmailField.text,
            password = loginPasswordField.text
        };
        Online.Instance.Login(data, Callback_LoginSuccess, Callback_LoginFailure);
    }

    public void DebugLoginButton()
    {
        Online_LoginData data = new Online_LoginData()
        {
            email = "debug@mail.com",
            password = "123456"
        };
        Online.Instance.Login(data, Callback_LoginSuccess, Callback_LoginFailure);
    }

    private void Callback_LoginSuccess(PlayFab.ClientModels.LoginResult result)
    {
        Debug.Log("Playfab ID: " + result.PlayFabId + "\nLast Login Time: " + result.LastLoginTime);

        //Populate database with player-specific information
        Database.Instance.InitPlayer();

        //Go to Main Menu
        MenuManager.Instance.GoToScreen("LoadingScreen");
    }
    private void Callback_LoginFailure(PlayFab.PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with Login");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }    

    public void CreateAccountButton()
    {
        accountCreationPanel.SetActive(true);
    }
}
