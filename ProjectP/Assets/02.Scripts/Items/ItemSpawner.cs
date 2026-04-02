using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{  
    [SerializeField] private GameObject _batteryPrefab;
    
    private Queue<GameObject> _batteryQueue = new();

    private void Start()
    {
        Init();
    }
    
    private void OnEnable()
    {
        PostManager.Instance.Subscribe<Vector2>(PostMessageKey.BatterySpawned, Spwan);
        PostManager.Instance.Subscribe<GameObject>(PostMessageKey.BatteryDespawned, Despawn);
    }

    private void OnDisable()
    {
        PostManager.Instance.Unsubscribe<Vector2>(PostMessageKey.BatterySpawned, Spwan);
        PostManager.Instance.Unsubscribe<GameObject>(PostMessageKey.BatteryDespawned, Despawn);
    }
    
    private void Init()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject obj = Instantiate(_batteryPrefab, gameObject.transform, true);
            obj.name = $"{_batteryPrefab.name}";
            obj.SetActive(false);
            _batteryQueue.Enqueue(obj);
        }
    }
    
    public void Spwan(Vector2 position)
    {
        GameObject obj = _batteryQueue.Dequeue();
        obj.transform.position = position;
        obj.SetActive(true);
    }

    public void Despawn(GameObject obj)
    {
        obj.SetActive(false);
        _batteryQueue.Enqueue(obj);
    }

    [ContextMenu("Test/Spwan")]
    public void OnTestSpwan()
    {
        Vector2 position = Vector2.zero;
        Spwan(position);
    }
}
