using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField]
    private GameManager m_gameManager;
    [SerializeField]
    private AudioManager m_audioManager;
    [SerializeField]
    private UserSettingsManager m_userSettingsManager;


    [SerializeField]
    private InputField m_swipeTimeIncrementInputField;
    [SerializeField]
    private Slider m_musicVolumeSlider;
    [SerializeField]
    private Slider m_soundEffectVolumeSlider;
    [SerializeField]
    private Button m_quitButton;
    [SerializeField]
    private Button m_restoreDefaultsButton;


    void Start()
    {
        m_quitButton.onClick.AddListener(QuitOptions);
        m_restoreDefaultsButton.onClick.AddListener(RestoreDefaults);
        Init();
    }

    public void Init()
    {
        m_swipeTimeIncrementInputField.text = m_userSettingsManager.UserSettings.EditorSwipeTimeIncrement.ToString();
        m_musicVolumeSlider.value = m_userSettingsManager.UserSettings.SongVolume;
        m_soundEffectVolumeSlider.value = m_userSettingsManager.UserSettings.SoundEffectVolume;
    }

    void Update()
    {
        if(m_gameManager.GetCurrentState() == GameStates.Options)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                m_audioManager.PlayStuckSound();
                UpdateValues();
                m_gameManager.ChangeState(GameStates.MainMenu);
            }
        }
    }

    private void QuitOptions()
    {
        m_audioManager.PlayStuckSound();
        m_gameManager.ChangeState(GameStates.MainMenu);
        UpdateValues();
    }

    private void RestoreDefaults()
    {
        m_audioManager.PlaySuccessfulMenuNavigationSound();
        m_swipeTimeIncrementInputField.text = GameManager.DEFAULT_EDITOR_SWIPE_TIME_INCREMENT.ToString();
        m_musicVolumeSlider.value = GameManager.DEFAULT_SONG_VOLUME;
        m_soundEffectVolumeSlider.value = GameManager.DEFAULT_EFFECT_VOLUME;
    }

    private void UpdateValues()
    {
        float swipeIncrementValue = float.Parse(m_swipeTimeIncrementInputField.text);
        if (swipeIncrementValue > 0.0f)
        {
            m_userSettingsManager.UserSettings.EditorSwipeTimeIncrement = swipeIncrementValue;
        }
        m_userSettingsManager.UserSettings.SongVolume = m_musicVolumeSlider.value;
        m_userSettingsManager.UserSettings.SoundEffectVolume = m_soundEffectVolumeSlider.value;
        m_userSettingsManager.SaveUserSettings();
    }
}
