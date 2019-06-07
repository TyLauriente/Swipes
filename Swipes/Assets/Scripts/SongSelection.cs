using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSelection : MonoBehaviour
{

    [SerializeField]
    private Text m_pickASongText;
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

    private int m_currentSongIndex = 0;

    private List<string> m_songNames;
    private string m_selectedSongName;
    private bool m_delayOver;
    private const float DELAY_DURATION = 0.05f;

    public bool songSelected;
    public bool quit;

    void Start()
    {
        m_leftButton.onClick.AddListener(Left);
        m_rightButton.onClick.AddListener(Right);
        m_selectButton.onClick.AddListener(Select);
        Init();
    }

    public void Init()
    {
        songSelected = false;
        quit = false;
        m_delayOver = true;
    }

    private void UpdateText()
    {
        if (m_currentSongIndex < m_songNames.Count)
        {
            m_songNameText.text = m_songNames[m_currentSongIndex];
            m_songNumberText.text = "Song " + ((int)(m_currentSongIndex + 1)).ToString();
        }
    }

    private void Left()
    {
        if (m_delayOver)
        {
            if (m_currentSongIndex != 0)
            {
                m_currentSongIndex--;
                UpdateText();
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
                m_currentSongIndex++;
                UpdateText();
            }
            m_delayOver = false;
            Invoke("EnableDelayOver", DELAY_DURATION);
        }
    }
    private void Select()
    {
        if (m_delayOver)
        {
            songSelected = true;
            if (m_currentSongIndex < m_songNames.Count)
            {
                m_selectedSongName = m_songNames[m_currentSongIndex];
            }
            m_delayOver = false;
            Invoke("EnableDelayOver", DELAY_DURATION);
        }
    }
    public void LoadMusicNames(List<string> names)
    {
        names.Sort();
        m_currentSongIndex = 0;
        m_songNames = names;
        if(names.Count < 1)
        {
            quit = true;
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
