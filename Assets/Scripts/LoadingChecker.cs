using UnityEngine;
using System.Collections;

public class LoadingChecker
{
    public UnityEngine.UI.Text loadingText;

    public string Update()
    {
        enum_loadingState state = Database.Instance.loadingState;
        if (state == enum_loadingState.Done)
        {
            MenuManager.Instance.GoToScreen("MainMenu");
        }
        return state.ToString();
    }
}