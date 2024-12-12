using UnityEngine;
using TMPro;

public class UI_NowStarCount : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI StarText;

    private StarCount m_starCount;
    private int m_oldStarCount = 0;

    private void Start()
    {
        m_starCount = GetComponent<StarCount>();
        StarTextUpdate();
    }

    private void FixedUpdate()
    {
        StarTextUpdate();
    }

    private void StarTextUpdate()
    {
        StarText.text = m_starCount.NowStarCount.ToString();

        if(m_oldStarCount != m_starCount.NowStarCount)
        {
            m_oldStarCount = m_starCount.NowStarCount;
        }
    }
}
