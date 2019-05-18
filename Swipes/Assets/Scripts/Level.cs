#pragma warning disable 0649
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Level
{
    string m_levelName;
    List<float> m_swipeTimes;
    List<Swipes> m_swipes;

    public Level()
    {
        m_swipeTimes = new List<float>();
        m_swipes = new List<Swipes>();
    }

    public void AddSwipe(float seconds, Swipes swipe)
    {
        m_swipeTimes.Add(seconds);
        m_swipes.Add(swipe);
    }

    public float GetSwipeTime(int index)
    {
        if(index >= 0 && index < m_swipeTimes.Count)
        {
            return m_swipeTimes[index];
        }
        return -1;
    }

    public Swipes GetSwipe(int index)
    {
        if (index >= 0 && index < m_swipes.Count)
        {
            return m_swipes[index];
        }
        return Swipes.Invalid;
    }
}

