using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateOrLoad : MonoBehaviour
{
    [SerializeField]
    private LevelManager m_levelManager;


    [SerializeField]
    private Button m_createLevelButton;
    [SerializeField]
    private Button m_loadLevelButton;
    [SerializeField]
    private Button m_quitButton;

    private bool m_createLevel;
    private bool m_loadLevel;
    private bool m_quit;

    public bool CreateLevel { get => m_createLevel; }
    public bool LoadLevel { get => m_loadLevel; }
    public bool Quit { get => m_quit; }

    void Start()
    {
        m_createLevelButton.onClick.AddListener(CreateNewLevel);
        m_loadLevelButton.onClick.AddListener(LoadOldLevel);
        m_quitButton.onClick.AddListener(QuitCreateOrLoad);
    }

    public void Init()
    {
        m_createLevel = false;
        m_loadLevel = false;
        m_quit = false;
    }

    private void CreateNewLevel()
    {
        m_createLevel = true;
    }

    private void LoadOldLevel()
    {
        if(m_levelManager.Levels.Count > 0)
        {
            m_loadLevel = true;
        }
    }

    private void QuitCreateOrLoad()
    {
        m_quit = true;
    }
}
