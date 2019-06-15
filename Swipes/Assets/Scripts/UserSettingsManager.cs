using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class UserSettingsManager : MonoBehaviour
{
    UserSettings m_userSettings;

    public UserSettings UserSettings { get => m_userSettings; set => m_userSettings = value; }

    private void Start()
    {
        m_userSettings = new UserSettings();
    }

    public void Init()
    {
        LoadUserSettings();
    }

    private void LoadUserSettings()
    {
        XmlSerializer xs = new XmlSerializer(typeof(UserSettings));

        string path = Application.persistentDataPath;


        path += "/UserSettings.xml";

        if(!File.Exists(path))
        {
            UserSettings = new UserSettings();
            SaveUserSettings();
        }

        using (TextReader textReader = new StreamReader(path))
        {
            UserSettings = (UserSettings)xs.Deserialize(textReader);
        }
    }

    public void SaveUserSettings()
    {
        if (UserSettings != null)
        {
            XmlSerializer xs = new XmlSerializer(typeof(UserSettings));

            string path = Application.persistentDataPath;
           

            path += "/UserSettings.xml";

            using (TextWriter textWriter = new StreamWriter(path))
            {
                xs.Serialize(textWriter, UserSettings);
            }
        }
    }

}
