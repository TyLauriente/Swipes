using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_currentSong;

    [SerializeField]
    private AudioSource m_winSound;

    [SerializeField]
    private AudioSource m_loseSound;


    private int m_winSoundId;
    private int m_winStreamId;

    private int m_loseSoundId;
    private int m_loseStreamId;


    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidNativeAudio.makePool(2);
        m_winSoundId = AndroidNativeAudio.load("WinSound.wav");
        m_loseSoundId = AndroidNativeAudio.load("LoseSound.wav");
        m_winStreamId = -1;
        m_loseStreamId = -1;
#endif
    }

    void PlayWinSound()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if(m_winStreamId != -1)
        {
            AndroidNativeAudio.stop(m_winStreamId);
        }
        m_winStreamId = AndroidNativeAudio.play(m_winSoundId);
#else
        m_winSound.PlayOneShot(m_winSound.clip);
#endif
    }

    void PlayLoseSound()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (m_loseStreamId != -1)
        {
            AndroidNativeAudio.stop(m_loseStreamId);
        }
        m_loseStreamId = AndroidNativeAudio.play(m_loseSoundId);
#else
        m_loseSound.PlayOneShot(m_loseSound.clip);
#endif
    }

    public float GetTimePassed()
    {
        return m_currentSong.time;
    }
}
