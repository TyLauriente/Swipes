using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * This Class is the GameManager
 * 
 * It keeps track of what state the game is in
 * as well as some global variables
 * 
 * 
 * Authored by Ty Lauriente
 * 05/15/2019
 * 
 * */

public enum GameStates
{
    MainMenu,
    Options,
    LevelSelect,
    LevelEditor,
    Gameplay
}

public enum Swipes
{
    Invalid,
    Up,
    Down,
    Left,
    Right,
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameplayManager m_gameplayManager;

    private float m_screenWidth;
    private float m_screenHeight;
    private GameStates m_gameState;

    // Start is called before the first frame update


    private void Start()
    {
        ChangeState(GameStates.Gameplay);
        m_screenWidth = (float)Screen.width * 0.5f;
        m_screenHeight = (float)Screen.height * 0.5f;
    }

    public float GetScreenWidth()
    {
        return m_screenWidth;
    }

    public float GetScreenHeight()
    {
        return m_screenHeight;
    }

    public GameStates GetCurrentState()
    {
        return m_gameState;
    }

    public void ChangeState(GameStates state)
    {
        m_gameState = state;

        if (state == GameStates.Gameplay)
        {
            m_gameplayManager.SetupGameplay();
        }
    }
}
