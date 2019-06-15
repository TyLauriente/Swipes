using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSelection : MonoBehaviour
{
    [SerializeField]
    private AudioManager m_audioManager;


    [SerializeField]
    private Text m_songNumberText;
    [SerializeField]
    private Text m_songNameText;
    [SerializeField]
    private Button m_leftButton;
    [SerializeField]
    private Button m_rightButton;
    [SerializeField]
    private Button m_selectButton;
    [SerializeField]
    private Button m_quitButton;

    private int m_currentSongIndex = 0;

    private List<string> m_songNames;
    private string m_selectedSongName;
    private bool m_delayOver;
    private const float DELAY_DURATION = 0.05f;

    private bool m_songSelected;
    private bool m_quit;

    public bool SongSelected { get => m_songSelected; }
    public bool Quit { get => m_quit; }

    void Start()
    {
        m_leftButton.onClick.AddListener(Left);
        m_rightButton.onClick.AddListener(Right);
        m_selectButton.onClick.AddListener(Select);
        m_quitButton.onClick.AddListener(QuitSongSelection);
        Init();
    }

    public void Init()
    {
        m_songSelected = false;
        m_quit = false;
        m_delayOver = true;
    }

    private void UpdateText()
    {
        if (m_currentSongIndex < m_songNames.Count)
        {
            m_songNameText.text = m_songNames[m_currentSongIndex];
            m_songNumberText.text = "Song " + ((int)(m_currentSongIndex + 1)).ToString();
            m_audioManager.PlaySong(m_songNames[m_currentSongIndex]);
        }
    }

    private void Left()
    {
        if (m_delayOver)
        {
            if (m_currentSongIndex != 0)
            {
                m_audioManager.PlaySuccessfulMenuNavigationSound();
                m_currentSongIndex--;
                UpdateText();
            }
            else
            {
                m_audioManager.PlayStuckSound();
            }
            m_delayOver = false;
            Invoke("EnableDelayOver", DELAY_DURATION);
        }
    }
    private void Right()
    {
        if (m_delayOver)
        {
            if (m_currentSongIndex + 1 < m_songNames.Count)
            {
                m_audioManager.PlaySuccessfulMenuNavigationSound();
                m_currentSongIndex++;
                UpdateText();
            }
            else
            {
                m_audioManager.PlayStuckSound();
            }
            m_delayOver = false;
            Invoke("EnableDelayOver", DELAY_DURATION);
        }
        else
        {
            m_audioManager.PlayStuckSound();
        }
    }
    private void Select()
    {
        if (m_delayOver)
        {
            if (m_currentSongIndex < m_songNames.Count)
            {
                m_songSelected = true;
                m_audioManager.PlaySuccessfulMenuNavigationSound();
                m_selectedSongName = m_songNames[m_currentSongIndex];
            }
            else
            {
                m_audioManager.PlayStuckSound();
            }
            m_delayOver = false;
            Invoke("EnableDelayOver", DELAY_DURATION);
        }
    }

    private void QuitSongSelection()
    {
        m_audioManager.PlayStuckSound();
        m_quit = true;
    }

    public void LoadMusicNames(List<string> names)
    {
        names.Sort();
        m_currentSongIndex = 0;
        m_songNames = names;
        if(names.Count < 1)
        {
            m_quit = true;
        }
        else
        {
            UpdateText();
        }
    }

    private void EnableDelayOver()
    {
        m_delayOver = true;
    }

    public string GetSelectedSongName()
    {
        return m_selectedSongName;
    }
}
