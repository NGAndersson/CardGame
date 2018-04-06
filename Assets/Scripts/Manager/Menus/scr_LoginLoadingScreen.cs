using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_LoginLoadingScreen : MonoBehaviour {
    public UnityEngine.UI.Text text;
    private string baseText = "Now Loading: ";
    LoadingChecker loadingChecker;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        text.text = baseText + loadingChecker.Update();
	}

    private void OnEnable()
    {
        loadingChecker = new LoadingChecker();
    }
}
