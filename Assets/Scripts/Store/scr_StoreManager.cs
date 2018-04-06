using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class scr_StoreManager : MonoBehaviour
{
    private List<PlayFab.ClientModels.StoreItem> storeItems = new List<PlayFab.ClientModels.StoreItem>();
    public GameObject storeContentPanel;
    public UnityEngine.UI.Text goldText;
    public UnityEngine.UI.Text crystalText;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateVirtualCurrencyTexts();
    }

    private void OnEnable()
    {
        // Get all store items
        Online.Instance.GetStoreItems("Cardstore", Callback_GetStoreItemsSuccess);
        goldText.text = Database.Instance.userCurrency["GO"].ToString();
        crystalText.text = Database.Instance.userCurrency["CR"].ToString();
    }

    public void ExitButton()
    {
        MenuManager.Instance.GoToScreen("MainMenu");
    }

    public void GetCurrency(string currency)
    {
        Online.Instance.ChangeUserVirtualCurrency(currency, 100, Callback_ChangeUserVirtualCurrencySuccess);
    }

    private void Callback_ChangeUserVirtualCurrencySuccess(PlayFab.ClientModels.ModifyUserVirtualCurrencyResult result)
    {
        Database.Instance.userCurrency[result.VirtualCurrency] = result.Balance;
    }

    public void UpdateVirtualCurrencyTexts()
    {
        foreach (string currency in Database.Instance.userCurrency.Keys)
        {
            switch (currency)
            {
                case "GO":
                    goldText.text = Database.Instance.userCurrency["GO"].ToString();
                    break;
                case "CR":
                    crystalText.text = Database.Instance.userCurrency["CR"].ToString();
                    break;
            }
        }
    }

    private void Callback_GetStoreItemsSuccess(PlayFab.ClientModels.GetStoreItemsResult result)
    {
        storeItems = result.Store;
        int i = 0;
        //Resize content panel to fit all store items
        GameObject.Find("StoreContent").GetComponent<RectTransform>().sizeDelta = new Vector2(storeItems.Count * 220 + 40, 500);

        //Create items
        foreach (PlayFab.ClientModels.StoreItem item in storeItems)
        {
            GameObject newItem = (GameObject)Instantiate(Resources.Load("Prefabs/Store/Pf_StoreEntry"), storeContentPanel.transform);
            newItem.GetComponent<scr_StoreListing>().Init(item);
            newItem.transform.localPosition = new Vector2(i * 220, 0);
            i++;
        }
    }
}
