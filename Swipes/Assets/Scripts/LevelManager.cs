using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * This Class is the LevelManager
 * 
 * It controls the different levels the game has
 * and allows access to these levels.
 * 
 * 
 * Authored by Ty Lauriente
 * 05/15/2019
 * 
 * */

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
        if (m_levels != null && index >= 0 && index < m_levels.Count)
        {
            return m_levels[index];
        }
        return new Level();
    }
    
    
}
