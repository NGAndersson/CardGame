using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_MainMenu : MonoBehaviour {
    
    public void MenuButton(string screen)
    {
        MenuManager.Instance.GoToScreen(screen);
    }
}
