using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * 
 * This Class is the SwipeManager
 * 
 * It controls the current swipe and the next swipe movement
 * and syncronization with the music
 * 
 * 
 * Authored by Ty Lauriente
 * 05/15/2019
 * 
 * */

public class SwipeManager : MonoBehaviour
{
    // The current swipe and next swipe variables
    [SerializeField]
    private SpriteRenderer m_SR_currentSwipe;
    [SerializeField]
    private SpriteRenderer m_SR_currentSwipeIndicator;
    [SerializeField]
    private SpriteRenderer m_SR_nextSwipe;
    [SerializeField]
    private AudioManager m_audioManager;
    [SerializeField]
    private GameplayManager m_gameplayManager;

    // Distance to offset the small arrow
    private const float offset = 1.425f;

    // Used to offset the small arrow
    private Swipes m_swipeType;
    private float m_start;
    private float m_end;
    private float m_percentage;
    private float m_divider;

    Vector3 m_newIndicatiorPos;


    private void Start()
    {
        m_start = 0.0f;
        m_start = 0.0f;
        m_percentage = 0.0f;
        m_divider = 0.0f;
    }

    public void Update()
    {
        if ((m_end - m_start) != 0.0f)
        {
            m_percentage = (m_audioManager.GetTimePassed() - m_start) * m_divider;
            if (m_percentage > 1.0f)
            {
                m_percentage = 1.0f;
            }
            else if(m_percentage < 0.0f)
            {
                m_percentage = 0.0f;
            }
        }

        m_newIndicatiorPos = m_SR_currentSwipe.transform.position;
        m_newIndicatiorPos.z = m_SR_currentSwipeIndicator.transform.position.z;

        if (m_swipeType == Swipes.Up)
        {
            m_newIndicatiorPos.y += -offset + (m_percentage * offset * 2.0f);
        }
        else if (m_swipeType == Swipes.Down)
        {
            m_newIndicatiorPos.y += offset - (m_percentage * offset * 2.0f);
        }
        else if (m_swipeType == Swipes.Left)
        {
            m_newIndicatiorPos.x += offset - (m_percentage * offset * 2.0f);
        }
        else if (m_swipeType == Swipes.Right)
        {
            m_newIndicatiorPos.x += -offset + (m_percentage * offset * 2.0f);
        }

        m_SR_currentSwipeIndicator.transform.position = m_newIndicatiorPos;
    }

    public void SetCurrentSwipeLocation(Vector2 location)
    {
        m_SR_currentSwipe.transform.position =
            new Vector3(location.x, location.y, m_SR_currentSwipe.transform.position.z);
        m_SR_nextSwipe.gameObject.SetActive(false);
    }

    public void SetCurrentSwipeType(Swipes swipe)
    {
        m_swipeType = swipe;
        m_SR_currentSwipe.transform.position = new Vector3(0.0f, 0.0f, m_SR_currentSwipe.transform.position.z);
        m_SR_currentSwipe.transform.rotation = Quaternion.identity;


        if(swipe == Swipes.Up)
        {
            m_SR_currentSwipe.transform.Rotate(new Vector3(0.0f, 0.0f, 0.0f));
        }
        else if(swipe == Swipes.Down)
        {
            m_SR_currentSwipe.transform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));
        }
        else if(swipe == Swipes.Left)
        {
            m_SR_currentSwipe.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
        }
        else if(swipe == Swipes.Right)
        {
            m_SR_currentSwipe.transform.Rotate(new Vector3(0.0f, 0.0f, 270.0f));
        }

        m_SR_currentSwipeIndicator.transform.rotation = m_SR_currentSwipe.transform.rotation;

        m_start = m_audioManager.GetTimePassed();
        m_end = m_start + m_gameplayManager.GetTimeUntilNextSwipe();
        m_divider = 1.0f / (m_end - m_start);
    }

    public void SetNextSwipeType(Swipes swipe)
    {
        if(swipe == Swipes.Invalid)
        {
            m_SR_nextSwipe.gameObject.SetActive(false);
            return;
        }
        m_SR_nextSwipe.transform.position = new Vector3(0.0f, 0.0f, m_SR_nextSwipe.transform.position.z);
        m_SR_nextSwipe.transform.rotation = Quaternion.identity;

        if (swipe == Swipes.Up)
        {
            m_SR_nextSwipe.transform.Rotate(new Vector3(0.0f, 0.0f, 0.0f));
        }
        else if (swipe == Swipes.Down)
        {
            m_SR_nextSwipe.transform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));
        }
        else if (swipe == Swipes.Left)
        {
            m_SR_nextSwipe.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
        }
        else if (swipe == Swipes.Right)
        {
            m_SR_nextSwipe.transform.Rotate(new Vector3(0.0f, 0.0f, 270.0f));
        }
    }

    public Vector3 GetCurrentSwipePosition()
    {
        return m_SR_currentSwipe.transform.position;
    }
}
