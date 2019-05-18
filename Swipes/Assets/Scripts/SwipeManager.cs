using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField]
    private GameManager m_gameManager;
    [SerializeField]
    private LevelManager m_levelmanager;
    [SerializeField]
    private SpriteRenderer m_SR_currentSwipe;
    [SerializeField]
    private SpriteRenderer m_SR_nextSwipe;

    public void SetCurrentSwipeLocation(Vector2 location)
    {
        m_SR_currentSwipe.transform.position =
            new Vector3(location.x, location.y, m_SR_currentSwipe.transform.position.z);
        m_SR_nextSwipe.gameObject.SetActive(false);
    }

    public void SetCurrentSwipeType(Swipes swipe)
    {
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

    public Vector2 GetCurrentSwipePosition()
    {
        return (Vector2)m_SR_currentSwipe.transform.position;
    }
}
