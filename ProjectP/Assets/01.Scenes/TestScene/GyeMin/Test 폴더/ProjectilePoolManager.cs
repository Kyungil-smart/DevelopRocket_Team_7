using System.Collections.Generic;
using UnityEngine;

public class TempProjectilePoolManager : MonoBehaviour
{
    // 구현 원리 요약:
    // 오브젝트를 미리 생성 후 Queue에 넣고 꺼내 쓰고 다시 넣는다

    public static TempProjectilePoolManager Instance;

    [System.Serializable]
    public class Pool
    {
        [Tooltip("풀 이름")]
        public string name;

        [Tooltip("프리팹")]
        public GameObject prefab;

        [Tooltip("초기 개수")]
        public int size;
    }

    [Header("풀 목록")]
    [Tooltip("생성할 풀들")]
    [SerializeField] private List<Pool> pools;

    private Dictionary<string, Queue<GameObject>> poolDict;
    private Dictionary<string, GameObject> prefabDict;

    private void Awake()
    {
        Instance = this;

        poolDict = new Dictionary<string, Queue<GameObject>>();
        prefabDict = new Dictionary<string, GameObject>();

        foreach (var pool in pools)
        {
            if (poolDict.ContainsKey(pool.name))
            {
                Debug.LogWarning($"중복 풀 이름: {pool.name}");
                continue;
            }

            Queue<GameObject> queue = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }

            poolDict.Add(pool.name, queue);
            prefabDict.Add(pool.name, pool.prefab);
        }
    }

    public GameObject Get(string name, Vector3 pos, Quaternion rot)
    {
        if (!poolDict.ContainsKey(name))
        {
            Debug.LogError($"풀 없음: {name}");
            return null;
        }

        Queue<GameObject> pool = poolDict[name];

        GameObject obj;

        // 자동 확장
        if (pool.Count == 0)
        {
            obj = Instantiate(prefabDict[name]);
        }
        else
        {
            obj = pool.Dequeue();
        }

        if (obj == null)
        {
            return Get(name, pos, rot);
        }

        obj.SetActive(true);
        obj.transform.position = pos;
        obj.transform.rotation = rot;

        // 자동 초기화 호출
        IPoolable poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
        {
            poolable.OnSpawn();
        }

        pool.Enqueue(obj);

        return obj;
    }
}