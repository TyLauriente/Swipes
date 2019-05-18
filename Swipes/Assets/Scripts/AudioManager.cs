using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]
    private AudioSource m_currentSong;

    //[SerializeField]
    //private AudioSource m_winSound;

    //[SerializeField]
    //private AudioSource m_loseSound;

    // Android Native Audio variables
    private int m_winSoundId;
    private int m_winStreamId;

    private int m_loseSoundId;
    private int m_loseStreamId;


    // Initialize Android Native audio with sound effects
    void Start()
    {
        AndroidNativeAudio.makePool(2);
        m_winSoundId = AndroidNativeAudio.load("WinSound.wav");
        m_loseSoundId = AndroidNativeAudio.load("LoseSound.wav");
        m_winStreamId = -1;
        m_loseStreamId = -1;
    }

    public void PlayWinSound()
    {
        m_winStreamId = AndroidNativeAudio.play(m_winSoundId);

        //m_winSound.PlayOneShot(m_winSound.clip);

    }

    public void PlayLoseSound()
    {
        m_loseStreamId = AndroidNativeAudio.play(m_loseSoundId);
        //m_loseSound.PlayOneShot(m_loseSound.clip);
    }

    public float GetTimePassed()
    {
        return m_currentSong.time;
    }
}
