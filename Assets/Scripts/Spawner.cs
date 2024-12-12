using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("プレイヤー設定")]
    public GameObject playerPrefab; // プレイヤープレハブ
    public Transform playerSpawnPosition; // プレイヤーのスポーン位置

    [Header("エネミー設定")]
    public List<GameObject> enemyPrefabs; // 複数のエネミープレハブ
    public Transform[] enemySpawnPositions; // エネミーのスポーン位置（複数対応）

    private GameObject spawnedPlayer;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        SpawnPlayer();
        SpawnAllEnemies();
    }

    public void SpawnPlayer()
    {
        if (playerPrefab == null || playerSpawnPosition == null)
        {
            Debug.LogError("プレイヤープレハブまたはスポーン位置が設定されていません");
            return;
        }

        // 既存のプレイヤーを削除
        if (spawnedPlayer != null)
        {
            Destroy(spawnedPlayer);
        }

        // プレイヤーをスポーン
        spawnedPlayer = Instantiate(playerPrefab, playerSpawnPosition.position, playerSpawnPosition.rotation);
        Debug.Log("プレイヤーをスポーンしました");
    }

    public void SpawnEnemy(int enemyIndex, int positionIndex)
    {
        if (enemyIndex < 0 || enemyIndex >= enemyPrefabs.Count)
        {
            Debug.LogError($"無効なエネミーインデックス: {enemyIndex}");
            return;
        }

        if (positionIndex < 0 || positionIndex >= enemySpawnPositions.Length)
        {
            Debug.LogError($"無効なスポーン位置インデックス: {positionIndex}");
            return;
        }

        // 指定されたエネミーをスポーン
        GameObject enemy = Instantiate(
            enemyPrefabs[enemyIndex],
            enemySpawnPositions[positionIndex].position,
            enemySpawnPositions[positionIndex].rotation
        );

        spawnedEnemies.Add(enemy);
        Debug.Log($"エネミー '{enemyPrefabs[enemyIndex].name}' を位置 {positionIndex + 1} にスポーンしました");
    }

    public void SpawnAllEnemies()
    {
        if (enemyPrefabs.Count == 0 || enemySpawnPositions.Length == 0)
        {
            Debug.LogError("エネミープレハブまたはスポーン位置が設定されていません");
            return;
        }

        // 各スポーン位置にランダムなエネミーをスポーン
        for (int i = 0; i < enemySpawnPositions.Length; i++)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            SpawnEnemy(randomIndex, i);
        }
    }

    public void RespawnEnemy(int enemyIndex, Vector3 newSpawnPosition)
    {
        if (enemyIndex < 0 || enemyIndex >= spawnedEnemies.Count)
        {
            Debug.LogError($"無効なエネミーインデックス: {enemyIndex}");
            return;
        }

        // 古いエネミーを削除
        if (spawnedEnemies[enemyIndex] != null)
        {
            Destroy(spawnedEnemies[enemyIndex]);
        }

        // 新しいエネミーをリスポーン
        GameObject enemy = Instantiate(
            enemyPrefabs[enemyIndex],
            newSpawnPosition,
            Quaternion.identity
        );

        spawnedEnemies[enemyIndex] = enemy;
        Debug.Log($"エネミー '{enemyPrefabs[enemyIndex].name}' を新しい位置にリスポーンしました");
    }
}
