using System;
using UnityEngine;

public struct Online_LoginData
{
    public string email;
    public string password;
}

public struct Online_RegisterUserData
{
    public string displayName;
    public string email;
    public string password;
}

public class Online {
    //Singleton
    private static Online instance;
    private Online() { }
    public static Online Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Online();
            }
            return instance;
        }
    }

    private PlayfabManager playfabManager = new PlayfabManager();


	// Use this for initialization
	void Start () {
		
	}

    /* ----------- User Data ---------- */
    public void UpdateUserData(string key, string json, Action<PlayFab.ClientModels.UpdateUserDataResult> successCallback, Action<PlayFab.PlayFabError> failureCallback = null)
    {
        playfabManager.UpdateUserData(key, json, successCallback, failureCallback);
    }

    public void ChangeUserVirtualCurrency(string currency, int amount, Action<PlayFab.ClientModels.ModifyUserVirtualCurrencyResult> successCallback, Action<PlayFab.PlayFabError> failureCallback = null)
    {
        if (amount > 0)
            playfabManager.AddUserVirtualCurrency(currency, amount, successCallback, failureCallback);
        else
            playfabManager.SubtractUserVirtualCurrency(currency, -amount, successCallback, failureCallback);
    }

    public void GetUserData(string key, uint currentVersion, Action<PlayFab.ClientModels.GetUserDataResult> successCallback, Action<PlayFab.PlayFabError> failureCallback = null)
    {
        playfabManager.GetUserData(key, currentVersion, successCallback, failureCallback);
    }

    public void BuyItem(string storeID, string itemID, string currencyType, int price, Action<PlayFab.ClientModels.PurchaseItemResult> successCallback, Action<PlayFab.PlayFabError> failureCallback = null)
    {
        playfabManager.BuyItem(storeID, itemID, currencyType, price, successCallback, failureCallback);
    }

    /* ----------- Database ----------- */
    public void GetCatalog(string catalog, Action<PlayFab.ClientModels.GetCatalogItemsResult> successCallback, Action<PlayFab.PlayFabError> failureCallback = null)
    {
        playfabManager.GetCatalog(catalog, successCallback, failureCallback);
    }

    public void GetUserInventory(Action<PlayFab.ClientModels.GetUserInventoryResult> successCallback, Action<PlayFab.PlayFabError> failureCallback = null)
    {
        playfabManager.GetInventory(successCallback, failureCallback);
    }

    public void GetStoreItems(string storeID, Action<PlayFab.ClientModels.GetStoreItemsResult> successCallback, Action<PlayFab.PlayFabError> failureCallback = null)
    {
        playfabManager.GetStoreItems(storeID, successCallback, failureCallback);
    }

    /* ------------ LOGIN ------------ */
    public void Login(Online_LoginData data, Action<PlayFab.ClientModels.LoginResult> successCallback, Action<PlayFab.PlayFabError> failureCallback = null)
    {
        playfabManager.Login(data, successCallback, failureCallback);
    }

    public void RegisterUser(Online_RegisterUserData data, Action<PlayFab.ClientModels.RegisterPlayFabUserResult> successCallback, Action<PlayFab.PlayFabError> failureCallback = null)
    {
        playfabManager.RegisterUser(data, successCallback, failureCallback);
    }
    
	
}
