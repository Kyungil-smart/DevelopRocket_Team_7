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

            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectDict[prefab.name].Enqueue(obj);
        }
    }
    
    public GameObject Spawn(string name, Vector2 position)
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
