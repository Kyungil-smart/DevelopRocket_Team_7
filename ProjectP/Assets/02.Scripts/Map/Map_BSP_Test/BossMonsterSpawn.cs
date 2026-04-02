using UnityEngine;

public class BossMonsterSpawn : MonoBehaviour
{
    public GameObject bossMonsterPrefab; // 보스 몬스터 프리팹
    public Vector2 spawnPosition;
    private bool isSpawned = false; // 보스 몬스터가 이미 소환되었는지 여부
    private void OnTriggerEnter2D(Collider2D collision)
    {
          if (collision.CompareTag("Player"))
          {
            if(isSpawned ==false)
            {
                Instantiate(bossMonsterPrefab,spawnPosition,Quaternion.identity);
                isSpawned = true;
            }
          }

    }
}
