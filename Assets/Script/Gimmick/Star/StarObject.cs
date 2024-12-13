using UnityEngine;

public class StarObject : MonoBehaviour
{
    private SE m_starSE;
    private StarCount m_starCount;
    private GameManager m_gameManager;

    private void Start()
    {
        m_gameManager = GameManager.Instance;
        m_starCount = GameObject.FindGameObjectWithTag("MainUI_StarCount").GetComponent<StarCount>();
        m_starSE = GetComponent<SE>();
    }

    private void FixedUpdate()
    {
        if (m_gameManager.GameMode == CurrentGameMode.enPause)
        {
            return;
        }
        // 自身を回転させる。
        transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f));
    }

    private void OnTriggerStay(Collider other)
    {
        //接触したオブジェクトのタグが"Player"のとき
        if (other.CompareTag("Player"))
        {
            m_starCount.StarAdd();
            if (m_starCount.MaxStarCount != m_starCount.NowStarCount)
            {
                //最後の星じゃないなら効果音再生
                m_starSE.PlaySE();
            }
            Destroy(gameObject);
        }
    }
}
