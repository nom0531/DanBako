using UnityEngine;

public class StarObject : MonoBehaviour
{
    private SE m_starSE;
    private StarCount m_starCount;

    void Start()
    {
        m_starCount = GameObject.FindGameObjectWithTag("MainUI_StarCount").GetComponent<StarCount>();
        m_starSE = GetComponent<SE>();
    }

    void FixedUpdate()
    {
        // 自身を回転させる。
        transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f));
    }

    void OnTriggerStay(Collider other)
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
