using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapToTheBeat : MonoBehaviour
{
    [SerializeField]
    private AudioManager m_audioManager;

    [SerializeField]
    private Text m_swipeNumberText;
    [SerializeField]
    private Button m_addSwipeButton;
    [SerializeField]
    private Button m_skipButton;
    [SerializeField]
    private Button m_quitButton;
    [SerializeField]
    private GameObject m_quitImage;
    [SerializeField]
    private GameObject m_areYouSureImage;

    private Level m_newLevel;

    private int m_last = 0, m_secondLast = 1, m_current = 5;

    private bool m_skip;
    private bool m_songOver;
    private bool m_quit;

    public bool Skip { get => m_skip; }
    public bool SongOver { get => m_songOver; }
    public bool Quit { get => m_quit; }

    void Start()
    {
        m_addSwipeButton.onClick.AddListener(AddSwipe);
        m_skipButton.onClick.AddListener(SkipTapToTheBeat);
        m_quitButton.onClick.AddListener(QuitTapToTheBeat);
        Init();
    }

    public void Init()
    {
        m_songOver = false;
        m_skip = false;
        m_quit = false;
        m_newLevel = new Level();
        m_newLevel.levelName = "";
        m_swipeNumberText.text = "Swipe 0";
        m_quitImage.SetActive(true);
        m_areYouSureImage.SetActive(false);
    }

    public void CheckSongOver()
    {
        if(m_audioManager.GetTimePassed() >= m_audioManager.GetTotatlTime())
        {
            m_songOver = true;
        }
    }

    private void AddSwipe()
    {
        do
        {
            m_current = Random.Range(0, 6);
        } while (m_current == m_last || m_current == m_secondLast);

        m_audioManager.ResetSongTimer();
        m_newLevel.AddSwipe(m_audioManager.GetTimePassed(), (Swipes)Random.Range(1, 5), m_current);
        m_swipeNumberText.text = "Swipe " + m_newLevel.swipes.Count.ToString();
    }

    private void SkipTapToTheBeat()
    {
        m_skip = true;
    }

    private void QuitTapToTheBeat()
    {
        if (m_quitImage.activeSelf)
        {
            m_quitImage.SetActive(false);
            m_areYouSureImage.SetActive(true);
            Invoke("ResetQuitImages", 2.5f);
        }
        else
        {
            m_quit = true;
        }
    }

    private void ResetQuitImages()
    {
        m_quitImage.SetActive(true);
        m_areYouSureImage.SetActive(false);
    }

    public Level GetNewLevel()
    {
        return m_newLevel;
    }

}
