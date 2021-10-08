using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{

    private int mDifficultySelected;

    public Animator animatorTablet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onPlayClick()
    {
        //Do something maybe...
        animatorTablet.SetTrigger("HideTablet");
        GameManager.Inst.Difficulty = mDifficultySelected;
        GameManager.Inst.Play();
    }

    public void ShowMenu()
    {
        animatorTablet.SetTrigger("ShowTablet");
    }

    public void onLevel1Toggled(bool pToggle)
    {
        if (! pToggle) return;
        mDifficultySelected = 1;
        //Do something maybe...
    }

    public void onLevel2Toggled(bool pToggle)
    {
        if (! pToggle) return;
        mDifficultySelected = 2;
        //Do something maybe...
    }

    public void onLevel3Toggled(bool pToggle)
    {
        if (! pToggle) return;
        mDifficultySelected = 3;
        //Do something maybe...
    }

    public void onQuitClick()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif // UNITY_EDITOR
    }
}
