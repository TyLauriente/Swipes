using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * This Class is the InputManager
 * 
 * It keeps track of all input events
 * and has functions to access them
 * 
 * Authored by Ty Lauriente
 * 05/15/2019
 * 
 * */

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private GameManager m_gameManager;
    private bool m_isFirstTouch;
    private bool m_isFirstRelease;
    private bool m_isDragging;

    // Start is called before the first frame update
    void Start()
    {
        m_isFirstTouch = false;
        m_isFirstRelease = false;
        m_isDragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        m_isFirstTouch = false;
        m_isFirstRelease = false;
        if (Input.GetMouseButtonDown(0))
        {
            m_isFirstTouch = true;
            m_isDragging = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            m_isDragging = false;
            m_isFirstRelease = true;
        }
    }

    public bool IsFirstTouch()
    {
        return m_isFirstTouch;
    }

    public bool IsFirstRelease()
    {
        return m_isFirstRelease;
    }

    public bool IsDragging()
    {
        return m_isDragging;
        
    }
}
