using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyPrefabs;
    public Dictionary<string, Queue<GameObject>> objectDict = new();

    private void Start()
    {
        Init();
    }
    
    private void Init()
    {
        foreach (var prefab in enemyPrefabs)
        {
            if (!objectDict.ContainsKey(prefab.name))
            {
                objectDict[prefab.name] = new Queue<GameObject>();
            }

            for (int i = 0; i < 10; i++)
            {
                GameObject obj = Instantiate(prefab, gameObject.transform, true);
                obj.name = prefab.name;
                obj.SetActive(false);
                objectDict[prefab.name].Enqueue(obj);    
            }
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
                spawnedEnemies.Add(SpawnEach(name, spawnedPositions.Dequeue()));
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
                spawnedEnemies.Add(SpawnEach(name, position));
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
    public GameObject SpawnEach(string name, Vector2 position)
    {
        GameObject obj = objectDict[name].Dequeue();
        obj.transform.position = position;
        obj.SetActive(true);
        return obj;
    }

    public void Despawn(string name, GameObject obj)
    {
        obj.SetActive(false);
        objectDict[name].Enqueue(obj);
    }
}
