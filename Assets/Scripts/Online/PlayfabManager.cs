using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager
{
    private uint version_InventoryData;

    private void Callback_Generic_Error(PlayFab.PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with PlayFab");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public void GetInventory(System.Action<GetUserInventoryResult> successCallback, System.Action<PlayFabError> failureCallback)
    {
        if (failureCallback == null) failureCallback = Callback_Generic_Error;
        var request = new GetUserInventoryRequest{};
        PlayFabClientAPI.GetUserInventory(request, successCallback, failureCallback);
    }

    public void Login(Online_LoginData data, System.Action<LoginResult> successCallback, System.Action<PlayFabError> failureCallback)
    {
        if (failureCallback == null) failureCallback = Callback_Generic_Error;
        var request = new LoginWithEmailAddressRequest { Email = data.email, Password = data.password};
        PlayFabClientAPI.LoginWithEmailAddress(request, successCallback, failureCallback);
    }

    public void RegisterUser(Online_RegisterUserData data, System.Action<RegisterPlayFabUserResult> successCallback, System.Action<PlayFabError> failureCallback)
    {
        if (failureCallback == null) failureCallback = Callback_Generic_Error;
        var request = new RegisterPlayFabUserRequest { DisplayName = data.displayName, Email = data.email, Password = data.password, RequireBothUsernameAndEmail = false };
        PlayFabClientAPI.RegisterPlayFabUser(request, successCallback, failureCallback);
    }

    public void GetCatalog(string catalog, System.Action<GetCatalogItemsResult> successCallback, System.Action<PlayFabError> failureCallback)
    {
        if (failureCallback == null) failureCallback = Callback_Generic_Error;
        var request = new GetCatalogItemsRequest { CatalogVersion = catalog };
        PlayFabClientAPI.GetCatalogItems(request, successCallback, failureCallback);
    }

    public void UpdateUserData(string key, string jsonData, System.Action<UpdateUserDataResult> successCallback, System.Action<PlayFabError> failureCallback)
    {
        if (failureCallback == null) failureCallback = Callback_Generic_Error;
        Dictionary<string, string> update = new Dictionary<string, string>();
        update[key] = jsonData;
        var request = new UpdateUserDataRequest { Data = update };
        PlayFabClientAPI.UpdateUserData(request, successCallback, failureCallback);
    }

    public void GetUserData(string key, uint currentVersion, System.Action<GetUserDataResult> successCallback, System.Action<PlayFabError> failureCallback)
    {
        if (failureCallback == null) failureCallback = Callback_Generic_Error;
        var request = new GetUserDataRequest { Keys = new List<string>() { key }, IfChangedFromDataVersion = currentVersion};
        PlayFabClientAPI.GetUserData(request, successCallback, failureCallback);
    }

    public void GetStoreItems(string store, System.Action<GetStoreItemsResult> successCallback, System.Action<PlayFabError> failureCallback)
    {
        if (failureCallback == null) failureCallback = Callback_Generic_Error;
        var request = new GetStoreItemsRequest { StoreId = store };
        PlayFabClientAPI.GetStoreItems(request, successCallback, failureCallback);
    }

    public void AddUserVirtualCurrency(string currency, int amount, System.Action<ModifyUserVirtualCurrencyResult> successCallback, System.Action<PlayFabError> failureCallback)
    {
        if (failureCallback == null) failureCallback = Callback_Generic_Error;
        var request = new AddUserVirtualCurrencyRequest { VirtualCurrency = currency, Amount = amount };
        PlayFabClientAPI.AddUserVirtualCurrency(request, successCallback, failureCallback);
    }

    public void SubtractUserVirtualCurrency(string currency, int amount, System.Action<ModifyUserVirtualCurrencyResult> successCallback, System.Action<PlayFabError> failureCallback)
    {
        if (failureCallback == null) failureCallback = Callback_Generic_Error;
        var request = new SubtractUserVirtualCurrencyRequest { VirtualCurrency = currency, Amount = amount };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, successCallback, failureCallback);
    }

    public void BuyItem(string storeID, string itemID, string currency, int price, System.Action<PurchaseItemResult> successCallback, System.Action<PlayFabError> failureCallback)
    {
        if (failureCallback == null) failureCallback = Callback_Generic_Error;
        var request = new PurchaseItemRequest { ItemId = itemID, VirtualCurrency = currency, Price = price, StoreId = storeID};
        PlayFabClientAPI.PurchaseItem(request, successCallback, failureCallback);
    }
}
