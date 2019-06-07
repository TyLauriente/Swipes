using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

/*
 * 
 * This Class is the LevelManager
 * 
 * It controls the different levels the game has
 * and allows access to these levels.
 * 
 * 
 * Authored by Ty Lauriente
 * 05/15/2019
 * 
 * */

public class LevelManager : MonoBehaviour
{
    const int MAX_PRIMARY_LEVELS = 10;


    private List<Level> m_levels;

    string m_primaryLevelPath;
    string m_userLevelPath;

    public List<Level> Levels { get => m_levels; }

    public void LoadLevels()
    {
        m_levels = new List<Level>();
        m_primaryLevelPath = "Levels/";
        m_userLevelPath = Application.persistentDataPath + "/Levels/";

        // Load all primary levels

        var xs = new XmlSerializer(typeof(Level));
        for(int levelIndex = 0; levelIndex < MAX_PRIMARY_LEVELS; ++levelIndex)
        {
            TextAsset levelAsText = (TextAsset)Resources.Load(m_primaryLevelPath + "Level" + levelIndex.ToString(), typeof(TextAsset));
            if(levelAsText != null)
            {
                using (TextReader reader = new StringReader(levelAsText.text))
                {
                    Levels.Add((Level)xs.Deserialize(reader));
                }
            }
        }

        // Load all user made levels

        string[] levelPaths = Directory.GetFiles(m_userLevelPath, "*xml");

        foreach (var levelPath in levelPaths)
        {
            using (TextReader reader = new StreamReader(levelPath))
            {
                Level tempLevel = (Level)xs.Deserialize(reader);
                tempLevel.isPrimaryLevel = false;
                Levels.Add(tempLevel);
            }
        }
    }

    public List<Level> GetUserLevels()
    {
        List<Level> userLevels = new List<Level>();
        foreach (var level in Levels)
        {
            if (!level.isPrimaryLevel)
            {
                userLevels.Add(level);
            }
        }
        return userLevels;
    }

    public Level GetUserLevel()
    {
        foreach (var level in Levels)
        {
            if(!level.isPrimaryLevel)
            {
                return level;
            }
        }
        return new Level();
    }

    public Level GetPrimaryLevel()
    {
        foreach (var level in Levels)
        {
            if (level.isPrimaryLevel)
            {
                return level;
            }
        }
        return new Level();
    }


}
