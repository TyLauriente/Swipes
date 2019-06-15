using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private LevelManager m_levelManager;
    [SerializeField]
    private GameManager m_gameManager;
    [SerializeField]
    private GameplayManager m_gameplayManager;
    [SerializeField]
    private AudioManager m_audioManager;


    [SerializeField]
    private Text m_levelNumberText;
    [SerializeField]
    private Text m_levelNameText;
    [SerializeField]
    private Text m_levelAccuracyText;
    [SerializeField]
    private Text m_primaryLevelText;
    [SerializeField]
    private Button m_leftButton;
    [SerializeField]
    private Button m_rightButton;
    [SerializeField]
    private Button m_selectButton;
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

    private int m_currentLevel;
    private float m_currentAccuracy;

    private List<Level> m_levels;
    private List<LevelStats> m_levelStats;

    void Start()
    {
        m_leftButton.onClick.AddListener(Left);
        m_rightButton.onClick.AddListener(Right);
        m_selectButton.onClick.AddListener(Select);
        m_quitButton.onClick.AddListener(QuitLevelSelect);
    }

    private void Update()
    {
        if(m_gameManager.GetCurrentState() == GameStates.LevelSelect)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                m_audioManager.PlayStuckSound();
                m_gameManager.ChangeState(GameStates.MainMenu);
                m_audioManager.StopSong();
            }
        }
    }

    public void Init()
    {
        m_currentLevel = 0;
        m_levelManager.LoadLevels();
        m_levels = m_levelManager.Levels;
        m_primaryLevelText.gameObject.SetActive(false);
        m_currentAccuracy = -1.0f;
        DisableGrades();
        m_fGradeImage.gameObject.SetActive(true);
        if (m_levels == null || m_levels.Count == 0)
        {
            m_gameManager.ChangeState(GameStates.MainMenu);
        }
        m_levelStats = new List<LevelStats>();
        foreach (var level in m_levels)
        {
            string filePath = Application.persistentDataPath + 
                "/LevelStats/LEVEL STAT - " + level.LevelName + " - LEVEL STAT.xml";
            
            if(File.Exists(filePath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(LevelStats));
                using (TextReader textReader = new StreamReader(filePath))
                {
                    m_levelStats.Add((LevelStats)xs.Deserialize(textReader));
                }
            }
        }
        UpdateText();
    }

    private void Left()
    {
        if(m_currentLevel > 0)
        {
            m_audioManager.PlaySuccessfulMenuNavigationSound();
            m_currentLevel--;
            UpdateText();
        }
        else
        {
            m_audioManager.PlayStuckSound();
        }
    }
    private void Right()
    {
        if(m_currentLevel + 1 < m_levels.Count)
        {
            m_audioManager.PlaySuccessfulMenuNavigationSound();
            m_currentLevel++;
            UpdateText();
        }
        else
        {
            m_audioManager.PlayStuckSound();
        }
    }
    
    private void UpdateText()
    {
        m_levelNumberText.text = "Level " + (m_currentLevel + 1).ToString();

        LevelStats levelStat = new LevelStats();
        bool foundStat = false;
        if (m_currentLevel < m_levels.Count && m_currentLevel >= 0)
        {
            foreach (var level in m_levelStats)
            {
                if (level.LevelName == m_levels[m_currentLevel].LevelName)
                {
                    levelStat = level;
                    foundStat = true;
                    break;
                }
            }
        }
        
        if (foundStat)
        {
            DisableGrades();
            m_currentAccuracy = levelStat.Accuracy;
            if(m_currentAccuracy >= GameManager.A_ACCURACY)
            {
                m_aGradeImage.gameObject.SetActive(true);
            }
            else if(m_currentAccuracy >= GameManager.B_ACCURACY)
            {
                m_bGradeImage.gameObject.SetActive(true);
            }
            else if(m_currentAccuracy >= GameManager.C_ACCURACY)
            {
                m_cGradeImage.gameObject.SetActive(true);
            }
            m_levelAccuracyText.text = "Accuracy\n" + levelStat.Accuracy.ToString("0.00") + "%";
        }
        else
        {
            m_currentAccuracy = -1.0f;
            m_levelAccuracyText.text = "Accuracy: N/A";
        }

        if (m_currentLevel < m_levels.Count && m_currentLevel >= 0)
        {
            m_levelNameText.text = m_levels[m_currentLevel].LevelName;
            m_audioManager.PlaySong(m_levels[m_currentLevel].MusicName);


            if (m_levels[m_currentLevel].IsPrimaryLevel)
            {
                m_primaryLevelText.gameObject.SetActive(true);
            }
            else
            {
                m_primaryLevelText.gameObject.SetActive(false);
            }
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

    private void Select()
    {
        if(m_currentLevel < m_levels.Count && m_currentLevel >= 0)
        {
            m_audioManager.PlaySuccessfulMenuNavigationSound();
            Invoke("StartGame", 0.05f);
        }
    }

    private void QuitLevelSelect()
    {
        m_audioManager.PlayStuckSound();
        m_gameManager.ChangeState(GameStates.MainMenu);
        m_audioManager.StopSong();
    }

    private void StartGame()
    {
        m_gameManager.SetNextLevel(m_levels[m_currentLevel]);
        if(m_currentAccuracy != -1.0f)
        {
            m_gameplayManager.SetPreviousAccuracy(m_currentAccuracy);
        }
        m_gameManager.ChangeState(GameStates.Gameplay);
    }
}
