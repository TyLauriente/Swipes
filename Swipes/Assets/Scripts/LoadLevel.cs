using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    [SerializeField]
    private LevelManager m_levelManager;
    [SerializeField]
    private AudioManager m_audioManager;

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
    [SerializeField]
    private Button m_deleteButton;
    [SerializeField]
    private GameObject m_deleteImage;
    [SerializeField]
    private GameObject m_areYouSureImage;
    [SerializeField]
    private Button m_quitButton;

    private Level m_oldLevel;
    private int m_currentLevelIndex;

    private bool m_levelSelected;
    private bool m_quit;

    private List<Level> m_userLevels;

    private bool m_pressedDelete;

    public bool LevelSelected { get => m_levelSelected; }
    public bool Quit { get => m_quit; }

    void Start()
    {
        m_leftButton.onClick.AddListener(LeftButtonPress);
        m_rightButton.onClick.AddListener(RightButtonPress);
        m_selectButton.onClick.AddListener(SelectButtonPress);
        m_deleteButton.onClick.AddListener(DeleteButtonPress);
        m_quitButton.onClick.AddListener(QuitLoadLevel);
    }

    public void Init()
    {
        m_levelSelected = false;
        m_pressedDelete = false;
        m_quit = false;
        m_deleteImage.gameObject.SetActive(true);
        m_areYouSureImage.gameObject.SetActive(false);
        m_currentLevelIndex = 0;
        m_levelManager.LoadLevels();
        m_userLevels = m_levelManager.GetUserLevels();
        UpdateText();
    }

    void UpdateText()
    {
        if(m_userLevels.Count != 0)
        {
            m_levelNumberText.text = "Level " + (m_currentLevelIndex + 1).ToString();
            m_levelNameText.text = m_userLevels[m_currentLevelIndex].LevelName;
        }
        else
        {
            m_quit = true;
        }
    }

    private void LeftButtonPress()
    {
        if(m_currentLevelIndex > 0)
        {
            m_audioManager.PlaySuccessfulMenuNavigationSound();
            m_currentLevelIndex--;
            UpdateText();
        }
        else
        {
            m_audioManager.PlayStuckSound();
        }
    }

    private void RightButtonPress()
    {
        if (m_currentLevelIndex < m_userLevels.Count - 1)
        {
            m_audioManager.PlaySuccessfulMenuNavigationSound();
            m_currentLevelIndex++;
            UpdateText();
        }
        else
        {
            m_audioManager.PlayStuckSound();
        }
    }

    private void SelectButtonPress()
    {
        m_audioManager.PlaySuccessfulMenuNavigationSound();
        m_oldLevel = m_userLevels[m_currentLevelIndex];
        m_levelSelected = true;
    }

    private void QuitLoadLevel()
    {
        m_audioManager.PlayStuckSound();
        m_quit = true;
    }

    private void DeleteButtonPress()
    {
        m_audioManager.PlaySuccessfulMenuNavigationSound();
        if (!m_pressedDelete)
        {
            m_pressedDelete = true;
            m_deleteImage.gameObject.SetActive(false);
            m_areYouSureImage.gameObject.SetActive(true);
            Invoke("ResetDeleteButton", 2.5f);
        }
        else
        {
            DeleteLevel();
            m_deleteImage.gameObject.SetActive(true);
            m_areYouSureImage.gameObject.SetActive(false);
            if (m_userLevels.Count == 0)
            {
                m_quit = true;
            }
            Init();
        }
    }

    private void ResetDeleteButton()
    {
        m_pressedDelete = false;
        m_deleteImage.gameObject.SetActive(true);
        m_areYouSureImage.gameObject.SetActive(false);
    }

    private void DeleteLevel()
    {
        Level leveltoDelete = m_userLevels[m_currentLevelIndex];
        m_userLevels.Remove(m_userLevels[m_currentLevelIndex]);

        string levelPath = Application.persistentDataPath + "/Levels/" + leveltoDelete.LevelName + ".xml";
        string levelStatPath = Application.persistentDataPath + "/LevelStats/LEVEL STAT - " + leveltoDelete.LevelName + " - LEVEL STAT.xml";

        if(File.Exists(levelPath))
        {
            File.Delete(levelPath);
        }
        if(File.Exists(levelStatPath))
        {
            File.Delete(levelStatPath);
        }
    }

    public Level GetOldLevel()
    {
        return m_oldLevel;
    }
}
