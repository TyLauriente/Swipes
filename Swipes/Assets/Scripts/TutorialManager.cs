using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private GameManager m_gameManager;
    [SerializeField]
    private AudioManager m_audioManager;

    [SerializeField]
    private Button m_quitButton;
    [SerializeField]
    private Button m_leftButton;
    [SerializeField]
    private Button m_rightButton;
    [SerializeField]
    private Text m_pageNumberText;


    [SerializeField]
    private SpriteRenderer m_image1;
    [SerializeField]
    private SpriteRenderer m_image2;
    [SerializeField]
    private SpriteRenderer m_image3;
    [SerializeField]
    private SpriteRenderer m_image4;
    [SerializeField]
    private SpriteRenderer m_image5;

    private const int TOTAL_PAGES = 5;
    private int m_currentPage;


    void Start()
    {
        m_quitButton.onClick.AddListener(QuitTutorial);
        m_leftButton.onClick.AddListener(LeftButtonPress);
        m_rightButton.onClick.AddListener(RightButtonPress);
        Init();
    }

    public void Init()
    {
        m_currentPage = 0;
        UpdateTutorialImages();
    }

    private void QuitTutorial()
    {
        m_audioManager.PlayStuckSound();
        m_gameManager.ChangeState(GameStates.MainMenu);
    }

    private void LeftButtonPress()
    {
        if(m_currentPage > 0)
        {
            m_audioManager.PlaySuccessfulMenuNavigationSound();
            m_currentPage--;
            UpdateTutorialImages();
        }
        else
        {
            m_audioManager.PlayStuckSound();
        }
    }

    private void RightButtonPress()
    {
        if (m_currentPage < TOTAL_PAGES - 1)
        {
            m_audioManager.PlaySuccessfulMenuNavigationSound();
            m_currentPage++;
            UpdateTutorialImages();
        }
        else
        {
            m_audioManager.PlayStuckSound();
        }
    }

    private void UpdateTutorialImages()
    {
        m_image1.gameObject.SetActive(false);
        m_image2.gameObject.SetActive(false);
        m_image3.gameObject.SetActive(false);
        m_image4.gameObject.SetActive(false);
        m_image5.gameObject.SetActive(false);

        if(m_currentPage == 0)
        {
            m_image1.gameObject.SetActive(true);
        }
        else if (m_currentPage == 1)
        {
            m_image2.gameObject.SetActive(true);
        }
        else if (m_currentPage == 2)
        {
            m_image3.gameObject.SetActive(true);
        }
        else if (m_currentPage == 3)
        {
            m_image4.gameObject.SetActive(true);
        }
        else if (m_currentPage == 4)
        {
            m_image5.gameObject.SetActive(true);
        }

        m_pageNumberText.text = (m_currentPage + 1).ToString() + " / " + TOTAL_PAGES;
    }


}
