using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public struct EnemyDespawnMsg
{
    public string name;
    public GameObject obj;
}


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> _enemyPrefabs;
    [SerializeField] GameObject _spawnEffectPrefab;
    [SerializeField] private int _maxNumPerEnemy = 10;
    public Dictionary<string, Queue<GameObject>> objectDict = new();

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        PostManager.Instance.Subscribe<EnemyDespawnMsg>(PostMessageKey.EnemyDespawned, Despawn);
    }

    private void OnDisable()
    {
        PostManager.Instance.Unsubscribe<EnemyDespawnMsg>(PostMessageKey.EnemyDespawned, Despawn);
    }
    
    private void Init()
    {
        // 몬스터 미리 생성
        foreach (var prefab in _enemyPrefabs)
        {
            if (!objectDict.ContainsKey(prefab.name))
            {
                objectDict[prefab.name] = new Queue<GameObject>();
            }

            for (int i = 0; i < _maxNumPerEnemy; i++)
            {
                GameObject obj = Instantiate(prefab, gameObject.transform, true);
                obj.name = $"{prefab.name}_{i:D3}";
                obj.SetActive(false);
                objectDict[prefab.name].Enqueue(obj);    
            }
        }
        
        // 몬스터 팝업시 이팩트 미리 생성
        objectDict[_spawnEffectPrefab.name] = new Queue<GameObject>();
        for (int i = 0; i < 15; i++)
        {
            GameObject obj = Instantiate(_spawnEffectPrefab, gameObject.transform, true);
            obj.name = _spawnEffectPrefab.name;
            obj.SetActive(false);
            objectDict[_spawnEffectPrefab.name].Enqueue(obj);
        }
    }

    /// <summary>
    /// 일반 몬스터 스폰 함수. 
    /// </summary>
    /// <param name="spawnNums">몬스터별 스폰 개수</param>
    /// <param name="positions">몬스터 스폰 위치 리스트</param>
    /// <returns></returns>
    public List<GameObject> Spwan(Dictionary<string, int> spawnNums, List<Vector2> positions)
    {
        List<GameObject> spawnedEnemies = new List<GameObject>();
        Queue<Vector2> spawnedPositions = new Queue<Vector2>(positions);
        foreach (var spawnNum in spawnNums)
        {
            string name = spawnNum.Key;
            int num = spawnNum.Value;
            for (int i = 0; i < num; i++)
            {
                StartCoroutine(SpawnEach(name, spawnedPositions.Dequeue(), 
                    spawnedObj => {
                    spawnedEnemies.Add(spawnedObj);
                }));

            }
        }
        return spawnedEnemies;
    }
    
    /// <summary>
    /// 일반 몬스터 스폰 함수. 한 장소에만 스폰 시킬 경우
    /// </summary>
    /// <param name="spawnNums">몬스터별 스폰 개수</param>
    /// <param name="position">몬스터 스폰 위치</param>
    /// <returns></returns>
    public List<GameObject> Spwan(Dictionary<string, int> spawnNums, Vector2 position)
    {
        List<GameObject> spawnedEnemies = new List<GameObject>();
        foreach (var spawnNum in spawnNums)
        {
            string name = spawnNum.Key;
            int num = spawnNum.Value;
            for (int i = 0; i < num; i++)
            {
                StartCoroutine(SpawnEach(name, position, 
                    spawnedObj => {
                    spawnedEnemies.Add(spawnedObj);
                }));
            }
        }
        return spawnedEnemies;
    }
    
    /// <summary>
    /// 단일 몬스터 스폰 함수
    /// </summary>
    /// <param name="name">Prefab name</param>
    /// <param name="position">스폰 위치</param>
    /// <returns></returns>
    public IEnumerator SpawnEach(string name, Vector2 position, System.Action<GameObject> returnObj)
    {
        // 소환 이팩트 생성
        yield return new WaitForSeconds(0.1f);
        GameObject effectObj = objectDict[_spawnEffectPrefab.name].Dequeue();
        effectObj.transform.position = new Vector2(position.x, position.y - 0.5f);
        effectObj.SetActive(true);
        yield return new WaitForSeconds(1f);
        
        // 몬스터 소환
        GameObject obj = objectDict[name].Dequeue();
        Debug.Log($"Spawned: {obj.name}");
        obj.transform.position = position;
        obj.SetActive(true);
        yield return new WaitForSeconds(1f);
        
        // 소환 이팩트 사라짐
        effectObj.SetActive(false);
        objectDict[_spawnEffectPrefab.name].Enqueue(effectObj);
        returnObj?.Invoke(obj);
    }

    public void Despawn(EnemyDespawnMsg msg)
    {
        msg.obj.SetActive(false);
        objectDict[msg.name].Enqueue(msg.obj);
    }

    [ContextMenu("Test/Spawn")]
    public void TestSpawn()
    {
        Dictionary<string, int> spawnNums = new Dictionary<string, int>();
        spawnNums.Add("Enemy", 5);
        
        List<Vector2> positions = new List<Vector2>();
        positions.Add(new Vector2(-16, -8));
        positions.Add(new Vector2(16, 8));
        positions.Add(new Vector2(-16, 8));
        positions.Add(new Vector2(0, 0));
        positions.Add(new Vector2(-4, 4));
        
        Spwan(spawnNums, positions);
    }
}
