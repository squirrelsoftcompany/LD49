using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float TIMER_MAX = 10.0f * 60.0f;

    public enum GameState
    {
        eMenuStart = 0,
        eMenuNext,
        eIngame,
        eNbState
    }

    private float mScore;
    public float Score
    {
        get { return mScore; }
        set { mScore = value; }
    }

    private int mDifficulty;
    public int Difficulty
    {
        get { return mDifficulty; }
        set { mDifficulty = value; }
    }


    private float mTimer;
    public float Timer
    {
        get { return mTimer; }
        set { mTimer = value; }
    }


    public GameState mGameState { get; set; }


    private static GameManager _inst = null;
    public static GameManager Inst
    {
        get => _inst;
    }

    private MenuSelector menuSelector;

    // Start is called before the first frame update
    void Start()
    {
        if (_inst == null) _inst = this;
        mGameState = GameState.eMenuStart;

        menuSelector = FindObjectOfType<MenuSelector>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mGameState == GameState.eIngame)
        {
            if(mTimer > 0)
            {
                mTimer -= Time.deltaTime;
            }
            else
            {
                // TODO : Time is over... launch the rocket
                BackToNextMenu();
            }
        }
    }

    public void Play()
    {
        mGameState = GameState.eIngame;
        Score = 0;
        mTimer = TIMER_MAX;
        Fulldisplay();
        RocketCraftor.Inst.LaunchChangeRocketAnimation();
        menuSelector.HideTablet();
    }

    public void GameOver()
    {
        RocketCraftor.Inst.LaunchChangeRocketAnimation();
    }

    public void Fulldisplay()
    {
        PovManager.Inst.SwitchToFD();
    }

    public void Validate()
    {
        if (RocketCraftor.Inst.CanBeValidated())
        {
            Score += RocketCraftor.Inst.Score();
        }
        Fulldisplay();
        RocketCraftor.Inst.LaunchChangeRocketAnimation();
    }

    public void BackToNextMenu()
    {
        mGameState = GameState.eMenuNext;
        menuSelector.ShowNextMenu();
        menuSelector.ShowTablet();
        PovManager.Inst.SwitchToMenu();
    }

    public void BackToStartMenu()
    {
        GameState previousGameState = mGameState;
        
        mGameState = GameState.eMenuStart;
        menuSelector.ShowStartMenu();
        if (previousGameState == GameState.eMenuNext)
        {
            menuSelector.ShowTablet();
            PovManager.Inst.SwitchToMenu();
        }
    }

    public float getTimer()
    {
        return mTimer;
    }
}
