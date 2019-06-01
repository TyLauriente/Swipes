using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameManager m_gameManager;
    [SerializeField]
    private InputManager m_inputManager;
    [SerializeField]
    private SpriteRenderer m_sr_menuImage;

    private const float verticalSpeed = 7.0f;
    private const float horizontalSpeed = verticalSpeed * 0.5f;

    private Vector2 m_touchPos;
    private Vector2 m_startTouchPosition;

    bool waitOver = false;

    // Update is called once per frame
    void Update()
    {
        if (m_gameManager.GetCurrentState() == GameStates.MainMenu && waitOver)
        {
            UpdatePositions();

            CheckIsDragging();

            CheckRelease();
        }
    }

    public void Wait()
    {
        waitOver = false;
        Invoke("WaitOver", 0.15f);
    }

    private void WaitOver()
    {
        waitOver = true;
    }


    private void UpdatePositions()
    {
        m_touchPos = Input.mousePosition;

        m_touchPos.x = ((m_touchPos.x - m_gameManager.GetScreenWidth()) / m_gameManager.GetScreenWidth()
            * horizontalSpeed);
        m_touchPos.y = ((m_touchPos.y - m_gameManager.GetScreenHeight()) / m_gameManager.GetScreenHeight()
            * verticalSpeed);


        if (m_inputManager.IsFirstTouch())
        {
            m_startTouchPosition = m_touchPos;
        }
    }

    private void CheckIsDragging()
    {
        if (m_inputManager.IsDragging())
        {
            m_touchPos -= m_startTouchPosition;

            if (m_touchPos.x < 0.0f || m_touchPos.x > 0.0f)
            {
                m_sr_menuImage.transform.position = new Vector3(m_touchPos.x, 0.0f, m_sr_menuImage.transform.position.z);
            }

            if (m_touchPos.y > 0.0f || m_touchPos.y < 0.0f)
            {
                m_sr_menuImage.transform.position = new Vector3(0.0f, m_touchPos.y, m_sr_menuImage.transform.position.z);
            }
        }
    }

    private void CheckRelease()
    {
        if (m_inputManager.IsFirstRelease())
        {
            if (m_touchPos.y > m_startTouchPosition.y) // Up - Level Select
            {
                m_gameManager.ChangeState(GameStates.LevelSelect);
            }
            else if (m_touchPos.y < m_startTouchPosition.y) // Down - Level Editor
            {
                m_gameManager.ChangeState(GameStates.LevelEditor);
            }
            else if (m_touchPos.x < m_startTouchPosition.x) // Right -  Tutorial
            {
                m_gameManager.ChangeState(GameStates.Tutorial);
            }
            else                                            // Left - Options
            {
                m_gameManager.ChangeState(GameStates.Options);
            }
            m_touchPos = new Vector2();
            m_startTouchPosition = new Vector2();
            m_sr_menuImage.transform.position = new Vector3(0.0f, 0.0f, m_sr_menuImage.transform.position.z);
        }
    }
}
