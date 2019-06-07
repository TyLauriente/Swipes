﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

enum LevelEditorState
{
    SongSelection,
    TapToTheBeat,
    DetailedEditor
}

public class LevelEditorManager : MonoBehaviour
{
    [SerializeField]
    private GameManager m_gameManager;
    [SerializeField]
    private AudioManager m_audioManager;
    [SerializeField]
    private SongSelection m_songSelection;
    [SerializeField]
    private TapToTheBeat m_tapToTheBeat;
    [SerializeField]
    private DetailedEditor m_detailedEditor;

    private LevelEditorState m_currentState;

    private Level m_newLevel;

    private string m_selectedSongName;

    public void Init()
    {
        m_currentState = LevelEditorState.SongSelection;
        m_songSelection.Init();
        m_tapToTheBeat.Init();
        m_songSelection.LoadMusicNames(m_audioManager.GetAllSongNames());
        m_songSelection.gameObject.SetActive(true);
        m_tapToTheBeat.gameObject.SetActive(false);
        m_detailedEditor.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_gameManager.GetCurrentState() == GameStates.LevelEditor)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                m_audioManager.StopSong();
                m_gameManager.ChangeState(GameStates.MainMenu);
            }


            if(m_currentState == LevelEditorState.SongSelection) // Song Selection
            {
                if(m_songSelection.quit)
                {
                    m_gameManager.ChangeState(GameStates.MainMenu);
                }
                else if(m_songSelection.songSelected)
                {
                    m_songSelection.gameObject.SetActive(false);
                    m_selectedSongName = m_songSelection.GetSelectedSongName();
                    m_currentState = LevelEditorState.TapToTheBeat;
                    if(!m_audioManager.PlaySong(m_selectedSongName))
                    {
                        m_gameManager.ChangeState(GameStates.MainMenu);
                        print("Error in LevelEditorManager: Couldn't play selected song");
                    }
                    else
                    {
                        m_tapToTheBeat.gameObject.SetActive(true);
                    }
                }
            }
            else if(m_currentState == LevelEditorState.TapToTheBeat) // Tap To The Beat
            {
                if(m_tapToTheBeat.Skip || m_tapToTheBeat.SongOver)
                {
                    m_audioManager.StopSong();
                    m_detailedEditor.gameObject.SetActive(true);
                    m_newLevel = m_tapToTheBeat.GetNewLevel();
                    m_detailedEditor.Init(m_newLevel);
                    m_tapToTheBeat.gameObject.SetActive(false);
                    m_currentState = LevelEditorState.DetailedEditor;
                }
            }
            else if(m_currentState == LevelEditorState.DetailedEditor) // Detailed Selection
            {
                m_detailedEditor.UpdateDetailedEditor();
                if(m_detailedEditor.Save)
                {
                    SaveLevel(m_detailedEditor.GetNewLevel());
                    m_gameManager.ChangeState(GameStates.MainMenu);
                }
            }
        }
    }

    private void SaveLevel(Level level)
    {
        level.isPrimaryLevel = false;
        level.levelName = "Debugging Name :-)";
        level.musicName = m_selectedSongName;


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
