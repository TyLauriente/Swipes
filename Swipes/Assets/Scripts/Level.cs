using System.Collections;
using UnityEngine;
using System.IO;
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
    public string levelName;
    public List<float> swipeTimes;
    public List<Swipes> swipes;
    public List<int> backgroundIndexes;
    public string musicName;
    public bool isPrimaryLevel;
    

    public Level()
    {
        swipeTimes = new List<float>();
        swipes = new List<Swipes>();
        backgroundIndexes = new List<int>();
        isPrimaryLevel = true;
    }

    public void AddSwipe(float seconds, Swipes swipe, int backgroundIndex)
    {
        swipeTimes.Add(seconds);
        swipes.Add(swipe);
        backgroundIndexes.Add(backgroundIndex);
    }

    public float GetSwipeTime(int swipeIndex)
    {
        if(swipeIndex >= 0 && swipeIndex < swipeTimes.Count)
        {
            return swipeTimes[swipeIndex];
        }
        return -1;
    }

    public Swipes GetSwipe(int swipeIndex)
    {
        if (swipeIndex >= 0 && swipeIndex < swipes.Count)
        {
            return swipes[swipeIndex];
        }
        return Swipes.Invalid;
    }

    public int GetBackgroundIndex(int swipeIndex)
    {
        if (swipeIndex < backgroundIndexes.Count)
        {
            return backgroundIndexes[swipeIndex];
        }
        return -1;
    }

}

