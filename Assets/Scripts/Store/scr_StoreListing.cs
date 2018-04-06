using UnityEngine;
using System.Collections;

public class scr_StoreListing : MonoBehaviour
{
    public int inventoryCount;
    public string currency;
    public PlayFab.ClientModels.StoreItem item;
    public GameObject storeEntry;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(PlayFab.ClientModels.StoreItem newItem)
    {
        item = newItem;
        //Create InventoryCount
        Inventory_Item invCard = Database.Instance.GetInventoryCard(item.ItemId);
        inventoryCount = (invCard == null) ? 0 : invCard.count;
        transform.Find("Pf_StoreInventoryCount").Find("Panel").Find("txt_InventoryCount").GetComponent<UnityEngine.UI.Text>().text = inventoryCount.ToString();

        //Create Card
        transform.Find("Pf_Store_Card").GetComponent<Card_Store>().InitData(Database.Instance.GetDatabaseCard(item.ItemId));

        //Create Cost
        transform.Find("Pf_ItemCost").Find("Panel").Find("txt_Cost").GetComponent<UnityEngine.UI.Text>().text = item.VirtualCurrencyPrices["GO"].ToString();
        
    }

    public void BuyButtonPressed()
    {
        if (Database.Instance.userCurrency["GO"] > item.VirtualCurrencyPrices["GO"])
            Online.Instance.BuyItem("Cardstore", item.ItemId, "GO", (int)item.VirtualCurrencyPrices["GO"], Callback_BuyItemSuccess);
    }

    private void Callback_BuyItemSuccess(PlayFab.ClientModels.PurchaseItemResult result)
    {
        Database.Instance.DownloadAllInventoryCards();
        inventoryCount++;
        transform.Find("Pf_StoreInventoryCount").Find("Panel").Find("txt_InventoryCount").GetComponent<UnityEngine.UI.Text>().text = inventoryCount.ToString();
    }
}
