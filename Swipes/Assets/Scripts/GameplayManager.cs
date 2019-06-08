using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

/*
 * 
 * This Class is the GameplayManager
 * 
 * This is where the main gameplay loop is
 * located and controlled from.
 * 
 * 
 * Authored by Ty Lauriente
 * 05/15/2019
 * 
 * */

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private GameManager m_gameManager;
    [SerializeField]
    private LevelManager m_levelManager;
    [SerializeField]
    private SwipeManager m_swipeManager;
    [SerializeField]
    private InputManager m_inputManager;
    [SerializeField]
    private AudioManager m_audioManager;
    [SerializeField]
    private BackgroundManager m_backgroundManager;

    private const float onePointTime = 0.3f;
    private const float twoPointTime = 0.1f;
    private const float threePointTime = 0.05f;


    [SerializeField]
    private GameObject m_onePointText;
    [SerializeField]
    private GameObject m_twoPointText;
    [SerializeField]
    private GameObject m_threePointText;

    public Text accuracyText;

    const float ANDROID_MUSIC_DELAY = 0.1f;


    private Level m_currentLevel;
    private int m_currentSwipe;

    private int m_points;


    private const float verticalSpeed = 7.0f;
    private const float horizontalSpeed = verticalSpeed * 0.5f;
    private const float requiredSwipeDistance = 0.2f;

    private Vector2 m_startTouchPosition;
    private Vector2 m_touchPos;

    private float m_timeUntilNextSwipe;

    private float m_accuracy;
    private float m_previousAccuracy;


    // Start is called before the first frame update
    void Start()
    {
        m_currentSwipe = 0;
        m_accuracy = 0.0f;
        m_previousAccuracy = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_gameManager.GetCurrentState() == GameStates.Gameplay)
        {
            UpdateTimeUntilNextSwipe();

            if (m_currentSwipe != 0)
            {
                m_accuracy = (m_points / (m_currentSwipe * 3.0f)) * 100.0f;
            }
            else
            {
                m_accuracy = 100.0f;
            }

            // TEMP ___________________________________________________________________________________________________________
            accuracyText.text = "Accuracy\n" + m_accuracy.ToString("0.0") + "%";
            // TEMP ___________________________________________________________________________________________________________

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                m_gameManager.ChangeState(GameStates.MainMenu);
                m_audioManager.StopSong();
                m_previousAccuracy = -1;
            }

            UpdatePositions();

            CheckIsDragging();

            CheckWinOrLose();
        }
    }
    private void UpdatePositions()
    {
        m_touchPos = Input.mousePosition;

        m_touchPos.x = ((m_touchPos.x - m_gameManager.GetScreenWidth()) / m_gameManager.GetScreenWidth()
            * horizontalSpeed);
        m_touchPos.y = ((m_touchPos.y - m_gameManager.GetScreenHeight()) / m_gameManager.GetScreenHeight()
            * verticalSpeed);

        m_swipeManager.UpdateSwipeIndicator();
        m_backgroundManager.SetBackgroundPosition(m_swipeManager.GetCurrentSwipePosition());

        if (m_inputManager.IsFirstTouch())
        {
            m_startTouchPosition = m_touchPos;
        }
    }

    private void CheckIsDragging()
    {
        if (m_inputManager.IsDragging())
        {
            m_touchPos -= m_startTouchPosition;

            if ((m_currentLevel.GetSwipe(m_currentSwipe) == Swipes.Left && m_touchPos.x < 0.0f) || (m_currentLevel.GetSwipe(m_currentSwipe) == Swipes.Right && m_touchPos.x > 0.0f))
            {
                m_swipeManager.SetCurrentSwipeLocation(new Vector2(m_touchPos.x, 0.0f));
            }

            if ((m_currentLevel.GetSwipe(m_currentSwipe) == Swipes.Up && m_touchPos.y > 0.0f) || (m_currentLevel.GetSwipe(m_currentSwipe) == Swipes.Down && m_touchPos.y < 0.0f))
            {
                m_swipeManager.SetCurrentSwipeLocation(new Vector2(0.0f, m_touchPos.y));
            }
        }
    }

    private void CheckWinOrLose()
    {
        if (m_inputManager.IsFirstRelease() || m_timeUntilNextSwipe < -onePointTime)
        {
            bool win = false;
            if (m_timeUntilNextSwipe < -onePointTime || m_timeUntilNextSwipe > onePointTime && m_currentSwipe < m_currentLevel.swipes.Count)
            {
                win = false;
            }
            else if (m_currentLevel.GetSwipe(m_currentSwipe) == Swipes.Up)
            {
                if (m_swipeManager.GetCurrentSwipePosition().y > requiredSwipeDistance)
                {
                    win = true;
                }
            }
            else if (m_currentLevel.GetSwipe(m_currentSwipe) == Swipes.Down)
            {
                if (m_swipeManager.GetCurrentSwipePosition().y < -requiredSwipeDistance)
                {
                    win = true;
                }
            }
            else if (m_currentLevel.GetSwipe(m_currentSwipe) == Swipes.Left)
            {
                if (m_swipeManager.GetCurrentSwipePosition().x < -requiredSwipeDistance)
                {
                    win = true;
                }
            }
            else if (m_currentLevel.GetSwipe(m_currentSwipe) == Swipes.Right)
            {
                if (m_swipeManager.GetCurrentSwipePosition().x > requiredSwipeDistance)
                {
                    win = true;
                }
            }

            if (win)
            {
                m_audioManager.PlayWinSound();
                if(Mathf.Abs(m_timeUntilNextSwipe + ANDROID_MUSIC_DELAY) <= threePointTime)
                {
                    m_points += 3;
                    m_threePointText.gameObject.SetActive(true);
                    Invoke("HideThreePointText", 0.5f);
                }
                else if(Mathf.Abs(m_timeUntilNextSwipe + ANDROID_MUSIC_DELAY) <= twoPointTime)
                {
                    m_points += 2;
                    m_twoPointText.gameObject.SetActive(true);
                    Invoke("HideTwoPointText", 1.0f);
                }
                else
                {
                    m_points += 1;
                    m_onePointText.gameObject.SetActive(true);
                    Invoke("HideOnePointText", 1.5f);
                }
            }
            else
            {
                m_audioManager.PlayLoseSound();
            }

            m_currentSwipe++;
            if (m_currentSwipe >= m_currentLevel.swipes.Count - 1)
            {
                m_accuracy = (m_points / (m_currentSwipe * 3.0f)) * 100.0f;
                if (m_accuracy > m_previousAccuracy)
                {
                    SaveLevelStats();
                }
                m_gameManager.ChangeState(GameStates.MainMenu);
                m_audioManager.StopSong();
                m_previousAccuracy = -1;
                return;
            }
            UpdateTimeUntilNextSwipe();
            m_audioManager.ResetSongTimer();
            m_swipeManager.SetCurrentSwipeType(m_currentLevel.GetSwipe(m_currentSwipe), m_timeUntilNextSwipe);
            m_swipeManager.SetNextSwipeType(m_currentLevel.GetSwipe(m_currentSwipe + 1));
            m_backgroundManager.SetNextBackground(m_currentLevel.GetBackgroundIndex(m_currentSwipe + 1));
        }
    }

    void HideOnePointText()
    {
        m_onePointText.gameObject.SetActive(false);
    }

    void HideTwoPointText()
    {
        m_twoPointText.gameObject.SetActive(false);
    }

    void HideThreePointText()
    {
        m_threePointText.gameObject.SetActive(false);
    }

    void UpdateTimeUntilNextSwipe()
    {
        m_timeUntilNextSwipe = m_currentLevel.GetSwipeTime(m_currentSwipe) - m_audioManager.GetTimePassed() - ANDROID_MUSIC_DELAY;
    }

    public void Init(Level level)
    {
        m_onePointText.gameObject.SetActive(false);
        m_twoPointText.gameObject.SetActive(false);
        m_threePointText.gameObject.SetActive(false);

        m_inputManager.Reset();
        m_startTouchPosition = new Vector2();
        m_touchPos = new Vector2();
        m_points = 0;

        m_currentLevel = level;

        m_currentSwipe = 0;

        UpdateTimeUntilNextSwipe();
        m_swipeManager.SetCurrentSwipeType(m_currentLevel.GetSwipe(m_currentSwipe), m_timeUntilNextSwipe);

        m_backgroundManager.SetNextBackground(m_currentLevel.GetBackgroundIndex(m_currentSwipe));
        m_backgroundManager.SetNextBackground(m_currentLevel.GetBackgroundIndex(m_currentSwipe + 1));
        m_audioManager.ResumeSong();
    }

    public void SetPreviousAccuracy(float accuracy)
    {
        m_previousAccuracy = accuracy;
    }

    private void SaveLevelStats()
    {
        LevelStats levelStat = new LevelStats();
        levelStat.levelName = m_currentLevel.levelName;
        levelStat.accuracy = m_accuracy;
        

        XmlSerializer xs = new XmlSerializer(typeof(LevelStats));

        string path = Application.persistentDataPath + "/LevelStats/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path += "LEVEL STAT - " + levelStat.levelName + " - LEVEL STAT.xml";

        using (TextWriter textWriter = new StreamWriter(path))
        {
            xs.Serialize(textWriter, levelStat);
        }
    }
}
