using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
 
public struct EnemySpawnMsg
{
    public Dictionary<string, int> spawnNums;
    public List<Vector2> positions;
}

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
    private Dictionary<string, Queue<GameObject>> _objectDict = new();

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        PostManager.Instance.Subscribe<EnemySpawnMsg>(PostMessageKey.EnemySpawned, SpawnEnemy);
        PostManager.Instance.Subscribe<EnemyDespawnMsg>(PostMessageKey.EnemyDespawned, Despawn);
    }

    private void OnDisable()
    {
        PostManager.Instance.Unsubscribe<EnemySpawnMsg>(PostMessageKey.EnemySpawned, SpawnEnemy);
        PostManager.Instance.Unsubscribe<EnemyDespawnMsg>(PostMessageKey.EnemyDespawned, Despawn);
    }
    
    private void Init()
    {
        // 몬스터 미리 생성
        foreach (var prefab in _enemyPrefabs)
        {
            if (!_objectDict.ContainsKey(prefab.name))
            {
                _objectDict[prefab.name] = new Queue<GameObject>();
            }

            for (int i = 0; i < _maxNumPerEnemy; i++)
            {
                GameObject obj = Instantiate(prefab, gameObject.transform, true);
                obj.name = $"{prefab.name}_{i:D3}";
                obj.SetActive(false);
                _objectDict[prefab.name].Enqueue(obj);    
            }
        }
        
        // 몬스터 팝업시 이팩트 미리 생성
        _objectDict[_spawnEffectPrefab.name] = new Queue<GameObject>();
        for (int i = 0; i < 15; i++)
        {
            GameObject obj = Instantiate(_spawnEffectPrefab, gameObject.transform, true);
            obj.name = _spawnEffectPrefab.name;
            obj.SetActive(false);
            _objectDict[_spawnEffectPrefab.name].Enqueue(obj);
        }
    }

    /// <summary>
    /// 실제 다른 객체에서 제어 가능한 함수.
    /// </summary>
    /// <param name="msg">EnemySpawnMsg 데이터</param>
    private void SpawnEnemy(EnemySpawnMsg msg) => Spwan(msg.spawnNums, msg.positions);

    /// <summary>
    /// 일반 몬스터 스폰 함수. 
    /// </summary>
    /// <param name="spawnNums">몬스터별 스폰 개수</param>
    /// <param name="positions">몬스터 스폰 위치 리스트</param>
    /// <returns></returns>
    private List<GameObject> Spwan(Dictionary<string, int> spawnNums, List<Vector2> positions)
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
    private List<GameObject> Spwan(Dictionary<string, int> spawnNums, Vector2 position)
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
    private IEnumerator SpawnEach(string name, Vector2 position, System.Action<GameObject> returnObj)
    {
        // 소환 이팩트 생성
        yield return new WaitForSeconds(0.1f);
        GameObject effectObj = _objectDict[_spawnEffectPrefab.name].Dequeue();
        effectObj.transform.position = new Vector2(position.x, position.y - 0.2f);
        effectObj.SetActive(true);
        yield return new WaitForSeconds(1f);
        
        // 몬스터 소환
        GameObject obj = _objectDict[name].Dequeue();
        obj.transform.position = position;
        obj.SetActive(true);
        yield return new WaitForSeconds(1f);
        
        // 소환 이팩트 사라짐
        effectObj.SetActive(false);
        _objectDict[_spawnEffectPrefab.name].Enqueue(effectObj);
        returnObj?.Invoke(obj);
    }

    private void Despawn(EnemyDespawnMsg msg)
    {
        msg.obj.SetActive(false);
        _objectDict[msg.name].Enqueue(msg.obj);
    }

    [ContextMenu("Test/Spawn")]
    public void TestSpawn()
    {
        Dictionary<string, int> spawnNums = new Dictionary<string, int>();
        // spawnNums.Add("MonsterChase", 2);
        spawnNums.Add("MonsterRange", 1);
        // spawnNums.Add("MonsterTank", 1);
        
        List<Vector2> positions = new List<Vector2>();
        positions.Add(new Vector2(-4.16f, -2.12f));
        // positions.Add(new Vector2(4.16f, 2.12f));
        // positions.Add(new Vector2(-4.16f, 2.12f));
        // positions.Add(new Vector2(0, 0));
        // positions.Add(new Vector2(-1f, 1f));
        
        Spwan(spawnNums, positions);
    }
}
