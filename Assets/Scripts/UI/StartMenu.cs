using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    private int mDifficultySelected;

    // Start is called before the first frame update
    void Start()
    {
        onLevel1Toggled(true);
    }

    public void onPlayClick()
    {
        //Do something maybe...
        GameManager.Inst.Difficulty = mDifficultySelected;
        GameManager.Inst.Play();
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
