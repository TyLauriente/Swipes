using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> m_sr_backgroundImages;

    [SerializeField]
    private SpriteRenderer m_sr_background1;

    [SerializeField]
    private SpriteRenderer m_sr_background2;


    private const float startingZValue = 4.0f;

    private bool background1OnTop;

    // Start is called before the first frame update
    void Start()
    {
        background1OnTop = true;
    }

    public void SetBackgroundPosition(Vector3 pos)
    {
        pos.z = startingZValue;
        m_sr_background1.transform.position = pos;
    }

    public void SetNextBackground(int nextBackground)
    {
        if (nextBackground < m_sr_backgroundImages.Count && nextBackground > 0)
        {
            m_sr_background1.transform.position = new Vector3(0.0f, 0.0f, m_sr_background1.transform.position.z);

            m_sr_background1.sprite = m_sr_background2.sprite;
            m_sr_background2.sprite = m_sr_backgroundImages[nextBackground];
        }
    }
}
