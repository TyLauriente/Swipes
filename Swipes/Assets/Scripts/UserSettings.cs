using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class UserSettings
{
    private float m_editorSwipeTimeIncrement;
    private float m_songVolume;
    private float m_soundEffectVolume;

    public float EditorSwipeTimeIncrement { get => m_editorSwipeTimeIncrement; set => m_editorSwipeTimeIncrement = value; }
    public float SongVolume { get => m_songVolume; set => m_songVolume = value; }
    public float SoundEffectVolume { get => m_soundEffectVolume; set => m_soundEffectVolume = value; }

    public UserSettings()
    {
        m_editorSwipeTimeIncrement = GameManager.DEFAULT_EDITOR_SWIPE_TIME_INCREMENT;
        m_songVolume = GameManager.DEFAULT_SONG_VOLUME;
        m_soundEffectVolume = GameManager.DEFAULT_EFFECT_VOLUME;
    }
}