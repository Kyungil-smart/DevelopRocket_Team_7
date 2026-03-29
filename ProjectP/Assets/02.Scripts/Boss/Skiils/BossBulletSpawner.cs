using System.Collections.Generic;
using UnityEngine;

public class BossBulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Queue<GameObject> _bullets = new();

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        PostManager.Instance.Subscribe<GameObject>(PostMessageKey.BossBulletDespawned, DespawnBullet);
    }

    private void OnDisable()
    {
        PostManager.Instance.Unsubscribe<GameObject>(PostMessageKey.BossBulletDespawned, DespawnBullet);
    }
    
    private void Init()
    {
        for (int i = 0; i < 32 * 5; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform, true);
            bullet.SetActive(false);
            _bullets.Enqueue(bullet);
        }
    }

    public void SpawnBullets()
    {
        foreach(var direction in GetDirections(32))
        {
            GameObject bullet = _bullets.Dequeue();
            bullet.SetActive(true);
            bullet.GetComponent<BossBulletMovement>().OnMove(transform.position, direction);
        }
    }

    public void DespawnBullet(GameObject bullet)
    {
        Debug.Log(bullet.name);
        bullet.SetActive(false);
        _bullets.Enqueue(bullet);
    }
    
    private List<Vector2> GetDirections(int step)
    {
        List<Vector2> directions = new List<Vector2>();
        for (int i = 0; i < step; i++)
        {
            // 각도를 라디안으로 변환
            float angle = i * (360f / step) * Mathf.Deg2Rad;

            // 삼각함수를 이용한 방향 벡터 계산
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);

            directions.Add(new Vector2(x, y).normalized);
        }

        return directions;
    }
}