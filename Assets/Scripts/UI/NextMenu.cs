using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextMenu : MonoBehaviour
{
    public Text mScoreValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mScoreValue.text = "" + GameManager.Inst.Score;
    }

    public void onNextClick()
    {
        GameManager.Inst.BackToStartMenu();
    }

    public void onReplay()
    {
        GameManager.Inst.Play();
    }
}
