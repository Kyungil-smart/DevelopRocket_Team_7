using System.Collections.Generic;
using UnityEngine;

public struct ProjectileSpwanMsg
{
    public string name;
    public Vector2 pos;
    public Quaternion rot;
}

namespace NewWeaponSystem
{
    public class ProjectilePoolManager : MonoBehaviour
    {
        [SerializeField] [Range (20, 50)] private int createStep = 20;
        private Queue<GameObject> _pool = new();
        private GameObject _prefab;

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
        
        public void Init(WeaponBlackboard blackboard)
        {   
            _prefab = blackboard.origin.projectilePrefab;
            for (int i = 0; i < createStep; i++)
            {
                _pool.Enqueue(InstantiateProjectile(_prefab));
            }
        }

        private GameObject InstantiateProjectile(GameObject projectilePrefab)
        {
            GameObject obj = Instantiate(projectilePrefab, transform, true);
            obj.name = projectilePrefab.name;
            return obj;
        }

        private GameObject Spawn(ProjectileSpwanMsg spawnMsg)
        {   
            if (_pool.Count <= 1)
            {
                for (int i = 0; i < createStep; i++)
                {
                    _pool.Enqueue(InstantiateProjectile(_prefab));
                }
            }
            GameObject obj = _pool.Dequeue();
            obj.transform.position = spawnMsg.pos;
            obj.transform.rotation = spawnMsg.rot;
            obj.SetActive(true);
            return obj;
        }

        private void Despawn(GameObject obj)
        {
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}

