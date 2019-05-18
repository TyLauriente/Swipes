#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private List<Level> m_levels;


    void Start()
    {
        m_levels = new List<Level>();
        Level level = new Level();

        for(float time = 1.1f; time < 150.0f; time += 1.1f)
        {
            level.AddSwipe(time, (Swipes)Random.Range(1, 5));
        }

        m_levels.Add(level);
    }

    public Level GetLevel(int index)
    {
        if (index >= 0 && index < m_levels.Count)
        {
            return m_levels[index];
        }
        return new Level();
    }
    
    
}
