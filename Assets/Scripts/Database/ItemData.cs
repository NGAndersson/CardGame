using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card_Data
{
    public string name;
    public string ID;
    public List<int> leftCon;
    public List<int> rightCon;
    public string info;
    public int AP;
    public int MV;
    public int TP;
    public int HP;
    public int Cost;
}

public class Inventory_Item
{
    public Card_Data card;
    public int count = 0;
}
