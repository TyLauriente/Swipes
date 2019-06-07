using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailedEditor : MonoBehaviour
{
    [SerializeField]
    private AudioManager m_audioManager;

    [SerializeField]
    private BackgroundManager m_backgroundManager;
    [SerializeField]
    private SwipeManager m_swipeManager;

    [SerializeField]
    private Text m_swipeNumberText;
    [SerializeField]
    private Text m_playPauseText;
    [SerializeField]
    private Slider m_musicSlider;
    [SerializeField]
    private Text m_musicTimeText;
    [SerializeField]
    private Button m_playPauseButton;
    [SerializeField]
    private Button m_upSwipeButton;
    [SerializeField]
    private Button m_downSwipeButton;
    [SerializeField]
    private Button m_leftSwipeButton;
    [SerializeField]
    private Button m_rightSwipeButton;
    [SerializeField]
    private Button m_removeSwipeButton;
    [SerializeField]
    private Button m_saveButton;

    private int m_currentSwipe;
    private Level m_newLevel;

    private string m_finalTime;

    private float m_timeUntilNextSwipe;

    private bool m_canAffectMusic;

    private bool m_save;

    public bool Save { get => m_save; }

    void Start()
    {
        m_upSwipeButton.onClick.AddListener(Up);
        m_downSwipeButton.onClick.AddListener(Down);
        m_leftSwipeButton.onClick.AddListener(Left);
        m_rightSwipeButton.onClick.AddListener(Right);
        m_removeSwipeButton.onClick.AddListener(Remove);
        m_playPauseButton.onClick.AddListener(PlayPauseToggle);
        m_saveButton.onClick.AddListener(SaveLevel);
        m_musicSlider.onValueChanged.AddListener(MusicSlider);

    }

    public void Init(Level level)
    {
        m_finalTime = "";
        m_currentSwipe = 0;
        m_newLevel = level;
        m_canAffectMusic = true;
        m_backgroundManager.SetNextBackground(m_newLevel.GetBackgroundIndex(m_currentSwipe));
        m_backgroundManager.SetNextBackground(m_newLevel.GetBackgroundIndex(m_currentSwipe + 1));
        m_swipeManager.SetCurrentSwipeType(m_newLevel.GetSwipe(m_currentSwipe), m_timeUntilNextSwipe);
    }

    private void Up()
    {

    }

    private void Down()
    {

    }

    private void Left()
    {

    }

    private void Right()
    {

    }

    private void Remove()
    {

    }
    private void PlayPauseToggle()
    {
        if(m_audioManager.IsSongPlaying())
        {
            m_audioManager.PauseSong();
        }
        else
        {
            m_audioManager.ResumeSong();
        }
    }
    private void SaveLevel()
    {
        m_save = true;
    }

    private void MusicSlider(float value)
    {
        if (m_canAffectMusic)
        {
            float newTime = m_audioManager.GetTotatlTime() * value;
            m_audioManager.SetSongTime(newTime);
            for (int swipeTimeIndex = 0; swipeTimeIndex < m_newLevel.swipeTimes.Count; ++swipeTimeIndex)
            {
                if (m_newLevel.swipeTimes[swipeTimeIndex] >= newTime)
                {
                    m_currentSwipe = swipeTimeIndex;
                    m_timeUntilNextSwipe = m_newLevel.GetSwipeTime(m_currentSwipe) - m_audioManager.GetTimePassed();
                    m_backgroundManager.SetNextBackground(m_newLevel.GetBackgroundIndex(m_currentSwipe));
                    m_backgroundManager.SetNextBackground(m_newLevel.GetBackgroundIndex(m_currentSwipe + 1));
                    m_swipeManager.SetCurrentSwipeType(m_newLevel.GetSwipe(m_currentSwipe), m_timeUntilNextSwipe);
                    break;
                }
            }
        }
    }

    public void UpdateDetailedEditor()
    {
        m_timeUntilNextSwipe = m_newLevel.GetSwipeTime(m_currentSwipe) - m_audioManager.GetTimePassed();
        m_swipeManager.UpdateSwipeIndicator();
        

        m_swipeNumberText.text = "Swipe " + (m_currentSwipe + 1).ToString();
        if (m_audioManager.IsSongPlaying())
        {
            m_playPauseText.text = "Status: Playing";
        }
        else
        {
            m_playPauseText.text = "Status: Paused";
        }
        UpdateTime();
        m_canAffectMusic = false;
        m_musicSlider.value = (m_audioManager.GetTimePassed() / m_audioManager.GetTotatlTime());
        m_canAffectMusic = true;
        if (m_timeUntilNextSwipe <= 0.0f && m_currentSwipe < m_newLevel.swipes.Count)
        {
            m_currentSwipe++;
            m_timeUntilNextSwipe = m_newLevel.GetSwipeTime(m_currentSwipe) - m_audioManager.GetTimePassed();
            m_swipeManager.SetCurrentSwipeType(m_newLevel.GetSwipe(m_currentSwipe), m_timeUntilNextSwipe);
            m_swipeManager.SetNextSwipeType(m_newLevel.GetSwipe(m_currentSwipe + 1));
            m_backgroundManager.SetNextBackground(m_newLevel.GetBackgroundIndex(m_currentSwipe + 1));
            m_audioManager.PlayWinSound();
        }
    }

    private void UpdateTime()
    {
        if (m_finalTime.Length == 0)
        {
            int finalMinutes = (int)m_audioManager.GetTotatlTime() / 60;
            int finalSeconds = (int)(m_audioManager.GetTotatlTime() - (finalMinutes * 60));
            if(finalSeconds > 9)
            {
                m_finalTime = "0" + finalMinutes.ToString() + ":" + finalSeconds.ToString();
            }
            else
            {
                m_finalTime = "0" + finalMinutes.ToString() + ":0" + finalSeconds.ToString();
            }
        }
        int minutes = (int)m_audioManager.GetTimePassed() / 60;
        int seconds = (int)(m_audioManager.GetTimePassed() - (minutes * 60));
        if (seconds > 9)
        {
            m_musicTimeText.text = "0" + minutes.ToString() + ":" + seconds.ToString() + " / " + m_finalTime;
        }
        else
        {
            m_musicTimeText.text = "0" + minutes.ToString() + ":0" + seconds.ToString() + " / " + m_finalTime; ;
        }
    }

    public Level GetNewLevel()
    {
        return m_newLevel;
    }
}
