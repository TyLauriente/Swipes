using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * 
 * This Class is the Level
 * 
 * It is the structure for the level consisting of
 * a list of times and Swipes (Directions for now)
 * 
 * 
 * 
 * Authored by Ty Lauriente
 * 05/15/2019
 * 
 * */

public class Level
{
    private string m_levelName;
    private List<float> m_swipeTimes;
    private List<Swipes> m_swipes;
    private List<int> m_backgroundIndexes;
    private int m_musicIndex;
    

    public Level()
    {
        m_swipeTimes = new List<float>();
        m_swipes = new List<Swipes>();
        m_backgroundIndexes = new List<int>();
    }

    public void AddSwipe(float seconds, Swipes swipe, int backgroundIndex)
    {
        m_swipeTimes.Add(seconds);
        m_swipes.Add(swipe);
        m_backgroundIndexes.Add(backgroundIndex);
    }

    public float GetSwipeTime(int swipeIndex)
    {
        if(swipeIndex >= 0 && swipeIndex < m_swipeTimes.Count)
        {
            return m_swipeTimes[swipeIndex];
        }
        return -1;
    }

    public Swipes GetSwipe(int swipeIndex)
    {
        if (swipeIndex >= 0 && swipeIndex < m_swipes.Count)
        {
            return m_swipes[swipeIndex];
        }
        return Swipes.Invalid;
    }

    public int GetBackgroundIndex(int swipeIndex)
    {
        if (swipeIndex < m_backgroundIndexes.Count)
        {
            return m_backgroundIndexes[swipeIndex];
        }
        return -1;
    }

    public void SetMusicIndex(int musicIndex)
    {
        m_musicIndex = musicIndex;
    }

    public int GetMusicIndex()
    {
        return m_musicIndex;
    }
}

