﻿using System.Collections;
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
    [SerializeField]
    private InputField m_levelNameInputField;
    [SerializeField]
    private Text m_swipeTimeText;
    [SerializeField]
    private Button m_swipeNumberIncrementButton;
    [SerializeField]
    private Button m_swipeNumberDecrementButton;
    [SerializeField]
    private Button m_swipeTimeIncrementButton;
    [SerializeField]
    private Button m_swipeTimeDecrementButton;
    [SerializeField]
    private Toggle m_insertNewSwipeToggle;
    [SerializeField]
    private Button m_quitButton;
    [SerializeField]
    private Image m_quitImage;
    [SerializeField]
    private Image m_areYouSureImage;

    private int m_last = 0, m_secondLast = 1, m_current = 5;

    const float ANDROID_MUSIC_DELAY = 0.1f;


    private int m_currentSwipe;
    private Level m_newLevel;

    private string m_finalTime;

    private float m_timeUntilNextSwipe;

    private bool m_canAffectMusic;

    private bool m_insertNewSwipe;
    private bool m_save;
    private bool m_quit;

    public bool Save { get => m_save; }
    public bool Quit { get => m_quit; }

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
        m_levelNameInputField.onEndEdit.AddListener(LevelNameOnInputEndEdit);
        m_swipeNumberIncrementButton.onClick.AddListener(IncrementSwipe);
        m_swipeNumberDecrementButton.onClick.AddListener(DecrementSwipe);
        m_swipeTimeIncrementButton.onClick.AddListener(IncrementSwipeTime);
        m_swipeTimeDecrementButton.onClick.AddListener(DecrementSwipeTime);
        m_insertNewSwipeToggle.onValueChanged.AddListener(InsertNewSwipeToggle);
        m_quitButton.onClick.AddListener(QuitLevel);
    }

    public void Init(Level level)
    {
        m_save = false;
        m_quit = false;
        m_finalTime = "";
        m_currentSwipe = 0;
        m_newLevel = level;
        m_canAffectMusic = true;
        m_backgroundManager.SetNextBackground(m_newLevel.GetBackgroundIndex(m_currentSwipe));
        m_backgroundManager.SetNextBackground(m_newLevel.GetBackgroundIndex(m_currentSwipe + 1));
        m_swipeManager.SetCurrentSwipeType(m_newLevel.GetSwipe(m_currentSwipe), m_timeUntilNextSwipe);
        m_levelNameInputField.text = m_newLevel.LevelName;
        m_insertNewSwipeToggle.isOn = false;
        m_quitImage.gameObject.SetActive(true);
        m_areYouSureImage.gameObject.SetActive(false);
        m_insertNewSwipe = m_insertNewSwipeToggle.isOn;
    }

    private void Up()
    {
        m_audioManager.PlaySuccessfulMenuNavigationSound();
        SetSwipe(Swipes.Up);
    }

    private void Down()
    {
        m_audioManager.PlaySuccessfulMenuNavigationSound();
        SetSwipe(Swipes.Down);
    }

    private void Left()
    {
        m_audioManager.PlaySuccessfulMenuNavigationSound();
        SetSwipe(Swipes.Left);
    }

    private void Right()
    {
        m_audioManager.PlaySuccessfulMenuNavigationSound();
        SetSwipe(Swipes.Right);
    }

    private void SetSwipe(Swipes swipe)
    {
        if (m_insertNewSwipe)
        {
            m_newLevel.Swipes.Insert(m_currentSwipe, swipe);
            m_newLevel.SwipeTimes.Insert(m_currentSwipe, m_audioManager.GetTimePassed());
            do
            {
                m_current = Random.Range(0, 6);
            } while (m_current == m_last || m_current == m_secondLast);
            m_newLevel.BackgroundIndexes.Insert(m_currentSwipe, m_current);
        }
        else
        {
            m_newLevel.Swipes[m_currentSwipe] = swipe;
        }
        if (m_currentSwipe - 1 > 0)
        {
            m_currentSwipe--;
            m_audioManager.SetSongTime(m_newLevel.SwipeTimes[m_currentSwipe - 1] + 0.01f);
        }
        else
        {
            m_currentSwipe = 0;
            m_audioManager.SetSongTime(0.0f);
        }
        UpdateTimeUntilNextSwipe();
        m_backgroundManager.SetNextBackground(m_newLevel.GetBackgroundIndex(m_currentSwipe));
        m_backgroundManager.SetNextBackground(m_newLevel.GetBackgroundIndex(m_currentSwipe + 1));
        m_swipeManager.SetCurrentSwipeType(m_newLevel.GetSwipe(m_currentSwipe), m_timeUntilNextSwipe);
    }

    private void Remove()
    {
        if (m_currentSwipe < m_newLevel.Swipes.Count)
        {
            m_audioManager.PlaySuccessfulMenuNavigationSound();
            m_newLevel.Swipes.Remove(m_newLevel.Swipes[m_currentSwipe]);
            m_newLevel.SwipeTimes.Remove(m_newLevel.SwipeTimes[m_currentSwipe]);
            m_newLevel.BackgroundIndexes.Remove(m_newLevel.BackgroundIndexes[m_currentSwipe]);
            if(m_currentSwipe > 0)
            {
                m_currentSwipe--;
            }
            UpdateTimeUntilNextSwipe();
            m_backgroundManager.SetNextBackground(m_newLevel.GetBackgroundIndex(m_currentSwipe));
            m_backgroundManager.SetNextBackground(m_newLevel.GetBackgroundIndex(m_currentSwipe + 1));
            m_swipeManager.SetCurrentSwipeType(m_newLevel.GetSwipe(m_currentSwipe), m_timeUntilNextSwipe);
        }
        else
        {
            m_audioManager.PlayStuckSound();
        }
    }

    private void UpdateTimeUntilNextSwipe()
    {
        m_timeUntilNextSwipe = m_newLevel.GetSwipeTime(m_currentSwipe) - m_audioManager.GetTimePassed() - ANDROID_MUSIC_DELAY;
    }

    private void InsertNewSwipeToggle(bool isOn)
    {
        m_audioManager.PlaySuccessfulMenuNavigationSound();
        m_insertNewSwipe = isOn;
    }
    private void PlayPauseToggle()
    {
        m_audioManager.PlaySuccessfulMenuNavigationSound();
        if (m_audioManager.IsSongPlaying())
        {
            m_audioManager.PauseSong();
        }
        else
        {
            m_audioManager.ResumeSong();
            m_audioManager.ResetSongTimer();
            UpdateTimeUntilNextSwipe();
            m_swipeManager.SetCurrentSwipeType(m_newLevel.GetSwipe(m_currentSwipe), m_timeUntilNextSwipe);
            m_swipeManager.SetNextSwipeType(m_newLevel.GetSwipe(m_currentSwipe + 1));
            m_backgroundManager.SetNextBackground(m_newLevel.GetBackgroundIndex(m_currentSwipe + 1));
        }
    }
    private void SaveLevel()
    {
        if (m_newLevel.LevelName.Length > 3)
        {
            m_audioManager.PlaySuccessfulMenuNavigationSound();
            m_save = true;
        }
        else
        {
            m_audioManager.PlayStuckSound();
        }
    }

    private void QuitLevel()
    {
        if (m_quitImage.IsActive())
        {
            m_audioManager.PlaySuccessfulMenuNavigationSound();
            m_quitImage.gameObject.SetActive(false);
            m_areYouSureImage.gameObject.SetActive(true);
            Invoke("ResetQuitImages", 2.5f);
        }
        else
        {
            m_audioManager.PlayStuckSound();
            m_quit = true;
        }
    }

    private void ResetQuitImages()
    {
        m_quitImage.gameObject.SetActive(true);
        m_areYouSureImage.gameObject.SetActive(false);
    }

    private void IncrementSwipe()
    {
        if(m_currentSwipe + 1 < m_newLevel.Swipes.Count)
        {
            m_audioManager.PlaySuccessfulMenuNavigationSound();
            m_currentSwipe++;
            if (m_currentSwipe == 0)
            {
                m_audioManager.SetSongTime(0.0f);
            }
            else
            {
                m_audioManager.SetSongTime(m_newLevel.SwipeTimes[m_currentSwipe - 1] + 0.01f);
            }
            m_audioManager.ResetSongTimer();
            UpdateTimeUntilNextSwipe();
            m_backgroundManager.SetNextBackground(m_newLevel.GetBackgroundIndex(m_currentSwipe));
            m_backgroundManager.SetNextBackground(m_newLevel.GetBackgroundIndex(m_currentSwipe + 1));
            m_swipeManager.SetCurrentSwipeType(m_newLevel.GetSwipe(m_currentSwipe), m_timeUntilNextSwipe);
        }
        else
        {
            m_audioManager.PlayStuckSound();
        }
    }

    private void DecrementSwipe()
    {
        if (m_currentSwipe - 1 >= 0)
        {
            m_audioManager.PlaySuccessfulMenuNavigationSound();
            m_currentSwipe--;
            if (m_currentSwipe == 0)
            {
                m_audioManager.SetSongTime(0.0f);
            }
            else
            {
                m_audioManager.SetSongTime(m_newLevel.SwipeTimes[m_currentSwipe - 1] + 0.01f);
            }
            m_audioManager.ResetSongTimer();
            UpdateTimeUntilNextSwipe();
            m_backgroundManager.SetNextBackground(m_newLevel.GetBackgroundIndex(m_currentSwipe));
            m_backgroundManager.SetNextBackground(m_newLevel.GetBackgroundIndex(m_currentSwipe + 1));
            m_swipeManager.SetCurrentSwipeType(m_newLevel.GetSwipe(m_currentSwipe), m_timeUntilNextSwipe);
        }
        else
        {
            m_audioManager.PlayStuckSound();
        }
    }

    private void IncrementSwipeTime()
    {
        if(m_newLevel.SwipeTimes[m_currentSwipe] + GameManager.DEFAULT_EDITOR_SWIPE_TIME_INCREMENT < m_audioManager.GetTotatlTime())
        {
            m_audioManager.PlaySuccessfulMenuNavigationSound();
            if (m_currentSwipe == 0)
            {
                m_audioManager.SetSongTime(0.0f);
            }
            else
            {
                m_audioManager.SetSongTime(m_newLevel.SwipeTimes[m_currentSwipe - 1] + 0.01f);
            }
            m_newLevel.SwipeTimes[m_currentSwipe] += GameManager.DEFAULT_EDITOR_SWIPE_TIME_INCREMENT;
            m_audioManager.ResetSongTimer();
            UpdateTimeUntilNextSwipe();
            m_swipeManager.SetCurrentSwipeType(m_newLevel.GetSwipe(m_currentSwipe), m_timeUntilNextSwipe);
        }
        else
        {
            m_audioManager.PlayStuckSound();
        }
    }

    private void DecrementSwipeTime()
    {
        if (m_newLevel.SwipeTimes[m_currentSwipe] - GameManager.DEFAULT_EDITOR_SWIPE_TIME_INCREMENT >= 0.0f)
        {
            m_audioManager.PlaySuccessfulMenuNavigationSound();
            if (m_currentSwipe == 0)
            {
                m_audioManager.SetSongTime(0.0f);
            }
            else
            {
                m_audioManager.SetSongTime(m_newLevel.SwipeTimes[m_currentSwipe - 1] + 0.01f);
            }
            m_newLevel.SwipeTimes[m_currentSwipe] -= GameManager.DEFAULT_EDITOR_SWIPE_TIME_INCREMENT;
            m_audioManager.ResetSongTimer();
            UpdateTimeUntilNextSwipe();
            m_swipeManager.SetCurrentSwipeType(m_newLevel.GetSwipe(m_currentSwipe), m_timeUntilNextSwipe);
        }
        else
        {
            m_audioManager.PlayStuckSound();
        }
    }

    private void LevelNameOnInputEndEdit(string levelName)
    {
        m_newLevel.LevelName = levelName;
    }

    private void MusicSlider(float value)
    {
        if (m_canAffectMusic)
        {
            float newTime = m_audioManager.GetTotatlTime() * value;
            m_audioManager.SetSongTime(newTime);
            for (int swipeTimeIndex = 0; swipeTimeIndex < m_newLevel.SwipeTimes.Count; ++swipeTimeIndex)
            {
                if (m_newLevel.SwipeTimes[swipeTimeIndex] >= newTime)
                {
                    m_currentSwipe = swipeTimeIndex;
                    m_audioManager.ResetSongTimer();
                    UpdateTimeUntilNextSwipe();
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
        UpdateTimeUntilNextSwipe(); ;
        m_swipeManager.UpdateSwipeIndicator();

        if (m_currentSwipe < m_newLevel.Swipes.Count)
        {
            m_swipeTimeText.text = "Swipe Time: " + m_newLevel.SwipeTimes[m_currentSwipe].ToString("0.00") + " s";
        }



        m_swipeNumberText.text = "Swipe " + (m_currentSwipe + 1).ToString();
        
        UpdateTime();
        m_canAffectMusic = false;
        m_musicSlider.value = (m_audioManager.GetTimePassed() / m_audioManager.GetTotatlTime());
        m_canAffectMusic = true;
        if (m_timeUntilNextSwipe <= 0.0f && m_currentSwipe < m_newLevel.Swipes.Count)
        {
            m_currentSwipe++;
            m_audioManager.ResetSongTimer();
            UpdateTimeUntilNextSwipe();
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
