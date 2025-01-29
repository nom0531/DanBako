using UnityEngine;

public class Deleto : MonoBehaviour
{
    [SerializeField] private GameObject targetObject; // アクティブ化するオブジェクト
    [SerializeField] private BoxCollider[] targetBoxColliders; // 無効化したいBoxColliderの配列

    private void Start()
    {
        // 念のため初期状態でtargetObjectを非アクティブ化
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        // このオブジェクトが削除されたときに実行
        if (targetObject != null)
        {
            targetObject.SetActive(true); // 指定オブジェクトをアクティブ化
        }

        if (targetBoxColliders != null)
        {
            foreach (var collider in targetBoxColliders)
            {
                if (collider != null)
                {
                    collider.enabled = false; // 指定したBoxColliderを無効化
                }
            }
        }
    }
}
