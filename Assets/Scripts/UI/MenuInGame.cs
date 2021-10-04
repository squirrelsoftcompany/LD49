using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<Canvas>().enabled = GameManager.Inst.mGameState == GameManager.GameState.eIngame;
    }

    public void onQuit()
    {
        GameManager.Inst.BackToMenu();
    }

    public void onFulldisplay()
    {
        GameManager.Inst.Fulldisplay();
    }

    public void onValidate()
    {
        GameManager.Inst.Validate();
    }
}
