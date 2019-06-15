using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField]
    private GameManager m_gameManager;
    [SerializeField]
    private AudioManager m_audioManager;

    [SerializeField]
    private Text m_winLoseText;
    [SerializeField]
    private Text m_levelNameText;
    [SerializeField]
    private Text m_accuracyText;
    [SerializeField]
    private Button m_quitButton;

    [SerializeField]
    private Image m_aGradeImage;
    [SerializeField]
    private Image m_bGradeImage;
    [SerializeField]
    private Image m_cGradeImage;
    [SerializeField]
    private Image m_fGradeImage;

    const string WIN_MESSAGE = "You Win!";
    const string LOSE_MESSAGE = "You Lose";


    void Start()
    {
        m_quitButton.onClick.AddListener(QuitResultsScreen);
    }

    public void Init(bool win, string levelName, float accuracy)
    {
        if(win)
        {
            m_winLoseText.text = WIN_MESSAGE;
        }
        else
        {
            m_winLoseText.text = LOSE_MESSAGE;
        }

        m_levelNameText.text = levelName;
        m_accuracyText.text = "Accuacy\n" + accuracy.ToString("0.00") + "%";
        DisableGrades();

        if (accuracy >= GameManager.A_ACCURACY)
        {
            m_aGradeImage.gameObject.SetActive(true);
        }
        else if (accuracy >= GameManager.B_ACCURACY)
        {
            m_bGradeImage.gameObject.SetActive(true);
        }
        else if (accuracy >= GameManager.C_ACCURACY)
        {
            m_cGradeImage.gameObject.SetActive(true);
        }
        else
        {
            m_fGradeImage.gameObject.SetActive(true);
        }
    }

    private void DisableGrades()
    {
        if (m_aGradeImage.IsActive())
        {
            m_aGradeImage.gameObject.SetActive(false);
        }
        if (m_bGradeImage.IsActive())
        {
            m_bGradeImage.gameObject.SetActive(false);
        }
        if (m_cGradeImage.IsActive())
        {
            m_cGradeImage.gameObject.SetActive(false);
        }
        if (m_fGradeImage.IsActive())
        {
            m_fGradeImage.gameObject.SetActive(false);
        }
    }

    private void QuitResultsScreen()
    {
        m_audioManager.PlayStuckSound();
        m_gameManager.ChangeState(GameStates.MainMenu);
    }
}
