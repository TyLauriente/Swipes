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
    [SerializeField]
    private UserSettingsManager m_userSettingsManager;

    // Normal audio variables

    public List<string> audioNames;

    [SerializeField]
    private List<AudioClip> m_clipList;

    [SerializeField]
    private AudioSource m_currentSong;

    [SerializeField]
    private AudioSource m_winSound;

    [SerializeField]
    private AudioSource m_loseSound;

    [SerializeField]
    private AudioSource m_navigateSound;

    [SerializeField]
    private AudioSource m_stuckSound;

    [SerializeField]
    private AudioSource m_beatLevelSound;

    [SerializeField]
    private AudioSource m_loseLevelSound;


    // Android Native Audio variables
    private int m_winSoundId;
    private int m_winStreamId;

    private int m_loseSoundId;
    private int m_loseStreamId;

    private int m_navigateSoundId;
    private int m_navigateStreamId;

    private int m_stuckSoundId;
    private int m_stuckStreamId;

    private int m_winLevelSoundId;
    private int m_winLevelStreamId;

    private int m_loseLevelSoundId;
    private int m_loseLevelStreamId;

    private bool m_isAndroid = false;

    private float m_songTimer;

    private string m_userSongPath;

    private const float resetTimerWait = 10.0f;
    private float m_resetTimer;

    // Initialize Android Native audio with sound effects
    void Start()
    {
        m_isAndroid = (Application.platform == RuntimePlatform.Android);


        AndroidNativeAudio.makePool(2);
        m_winSoundId = AndroidNativeAudio.load("WinSound.wav");
        m_loseSoundId = AndroidNativeAudio.load("LoseSound.wav");
        m_navigateSoundId = AndroidNativeAudio.load("SuccessfulMenuNavigation.wav");
        m_stuckSoundId = AndroidNativeAudio.load("StuckMenuSound.wav");
        m_winStreamId = -1;
        m_loseStreamId = -1;
        m_navigateStreamId = -1;
        m_stuckStreamId = -1;
        m_winLevelStreamId = -1;
        m_loseLevelStreamId = -1;
        m_songTimer = 0.0f;
        m_resetTimer = 0.0f;
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

    public void ResetSongTimer()
    {
        m_songTimer = m_currentSong.time;
    }

    public bool PlaySong(string songName)
    {
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
            m_currentSong.volume = m_userSettingsManager.UserSettings.SongVolume;
            m_currentSong.Play();
            m_songTimer = 0.0f;
            m_resetTimer = 0.0f;
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
            m_resetTimer = 0.0f;
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
        m_songTimer = m_currentSong.time;
        m_resetTimer = 0.0f;
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
            m_winStreamId = AndroidNativeAudio.play(m_winSoundId, m_userSettingsManager.UserSettings.SoundEffectVolume);
        }
        else
        {
            m_winSound.PlayOneShot(m_winSound.clip, m_userSettingsManager.UserSettings.SoundEffectVolume);
        }
    }

    public void PlayLoseSound()
    {
        if (m_isAndroid)
        {
            m_loseStreamId = AndroidNativeAudio.play(m_loseSoundId, m_userSettingsManager.UserSettings.SoundEffectVolume);
        }
        else
        {
            m_loseSound.PlayOneShot(m_loseSound.clip, m_userSettingsManager.UserSettings.SoundEffectVolume);
        }
    }

    public void PlaySuccessfulMenuNavigationSound()
    {
        if (m_isAndroid)
        {
            m_navigateStreamId = AndroidNativeAudio.play(m_navigateSoundId, m_userSettingsManager.UserSettings.SoundEffectVolume);
        }
        else
        {
            m_navigateSound.PlayOneShot(m_navigateSound.clip, m_userSettingsManager.UserSettings.SoundEffectVolume);
        }
    }
    public void PlayStuckSound()
    {
        if (m_isAndroid)
        {
            m_stuckStreamId = AndroidNativeAudio.play(m_stuckSoundId, m_userSettingsManager.UserSettings.SoundEffectVolume);
        }
        else
        {
            m_stuckSound.PlayOneShot(m_stuckSound.clip, m_userSettingsManager.UserSettings.SoundEffectVolume);
        }
    }
    public void PlayWinLevelSound()
    {
        m_beatLevelSound.PlayOneShot(m_beatLevelSound.clip, m_userSettingsManager.UserSettings.SoundEffectVolume);
    }

    public void PlayLoseLevelSound()
    {
        m_loseLevelSound.PlayOneShot(m_loseLevelSound.clip, m_userSettingsManager.UserSettings.SoundEffectVolume);
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
