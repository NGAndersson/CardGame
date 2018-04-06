using UnityEngine;
using System.Collections.Generic;

public class Card : MonoBehaviour
{
    public Card_Data data = new Card_Data();
    private GameObject visuals;
    private Color[] colors = new Color[3] {
        new Color(1,0,0),
        new Color(0,1,0),
        new Color(0,0,1)
    };


    // Use this for initialization
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {

    }

    public void InitData(Card_Data baseData)
    {
        data.name = baseData.name;
        data.info = baseData.info;
        data.AP = baseData.AP;
        data.HP = baseData.HP;
        data.TP = baseData.TP;
        data.MV = baseData.MV;
        data.ID = baseData.ID;
        data.Cost = baseData.Cost;
        foreach (int i in baseData.leftCon)
            data.leftCon.Add(i);
        data.leftCon.Sort();
        foreach (int i in baseData.rightCon)
            data.rightCon.Add(i);
        data.rightCon.Sort();

        visuals = (GameObject)Instantiate(Resources.Load("Prefabs/Cards/Pf_Card_Visuals"), transform);
        Transform visualsT = visuals.transform;
        visualsT.SetSiblingIndex(0);

        //Connection markers
        List<int>[] connection = new List<int>[2] { data.leftCon, data.rightCon };
        Transform[] conPanels = new Transform[2] { visualsT.Find("Con_Left"), visualsT.Find("Con_Right") };
        for (int j = 0; j < 2; j++)
        {
            float panelHeight = conPanels[j].GetComponent<RectTransform>().sizeDelta.y;
            int conIndex = 0;
            foreach (int color in connection[j])
            {
                //Create new marker
                GameObject newColor = new GameObject("Color_" + color);
                newColor.transform.SetParent(conPanels[j]);
                //Position marker correctly
                RectTransform rect = newColor.AddComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(1, 0);
                rect.pivot = new Vector2(0.5f, 0);
                rect.sizeDelta = new Vector2(0, panelHeight / connection[j].Count - (connection[j].Count-1)*10);
                rect.localScale = new Vector3(1, 1, 1);
                rect.anchoredPosition = new Vector2(0, conIndex * panelHeight / connection[j].Count);
                conIndex++;

                //Set correct color
                UnityEngine.UI.Image image = newColor.AddComponent<UnityEngine.UI.Image>();
                image.color = colors[color];
            }
        }

        //Data text
        visualsT.Find("Name").GetComponent<UnityEngine.UI.Text>().text = data.name;
        visualsT.Find("CardInfo").GetComponent<UnityEngine.UI.Text>().text = data.info;
        visualsT.Find("AP_Panel").Find("AP").GetComponent<UnityEngine.UI.Text>().text = "AP: " + data.AP.ToString();
        visualsT.Find("HP_Panel").Find("HP").GetComponent<UnityEngine.UI.Text>().text = "HP: " + data.HP.ToString();
        visualsT.Find("TP_Panel").Find("TP").GetComponent<UnityEngine.UI.Text>().text = "TP: " + data.TP.ToString();
        visualsT.Find("MV_Panel").Find("MV").GetComponent<UnityEngine.UI.Text>().text = "MV: " + data.MV.ToString();

        //Card art
        Sprite cardArt = (Sprite)(Resources.Load("Cards/" + data.ID, typeof(Sprite)));
        if (cardArt == null)
            cardArt = (Sprite)(Resources.Load("Cards/Card", typeof(Sprite)));
        visualsT.Find("CardArt").GetComponent<UnityEngine.UI.Image>().sprite = cardArt;

        //Cost Markers
        Transform costPanel = visualsT.Find("Cost_Panel");
        UnityEngine.UI.Image[] costMarkers = new UnityEngine.UI.Image[7]{
            costPanel.Find("CM1").GetComponent<UnityEngine.UI.Image>(),
            costPanel.Find("CM2").GetComponent<UnityEngine.UI.Image>(),
            costPanel.Find("CM3").GetComponent<UnityEngine.UI.Image>(),
            costPanel.Find("CM4").GetComponent<UnityEngine.UI.Image>(),
            costPanel.Find("CM5").GetComponent<UnityEngine.UI.Image>(),
            costPanel.Find("CM6").GetComponent<UnityEngine.UI.Image>(),
            costPanel.Find("CM7").GetComponent<UnityEngine.UI.Image>()
        };
        for(int i = 0; i < data.Cost; i++)
            costMarkers[i].color = new Color(1, 1, 0);
        for (int i = data.Cost; i < 7; i++)
            costMarkers[i].color = new Color(0.2f, 0.2f, 0.2f);
    }
}
