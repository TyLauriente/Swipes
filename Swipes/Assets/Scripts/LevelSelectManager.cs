using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private LevelManager m_levelManager;
    [SerializeField]
    private GameManager m_gameManager;
    [SerializeField]
    private Button btn1;
    [SerializeField]
    private Button btn2;
    [SerializeField]
    private Text btn1Text;
    [SerializeField]
    private Text btn2Text;

    Level userLevel, primaryLevel;

    bool init = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!init)
        {
            userLevel = m_levelManager.GetUserLevel();
            primaryLevel = m_levelManager.GetPrimaryLevel();

            if (userLevel.levelName != "" && userLevel.GetSwipe(0) != Swipes.Invalid)
            {
                btn1Text.text = "UserLevel";
                btn1.onClick.AddListener(UserLevel);
            }
            else
            {
                btn1Text.text = "Invalid User\nLevel";
            }
            if (primaryLevel.levelName != "")
            {
                btn2Text.text = "PrimaryLevel";
                btn2.onClick.AddListener(PrimaryLevel);
            }
            else
            {
                btn2Text.text = "Invalid Primary\nLevel";
            }
            init = true;
        }
    }

    public void Reset()
    {
        init = false;
    }

    void UserLevel()
    {
        m_gameManager.SetNextLevel(userLevel);
        m_gameManager.ChangeState(GameStates.Gameplay);
    }

    void PrimaryLevel()
    {
        m_gameManager.SetNextLevel(primaryLevel);
        m_gameManager.ChangeState(GameStates.Gameplay);
    }

}
