using UnityEngine;

public class Deleto : MonoBehaviour
{
    [SerializeField] private GameObject targetObject; // アクティブ化するオブジェクト
    [SerializeField] private BoxCollider targetBoxCollider; // 無効化したいBoxCollider

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

        if (targetBoxCollider != null)
        {
            targetBoxCollider.enabled = false; // 指定したBoxColliderを無効化
        }
    }
}
