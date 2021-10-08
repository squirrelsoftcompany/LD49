using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInGame : MonoBehaviour
{
    [Header("Buttons")]
    public Button m_validateButton;
    public Button m_invalidateButton;
    [Header("Timer")]
    public Text mTimerText;
    public Slider mSlider;
    private int mSec;
    private int mMin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<Canvas>().enabled = GameManager.Inst.mGameState == GameManager.GameState.eIngame;
        float time = GameManager.Inst.Timer;
        mMin = (int)(time / 60f);
        mSec = (int)(time % 60f);
        mTimerText.text = mMin.ToString("00") + ":" + mSec.ToString("00");
        mSlider.value = (GameManager.TIMER_MAX - time) / GameManager.TIMER_MAX;
        
        // (Dis)activate validate button depending on rocket state
        m_validateButton.gameObject.SetActive(RocketCraftor.Inst.CanBeValidated());
        m_invalidateButton.gameObject.SetActive(!RocketCraftor.Inst.CanBeValidated());
    }

    public void onQuit()
    {
        GameManager.Inst.BackToNextMenu();
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
