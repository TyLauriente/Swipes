using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class TempLevelCreator : MonoBehaviour
{
    [SerializeField]
    AudioSource song;

    Level level;

    private int last = 0, secondLast = 1, current = 5;

    bool savedLevel = false;
    float timer = 0.0f;

    private const int LEVEL_NUMBER = 1;

    // Start is called before the first frame update
    void Start()
    {
        level = new Level();
        level.levelName = "Level2";
        level.musicName = "East - Lydian Collective";

        song.Play();
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.J))
        {
            last = secondLast;
            secondLast = current;
            do
            {
                current = (int)Random.Range(0, 6);
            } while (current == last || current == secondLast);
            level.AddSwipe(timer, (Swipes)Random.Range(1, 5), current);
        }

        if(!song.isPlaying ||Input.GetKeyDown(KeyCode.Space))
        {
            SaveLevel();
        }
    }

    private void SaveLevel()
    {
        XmlSerializer xs = new XmlSerializer(typeof(Level));
        
        string path = Application.persistentDataPath + "/Levels/";
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        
        TextWriter textWriter = new StreamWriter(path + "Level5.xml");
        
        xs.Serialize(textWriter, level);
    }
}
