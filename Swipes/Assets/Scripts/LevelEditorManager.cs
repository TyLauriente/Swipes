using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorManager : MonoBehaviour
{
    [SerializeField]
    private GameManager m_gameManager;
    [SerializeField]
    private AudioManager m_audioManager;
    [SerializeField]
    private Button m_btn;

    private bool hasStarted = false;

    private int last = 0, secondLast = 1, current = 5;
    Level level;
    bool isOver = false;
    bool waitOver = false;

    // Start is called before the first frame update
    void Start()
    {
        level = new Level();

        level.levelName = "TestLevel";
        level.musicName = "Thirty One - Lydian Collective";
        level.isPrimaryLevel = false;
        m_btn.onClick.AddListener(End);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_gameManager.GetCurrentState() == GameStates.LevelEditor && waitOver)
        {
            if (m_audioManager.GetTimePassed() >= m_audioManager.GetTotatlTime() || isOver)
            {
                SaveLevel();
            }
            if (Input.anyKeyDown)
            {
                if (!hasStarted)
                {
                    m_audioManager.PlaySong("Thirty One - Lydian Collective");
                    hasStarted = true;
                }
                else
                {
                    do
                    {
                        current = (int)Random.Range(0, 6);
                    } while (current == last || current == secondLast);
                    level.AddSwipe(m_audioManager.GetTimePassed(), (Swipes)Random.Range(1, 5), current);
                }
            }
        }
    }

    public void Wait()
    {
        waitOver = false;
        isOver = false;
        hasStarted = false;
        level = new Level();
        level.levelName = "TestLevel";
        level.musicName = "Thirty One - Lydian Collective";
        level.isPrimaryLevel = false;
        Invoke("WaitOver", 0.15f);
    }

    private void WaitOver()
    {
        waitOver = true;
    }

    void End()
    {
        isOver = true;
    }

    private void SaveLevel()
    {
        XmlSerializer xs = new XmlSerializer(typeof(Level));

        string path = Application.persistentDataPath + "/Levels/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        int levelIndex = 1;
        bool found = false;
        do
        {
            if(!File.Exists(path + "UserLevel" + levelIndex.ToString() + ".xml"))
            {
                path += "UserLevel" + levelIndex.ToString() + ".xml";
                found = true;
            }
            levelIndex++;
        } while (!found);

        using (TextWriter textWriter = new StreamWriter(path))
        {
            xs.Serialize(textWriter, level);
        }

        m_audioManager.StopSong();
        m_gameManager.ChangeState(GameStates.MainMenu);
    }
}
