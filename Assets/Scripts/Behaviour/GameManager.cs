using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

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


    private float mDifficulty;
    public float Difficulty
    {
        get { return mDifficulty; }
        set { mDifficulty = value; }
    }


    private GameState mGameState { get; set; }


    private static GameManager _inst = null;
    public static GameManager Inst
    {
        get => _inst;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_inst == null) _inst = this;
        mGameState = GameState.eMenuStart;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        mGameState = GameState.eIngame;
        //TODO : Animation tablet
        //TODO : Move camera
    }

    public void Fulldisplay()
    {
        PovManager.Inst.SetCurrentRocketPOV(PovManager.RocketPOV.eFullDisplay);
    }

    public void Validate()
    {
        //TODO : Calcul score + call next rocket
    }

    public void BackToMenu()
    {
        mGameState = GameState.eMenuNext;
        //TODO : Animation tablet
        //TODO : Move camera
    }




}
