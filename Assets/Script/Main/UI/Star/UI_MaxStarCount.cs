using UnityEngine;
using TMPro;

public class UI_MaxStarCount : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI StarText;

    private StarCount m_starCount;

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
        StarText.text = "/" + m_starCount.MaxStarCount.ToString();
    }
}
