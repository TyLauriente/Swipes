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
    Gameplay,
    Tutorial,
    ResultScreen
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
    [SerializeField]
    private AudioManager m_audioManager;
    [SerializeField]
    private LevelManager m_levelManager;
    [SerializeField]
    private MainMenuManager m_mainMenuManager;
    [SerializeField]
    private LevelEditorManager m_levelEditorManager;
    [SerializeField]
    private LevelSelectManager m_levelSelectManager;
    [SerializeField]
    private ResultsScreen m_resultsScreenManager;

    [SerializeField]
    private UserSettingsManager m_userSettingsManager;


    [SerializeField]
    private GameObject m_gameplayObject;
    [SerializeField]
    private GameObject m_levelEditorObject;
    [SerializeField]
    private GameObject m_levelSelectObject;
    [SerializeField]
    private GameObject m_optionsObject;
    [SerializeField]
    private GameObject m_mainMenuObject;
    [SerializeField]
    private GameObject m_tutorialObject;
    [SerializeField]
    private GameObject m_resultsScreenObject;

    public const int ALLOWED_LOSSES = 5;
    public const float A_ACCURACY = 95.0f;
    public const float B_ACCURACY = 85.0f;
    public const float C_ACCURACY = 75.0f;
    public const float DEFAULT_EDITOR_SWIPE_TIME_INCREMENT = 0.05f;
    public const float DEFAULT_SONG_VOLUME = 0.4f;
    public const float DEFAULT_EFFECT_VOLUME = 1f;

    private float m_screenWidth;
    private float m_screenHeight;
    private GameStates m_gameState;

    private bool init = false;

    private Level m_nextLevel;

    // Start is called before the first frame update


    private void Start()
    {
        m_screenWidth = (float)Screen.width * 0.5f;
        m_screenHeight = (float)Screen.height * 0.5f;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 100;
        m_levelManager.LoadLevels();
        m_audioManager.LoadUserSongs();
        m_userSettingsManager.Init();
    }

    private void Update()
    {
        if(!init)
        {
            ChangeState(GameStates.MainMenu);
            init = true;
        }
    }

    public void SetNextLevel(Level level)
    {
        m_nextLevel = level;
    }

    public void InitializeResults(bool win, string levelName, float accuracy)
    {
        m_resultsScreenManager.Init(win, levelName, accuracy);
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
        SetAllObjectsUnactive();

        if (state == GameStates.Gameplay)
        {
            m_audioManager.PlaySong(m_nextLevel.musicName);
            m_audioManager.StopSong();
            m_gameplayObject.SetActive(true);
            m_gameplayManager.Init(m_nextLevel);
        }
        else if (state == GameStates.LevelEditor)
        {
            m_levelEditorManager.Init();
            m_levelEditorObject.SetActive(true);
        }
        else if (state == GameStates.LevelSelect)
        {
            m_levelSelectObject.SetActive(true);
            m_levelManager.LoadLevels();
            m_levelSelectManager.Init();
        }
        else if (state == GameStates.Options)
        {
            m_optionsObject.SetActive(true);
        }
        else if(state == GameStates.MainMenu)
        {
            m_mainMenuObject.SetActive(true);
            m_mainMenuManager.Wait();
        }
        else if(state == GameStates.Tutorial)
        {
            m_tutorialObject.SetActive(true);
        }
        else if(state == GameStates.ResultScreen)
        {
            m_resultsScreenObject.SetActive(true);
        }
        m_gameState = state;
    }

    private void SetAllObjectsUnactive()
    {
        m_gameplayObject.SetActive(false);
        m_levelEditorObject.SetActive(false);
        m_levelSelectObject.SetActive(false);
        m_optionsObject.SetActive(false);
        m_mainMenuObject.SetActive(false);
        m_tutorialObject.SetActive(false);
        m_resultsScreenObject.SetActive(false);
    }
}
