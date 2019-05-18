using System.Collections;
using System.Collections.Generic;
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

    //TEMP----------------------
    public Text timeText;
    public Text accuracyText;
    int numberOfWins = 0;
    //TEMP----------------------

    private Level m_currentLevel;
    private int m_currentSwipe;


    private const float verticalSpeed = 7.0f;
    private const float horizontalSpeed = verticalSpeed * 0.5f;
    private const float allowedTimeDifference = 0.5f;
    private const float requiredSwipeDistance = 0.2f;

    private Vector2 m_startTouchPosition;
    private Vector2 m_touchPos;

    private float m_timeUntilNextSwipe;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_gameManager.GetCurrentState() == GameStates.Gameplay)
        {
            // TEMP ___________________________________________________________________________________________________________
            m_timeUntilNextSwipe = m_currentLevel.GetSwipeTime(m_currentSwipe) - m_audioManager.GetTimePassed();
            timeText.text = m_timeUntilNextSwipe.ToString("0.0");
            accuracyText.text = "Accuracy\n" + ((float)numberOfWins / (float)m_currentSwipe * 100.0f).ToString("0.0") + "%";
            // TEMP ___________________________________________________________________________________________________________

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
        if (m_inputManager.IsFirstRelease() || m_timeUntilNextSwipe < -allowedTimeDifference)
        {
            bool win = false;
            if (m_timeUntilNextSwipe < -allowedTimeDifference || m_timeUntilNextSwipe > allowedTimeDifference)
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
                numberOfWins++;
            }
            else
            {
                m_audioManager.PlayLoseSound();
            }

            m_currentSwipe++;
            m_swipeManager.SetCurrentSwipeType(m_currentLevel.GetSwipe(m_currentSwipe));
            m_swipeManager.SetNextSwipeType(m_currentLevel.GetSwipe(m_currentSwipe + 1));
        }
    }

    public void SetupGameplay()
    {
        m_currentLevel = m_levelManager.GetLevel(0);
        m_currentSwipe = 0;
        m_swipeManager.SetCurrentSwipeType(m_currentLevel.GetSwipe(m_currentSwipe));
        m_swipeManager.SetNextSwipeType(m_currentLevel.GetSwipe(m_currentSwipe + 1));
    }
}
