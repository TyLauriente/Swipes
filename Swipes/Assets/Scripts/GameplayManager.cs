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
    private SwipeManager m_swipeManager;
    [SerializeField]
    private InputManager m_inputManager;
    [SerializeField]
    private AudioManager m_audioManager;
    [SerializeField]
    private BackgroundManager m_backgroundManager;

    private const float ONE_POINT_TIME = 1.0f;
    private const float TWO_POINT_TIME = 0.5f;
    private const float THREE_POINT_TIME = 0.05f;


    [SerializeField]
    private GameObject m_onePointText;
    [SerializeField]
    private GameObject m_twoPointText;
    [SerializeField]
    private GameObject m_threePointText;

    [SerializeField]
    public Text m_accuracyText;

    const float ANDROID_MUSIC_DELAY = 0.1f;


    private Level m_currentLevel;
    private int m_currentSwipe;

    private int m_points;
    private int m_losses;


    private const float VERTICAL_SPEED = 7.0f;
    private const float HORIZONTAL_SPEED = VERTICAL_SPEED * 0.5f;
    private const float REQUIRED_SWIPE_DISTANCE = 0.1f;

    private Vector2 m_startTouchPosition;
    private Vector2 m_touchPos;

    private float m_timeUntilNextSwipe;

    private float m_accuracy;
    private float m_previousAccuracy;

    private bool m_win;


    void Start()
    {
        m_currentSwipe = 0;
        m_accuracy = 0.0f;
        m_previousAccuracy = -1;
        m_win = false;
    }

    public void Init(Level level)
    {
        m_onePointText.gameObject.SetActive(false);
        m_twoPointText.gameObject.SetActive(false);
        m_threePointText.gameObject.SetActive(false);
        m_win = false;
        m_losses = 0;
        m_previousAccuracy = -1;

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

    // Update is called once per frame
    void Update()
    {
        if (m_gameManager.GetCurrentState() == GameStates.Gameplay)
        {
            UpdateTimeUntilNextSwipe();
            UpdateAccuracy();

            if (m_currentSwipe != 0)
            {
                UpdateAccuracy();
            }
            else
            {
                m_accuracy = 100.0f;
            }

            m_accuracyText.text = "Accuracy\n" + m_accuracy.ToString("0.0") + "%";

            UpdatePositions();

            CheckIsDragging();

            CheckWinOrLose();
        }
    }
    private void UpdatePositions()
    {
        m_touchPos = Input.mousePosition;

        m_touchPos.x = ((m_touchPos.x - m_gameManager.GetScreenWidth()) / m_gameManager.GetScreenWidth()
            * HORIZONTAL_SPEED);
        m_touchPos.y = ((m_touchPos.y - m_gameManager.GetScreenHeight()) / m_gameManager.GetScreenHeight()
            * VERTICAL_SPEED);

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
        if (m_inputManager.IsFirstRelease() || m_timeUntilNextSwipe < -ONE_POINT_TIME)
        {
            bool win = false;
            if (m_timeUntilNextSwipe < -ONE_POINT_TIME || m_timeUntilNextSwipe > ONE_POINT_TIME && m_currentSwipe < m_currentLevel.Swipes.Count)
            {
                win = false;
            }
            else if (m_currentLevel.GetSwipe(m_currentSwipe) == Swipes.Up)
            {
                if (m_swipeManager.GetCurrentSwipePosition().y > REQUIRED_SWIPE_DISTANCE)
                {
                    win = true;
                }
            }
            else if (m_currentLevel.GetSwipe(m_currentSwipe) == Swipes.Down)
            {
                if (m_swipeManager.GetCurrentSwipePosition().y < -REQUIRED_SWIPE_DISTANCE)
                {
                    win = true;
                }
            }
            else if (m_currentLevel.GetSwipe(m_currentSwipe) == Swipes.Left)
            {
                if (m_swipeManager.GetCurrentSwipePosition().x < -REQUIRED_SWIPE_DISTANCE)
                {
                    win = true;
                }
            }
            else if (m_currentLevel.GetSwipe(m_currentSwipe) == Swipes.Right)
            {
                if (m_swipeManager.GetCurrentSwipePosition().x > REQUIRED_SWIPE_DISTANCE)
                {
                    win = true;
                }
            }

            bool showResults = false;

            if (win)
            {
                m_audioManager.PlayWinSound();
                float percent = m_swipeManager.GetPercentage();
                if (percent > 0.95f && percent < 1.15f)
                {
                    m_points += 3;
                    m_threePointText.gameObject.SetActive(true);
                    Invoke("HideThreePointText", 0.5f);
                }
                else if (percent > 0.90f && percent < 1.1f)
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
                m_losses++;
                m_audioManager.PlayLoseSound();
            }

            m_currentSwipe++;

            UpdateAccuracy();
            if (m_accuracy < GameManager.C_ACCURACY && m_losses >= GameManager.ALLOWED_LOSSES)
            {
                showResults = true;
            }

            if (m_currentSwipe >= m_currentLevel.Swipes.Count - 1)
            {
                if (m_accuracy > m_previousAccuracy && m_accuracy >= GameManager.C_ACCURACY)
                {
                    SaveLevelStats();
                }
                if (m_accuracy >= GameManager.C_ACCURACY)
                {
                    m_win = true;
                }
                showResults = true;
                m_previousAccuracy = -1;
            }
            if (showResults)
            {
                ShowResults();
                return;
            }
            UpdateTimeUntilNextSwipe();
            m_audioManager.ResetSongTimer();
            m_swipeManager.SetCurrentSwipeType(m_currentLevel.GetSwipe(m_currentSwipe), m_timeUntilNextSwipe);
            m_swipeManager.SetNextSwipeType(m_currentLevel.GetSwipe(m_currentSwipe + 1));
            m_backgroundManager.SetNextBackground(m_currentLevel.GetBackgroundIndex(m_currentSwipe + 1));
        }
    }

    private void ShowResults()
    {
        if(m_win)
        {
            m_audioManager.PlayWinLevelSound();
        }
        else
        {
            m_audioManager.PlayLoseLevelSound();
        }
        m_gameManager.ChangeState(GameStates.ResultScreen);
        if (m_currentLevel != null)
        {
            m_gameManager.InitializeResults(m_win, m_currentLevel.LevelName, m_accuracy);
        }
        m_audioManager.StopSong();
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

    public void SetPreviousAccuracy(float accuracy)
    {
        m_previousAccuracy = accuracy;
    }

    private void UpdateAccuracy()
    {
        m_accuracy = (m_points / (m_currentSwipe * 3.0f)) * 100.0f;
    }

    private void SaveLevelStats()
    {
        LevelStats levelStat = new LevelStats();
        levelStat.LevelName = m_currentLevel.LevelName;
        levelStat.Accuracy = m_accuracy;
        

        XmlSerializer xs = new XmlSerializer(typeof(LevelStats));

        string path = Application.persistentDataPath + "/LevelStats/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path += "LEVEL STAT - " + levelStat.LevelName + " - LEVEL STAT.xml";

        using (TextWriter textWriter = new StreamWriter(path))
        {
            xs.Serialize(textWriter, levelStat);
        }
    }
}
