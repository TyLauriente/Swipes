using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LevelStats
{
    private string m_levelName;
    private float m_accuracy;
    private int m_misses;

    public string LevelName { get => m_levelName; set => m_levelName = value; }
    public float Accuracy { get => m_accuracy; set => m_accuracy = value; }
    public int Misses { get => m_misses; set => m_misses = value; }

    public LevelStats()
    {
        m_levelName = "";
        m_accuracy = 0.0f;
        m_misses = 0;
    }

}

