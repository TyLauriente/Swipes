﻿using System.Collections;
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

    private int m_currentLevel;
    private float m_currentAccuracy;

    private List<Level> m_levels;
    private List<LevelStats> m_levelStats;

    void Start()
    {
        m_leftButton.onClick.AddListener(Left);
        m_rightButton.onClick.AddListener(Right);
        m_selectButton.onClick.AddListener(Select);
        Init();
    }

    private void Update()
    {
        if(m_gameManager.GetCurrentState() == GameStates.LevelSelect)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                m_gameManager.ChangeState(GameStates.MainMenu);
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
        UpdateText();
        if (m_levels == null || m_levels.Count == 0)
        {
            m_gameManager.ChangeState(GameStates.MainMenu);
        }
        m_levelStats = new List<LevelStats>();
        foreach (var level in m_levels)
        {
            string filePath = Application.persistentDataPath + 
                "/LevelStats/LEVEL STAT - " + level.levelName + " - LEVEL STAT.xml";
            
            if(File.Exists(filePath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(LevelStats));
                using (TextReader textReader = new StreamReader(filePath))
                {
                    m_levelStats.Add((LevelStats)xs.Deserialize(textReader));
                }
            }
        }
    }

    private void Left()
    {
        if(m_currentLevel > 0)
        {
            m_currentLevel--;
            UpdateText();
        }
    }
    private void Right()
    {
        if(m_currentLevel + 1 < m_levels.Count)
        {
            m_currentLevel++;
            UpdateText();
        }
    }
    
    private void UpdateText()
    {
        m_levelNumberText.text = "Level " + (m_currentLevel + 1).ToString();

        LevelStats levelStat = new LevelStats();
        bool foundStat = false;
        if (m_levelStats != null)
        {
            foreach (var level in m_levelStats)
            {
                if (level.levelName == m_levels[m_currentLevel].levelName)
                {
                    levelStat = level;
                    foundStat = true;
                    break;
                }
            }
        }

        if(foundStat)
        {
            m_currentAccuracy = levelStat.accuracy;
            m_levelAccuracyText.text = "Accuracy: " + levelStat.accuracy.ToString("0.00") + "%";
        }
        else
        {
            m_currentAccuracy = -1.0f;
            m_levelAccuracyText.text = "Accuracy: N/A";
        }

        if (m_currentLevel < m_levels.Count && m_currentLevel >= 0)
        {
            m_levelNameText.text = m_levels[m_currentLevel].levelName;
            if (m_levels[m_currentLevel].isPrimaryLevel)
            {
                m_primaryLevelText.gameObject.SetActive(true);
            }
            else
            {
                m_primaryLevelText.gameObject.SetActive(false);
            }
        }
    }

    private void Select()
    {
        if(m_currentLevel < m_levels.Count && m_currentLevel >= 0)
        {
            Invoke("StartGame", 0.05f);
        }
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
