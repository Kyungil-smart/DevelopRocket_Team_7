using System.Collections.Generic;
using UnityEngine;


public class ProjectilePoolManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> projectilePrefabs;
    [SerializeField] [Range (20, 50)] private int createStep = 20;
    private Dictionary<string, Queue<GameObject>> projectilePool = new();

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        PostManager.Instance.Subscribe<ProjectileSpwanMsg, GameObject>(PostMessageKey.ProjectileSpawned, Spawn);
        PostManager.Instance.Subscribe<GameObject>(PostMessageKey.ProjectileSpawned, Despawn);
    }

    private void OnDisable()
    {
        PostManager.Instance.Unsubscribe<ProjectileSpwanMsg, GameObject>(PostMessageKey.ProjectileSpawned, Spawn);
        PostManager.Instance.Unsubscribe<GameObject>(PostMessageKey.ProjectileSpawned, Despawn);
    }
    
    private void Init()
    {
        foreach (GameObject projectilePrefab in projectilePrefabs)
        {
            // 초기 개수 미리 생산
            for (int i = 0; i < createStep; i++)
            {
                if (!projectilePool.ContainsKey(projectilePrefab.name))
                    projectilePool.Add(projectilePrefab.name, new Queue<GameObject>());
                projectilePool[projectilePrefab.name].Enqueue(InstantiateProjectile(projectilePrefab));
            }
        }
    }

    private GameObject GetPrefab(string name)
    {
        foreach(var proj in projectilePrefabs)
        {
            if (proj.name == name)
                return proj.gameObject;
        }
        return null;
    }

    private GameObject InstantiateProjectile(GameObject projectilePrefab)
    {
        GameObject obj = Instantiate(projectilePrefab, transform, true);
        obj.name = projectilePrefab.name;
        return obj;
    }

    private GameObject Spawn(ProjectileSpwanMsg spawnMsg)
    {
        if (projectilePool.TryGetValue(spawnMsg.name, out Queue<GameObject> proj))
        {
            if (proj.Count <= 1)
            {
                GameObject prefab = GetPrefab(spawnMsg.name);
                for (int i = 0; i < createStep; i++)
                {
                    projectilePool[spawnMsg.name].Enqueue(InstantiateProjectile(prefab));
                }
            }
            GameObject obj = proj.Dequeue();
            obj.transform.position = spawnMsg.pos;
            obj.transform.rotation = spawnMsg.rot;
            obj.SetActive(true);
            return obj;
        }
        Debug.LogError($"{spawnMsg.name} not found");
        return null;
    }

    private void Despawn(GameObject obj)
    {
        obj.SetActive(false);
        projectilePool[obj.name].Enqueue(obj);
    }
}
