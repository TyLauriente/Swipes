using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    [SerializeField]
    private LevelManager m_levelManager;

    [SerializeField]
    private Text m_levelNumberText;
    [SerializeField]
    private Text m_levelNameText;
    [SerializeField]
    private Button m_leftButton;
    [SerializeField]
    private Button m_rightButton;
    [SerializeField]
    private Button m_selectButton;

    private Level m_oldLevel;
    private int m_currentLevelIndex;

    private bool m_levelSelected;

    private List<Level> m_userLevels;

    public bool LevelSelected { get => m_levelSelected; }

    void Start()
    {
        m_leftButton.onClick.AddListener(LeftButtonPress);
        m_rightButton.onClick.AddListener(RightButtonPress);
        m_selectButton.onClick.AddListener(SelectButtonPress);
    }

    public void Init()
    {
        m_levelSelected = false;
        m_currentLevelIndex = 0;
        m_userLevels = m_levelManager.GetUserLevels();
        UpdateText();
    }

    void UpdateText()
    {
        m_levelNumberText.text = "Level " + (m_currentLevelIndex + 1).ToString();
        m_levelNameText.text = m_userLevels[m_currentLevelIndex].levelName;
    }

    private void LeftButtonPress()
    {
        if(m_currentLevelIndex > 0)
        {
            m_currentLevelIndex--;
            UpdateText();
        }
    }

    private void RightButtonPress()
    {
        if (m_currentLevelIndex < m_userLevels.Count - 1)
        {
            m_currentLevelIndex++;
            UpdateText();
        }
    }

    private void SelectButtonPress()
    {
        m_oldLevel = m_userLevels[m_currentLevelIndex];
        m_levelSelected = true;
    }

    public Level GetOldLevel()
    {
        return m_oldLevel;
    }
}
