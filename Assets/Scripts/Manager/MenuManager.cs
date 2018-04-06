using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager {
    //Singleton
    private static MenuManager instance;
    private MenuManager() { }
    public static MenuManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MenuManager();
                instance.Init();
            }
            return instance;
        }
    }
    public List<GameObject> menus = new List<GameObject>();
    private string prevMenu = null;
    private string currentMenu;

	// Use this for initialization
	public void Init() {
        Transform parent = GameObject.Find("Menus").transform;
        Transform[] trans = GameObject.Find("Menus").GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trans)
        {
            if(t.parent == parent)
                menus.Add(t.gameObject);
        }
    }

    public void GoToScreen(string newMenu)
    {
        foreach (GameObject menu in menus)
        {
            menu.SetActive(false);
        }

        bool found = false;
        foreach(GameObject menu in menus)
        {
            if (menu.name == newMenu)
            {
                menu.SetActive(true);
                prevMenu = currentMenu;
                currentMenu = newMenu;
                found = true;
                break;
            }
        }
        if (!found)
            Debug.LogError("Tried to go to menu \"" + newMenu + "\". But it could not be found!");
    }

    public void GoToPrevious()
    {
        if (prevMenu != null)
            GoToScreen(prevMenu);
        else
            Debug.LogError("Can not go to previous menu before first regular menu-change");
    }
}
