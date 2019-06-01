﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class TempLevelCreator : MonoBehaviour
{
    [SerializeField]
    LevelManager m_levelManager;

    Level level;

    // Start is called before the first frame update
    void Start()
    {
        m_levelManager.LoadLevels();
        level = m_levelManager.GetLevel(0);
        for (int index = 0; index < level.swipes.Count; ++index)
        {
            level.swipeTimes[index] += 0.18f;
        }
        SaveLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SaveLevel()
    {
        XmlSerializer xs = new XmlSerializer(typeof(Level));
        
        string path = Application.persistentDataPath + "/Levels/";
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        
        TextWriter textWriter = new StreamWriter(path + "LevelNew.xml");
        
        xs.Serialize(textWriter, level);
    }
}
