using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

/*
 * 
 * This Class is the AudioManager
 * 
 * It keeps track of all audio and has 
 * a function to access time passed from music
 * 
 * 
 * Authored by Ty Lauriente
 * 05/15/2019
 * 
 * */

public class AudioManager : MonoBehaviour
{
    // Normal audio variables

    public List<string> audioNames;

    [SerializeField]
    private List<AudioClip> m_clipList;

    [SerializeField]
    private AudioSource m_winSound;

    [SerializeField]
    private AudioSource m_loseSound;

    [SerializeField]
    private AudioSource m_currentSong;

    // Android Native Audio variables
    private int m_winSoundId;
    private int m_winStreamId;

    private int m_loseSoundId;
    private int m_loseStreamId;

    private bool m_isAndroid = false;

    private float m_songTimer;

    private string m_userSongPath;

    const float editorTimerOffset = 0.18f;

    // Initialize Android Native audio with sound effects
    void Start()
    {
        m_isAndroid = (bool)(Application.platform == RuntimePlatform.Android);


        AndroidNativeAudio.makePool(2);
        m_winSoundId = AndroidNativeAudio.load("WinSound.wav");
        m_loseSoundId = AndroidNativeAudio.load("LoseSound.wav");
        m_winStreamId = -1;
        m_loseStreamId = -1;
        m_songTimer = 0.0f;
    }

    public void LoadUserSongs()
    {
        // Load user songs
        m_userSongPath = Application.persistentDataPath + "/Music";

        if (!Directory.Exists(m_userSongPath))
        {
            Directory.CreateDirectory(m_userSongPath);
        }
        else
        {
            string[] songPaths = Directory.GetFiles(m_userSongPath);
            XmlSerializer xs = new XmlSerializer(typeof(AudioClip));
            foreach (var songPath in songPaths)
            {
                using (WWW audioFile = new WWW("file://" + songPath))
                {
                    AudioClip clip = audioFile.GetAudioClip();
                    clip.name = Path.GetFileNameWithoutExtension(songPath);
                    bool isLoaded = false;
                    foreach (var c in m_clipList)
                    {
                        if(c.name == clip.name)
                        {
                            isLoaded = true;
                        }
                    }

                    if (!isLoaded)
                    {
                        m_clipList.Add(clip);
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (m_currentSong.isPlaying)
        {
            m_songTimer += Time.deltaTime;
        }
    }

    public bool PlaySong(string songName)
    {
        //m_currentSong.time = m_currentSong.clip.length * percentage;
        if (m_currentSong.isPlaying)
        {
            m_currentSong.Stop();
        }

        AudioClip foundClip = null;
        foreach (var clip in m_clipList)
        {
            if(clip.name == songName)
            {
                foundClip = clip;
                break;
            }
        }
        if (foundClip != null)
        {
            m_currentSong.clip = foundClip;
            m_currentSong.Play();
            if (m_isAndroid)
            {
                m_songTimer = 0.0f;
            }
            else
            {
                m_songTimer = editorTimerOffset;
            }
            return true;
        }
        return false;
    }

    public bool IsSongPlaying()
    {
        return m_currentSong.isPlaying;
    }

    public void SetSongTime(float time)
    {
        if(time < m_currentSong.clip.length)
        {
            m_currentSong.time = time;
            m_songTimer = time;
        }
    }

    public void PauseSong()
    {
        if (m_currentSong.isPlaying)
        {
            m_currentSong.Pause();
        }
    }
    public void ResumeSong()
    {
        m_currentSong.Play();
    }

    public void StopSong()
    {
        if (m_currentSong.isPlaying)
        {
            m_songTimer = 0.0f;
            m_currentSong.Pause();
            m_currentSong.time = 0.0f;
        }
    }


    public void PlayWinSound()
    {
        if (m_isAndroid)
        {
            m_winStreamId = AndroidNativeAudio.play(m_winSoundId);
        }
        else
        {
            m_winSound.PlayOneShot(m_winSound.clip);
        }
    }

    public void PlayLoseSound()
    {
        if (m_isAndroid)
        {
            m_loseStreamId = AndroidNativeAudio.play(m_loseSoundId);
        }
        else
        {
            m_loseSound.PlayOneShot(m_loseSound.clip);
        }
    }

    public float GetTimePassed()
    {
        return m_songTimer;
    }

    public float GetTotatlTime()
    {
        return m_currentSong.clip.length;
    }

    public List<string> GetAllSongNames()
    {
        List<string> songNames = new List<string>();

        LoadUserSongs();

        foreach (var clip in m_clipList)
        {
            songNames.Add(clip.name);
        }
        return songNames;
    }
}
