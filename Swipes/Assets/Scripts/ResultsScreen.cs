using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField]
    private GameManager m_gameManager;

    [SerializeField]
    private Text m_winLoseText;
    [SerializeField]
    private Text m_levelNameText;
    [SerializeField]
    private Text m_accuracyText;
    [SerializeField]
    private Button m_quitButton;

    [SerializeField]
    private GameObject m_aGrade;
    [SerializeField]
    private GameObject m_bGrade;
    [SerializeField]
    private GameObject m_cGrade;
    [SerializeField]
    private GameObject m_fGrade;

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
            m_aGrade.SetActive(true);
        }
        else if (accuracy >= GameManager.B_ACCURACY)
        {
            m_bGrade.SetActive(true);
        }
        else if (accuracy >= GameManager.C_ACCURACY)
        {
            m_cGrade.SetActive(true);
        }
        else
        {
            m_fGrade.SetActive(true);
        }
    }

    private void DisableGrades()
    {
        m_aGrade.SetActive(false);
        m_bGrade.SetActive(false);
        m_cGrade.SetActive(false);
        m_fGrade.SetActive(false);
    }

    private void QuitResultsScreen()
    {
        m_gameManager.ChangeState(GameStates.MainMenu);
    }
}
