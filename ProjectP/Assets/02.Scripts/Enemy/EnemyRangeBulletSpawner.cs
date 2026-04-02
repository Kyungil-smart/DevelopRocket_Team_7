using System.Collections.Generic;
using UnityEngine;


public struct EnemyRangeBulletSpawnMsg
{
    public Vector2 startPos;
    public Vector2 direction;
    public int damage;
}

public class EnemyRangeBulletSpawner : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] private int _maxNumPerEnemy = 10;
    private Queue<GameObject> _pool = new();
    
    private void Start()
    {
        Init();
    }

    private void Init() => CreateBullets();
    
    private void OnEnable()
    {
        PostManager.Instance.Subscribe<EnemyRangeBulletSpawnMsg>(PostMessageKey.EnemyRangeBulletSpawned, Spawn);
        PostManager.Instance.Subscribe<GameObject>(PostMessageKey.EnemyRangeBulletDespawned, Despawn);
    }

    private void OnDisable()
    {
        PostManager.Instance.Unsubscribe<EnemyRangeBulletSpawnMsg>(PostMessageKey.EnemyRangeBulletSpawned, Spawn);
        PostManager.Instance.Unsubscribe<GameObject>(PostMessageKey.EnemyRangeBulletDespawned, Despawn);
    }

    private void CreateBullets()
    {
        for (int i = 0; i < _maxNumPerEnemy; i++)
        {
            GameObject obj = Instantiate(_prefab, gameObject.transform, true);
            obj.name = $"{_prefab.name}";
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }   
    }
    
    private void Spawn(EnemyRangeBulletSpawnMsg msg)
    {
        if (_pool.Count <= 1) CreateBullets();
        GameObject obj = _pool.Dequeue();
        obj.transform.position = msg.startPos;
        EnemyRangeBulletAttack ak = obj.GetComponent<EnemyRangeBulletAttack>();
        ak.SetDamage(msg.damage);
        EnemyRangeBulletMovement mv = obj.GetComponent<EnemyRangeBulletMovement>();
        obj.SetActive(true);
        mv.SetDirection(msg.direction);
        mv.FireBullet();
    }

    private void Despawn(GameObject obj)
    {
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }
}