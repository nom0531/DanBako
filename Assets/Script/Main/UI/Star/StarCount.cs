using UnityEngine;

public class StarCount : MonoBehaviour
{
    private int m_nowStarCount = 0;
    private int m_maxStarCount = 0;

    public int NowStarCount
    {
        get => m_nowStarCount;
    }

    public int MaxStarCount
    {
        get => m_maxStarCount;
    }

    void Start()
    {
        GameObject[] stars = GameObject.FindGameObjectsWithTag("Star");
        m_maxStarCount = stars.Length;    //要素数はオブジェクトの数
    }

    //所持している星を増やす
    public void StarAdd()
    {
        m_nowStarCount++;
    }
}
