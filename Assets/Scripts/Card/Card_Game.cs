using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Game : Card {
    
    // Use this for initialization
    public override void Start () {

	}
	
	// Update is called once per frame
	public override void Update () {
		
	}

    public void CardClicked ()
    {
        GameObject.Find("GameManager").GetComponent<scr_GameManager>().ResolveCard(this);
    }
}
