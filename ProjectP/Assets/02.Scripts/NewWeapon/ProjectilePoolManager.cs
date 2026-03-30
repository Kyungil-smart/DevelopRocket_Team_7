using System.Collections.Generic;
using UnityEngine;


namespace NewWeaponSystem
{
    public struct ProjectileSpwanMsg
    {
        public Vector2 startPos;
        public Vector2 direction;
    }
    
    public class ProjectilePoolManager : MonoBehaviour
    {
        [SerializeField] [Range (20, 50)] private int createStep = 20;
        private Queue<GameObject> _pool = new();
        [SerializeField] private GameObject _prefab;

        private void Start()
        {
            Init();
        }
        
        private void OnEnable()
        {
            PostManager.Instance.Subscribe<ProjectileSpwanMsg>(PostMessageKey.ProjectileSpawned, Spawn);
            PostManager.Instance.Subscribe<GameObject>(PostMessageKey.ProjectileDespawned, Despawn);
        }

        private void OnDisable()
        {
            PostManager.Instance.Unsubscribe<ProjectileSpwanMsg>(PostMessageKey.ProjectileSpawned, Spawn);
            PostManager.Instance.Unsubscribe<GameObject>(PostMessageKey.ProjectileDespawned, Despawn);
        }
        
        public void Init()
        {   
            for (int i = 0; i < createStep; i++)
            {
                _pool.Enqueue(InstantiateProjectile(_prefab));
            }
        }

        private GameObject InstantiateProjectile(GameObject projectilePrefab)
        {
            GameObject obj = Instantiate(projectilePrefab, transform, true);
            obj.name = projectilePrefab.name;
            obj.SetActive(false);
            return obj;
        }

        private void Spawn(ProjectileSpwanMsg spawnMsg)
        {   
            if (_pool.Count <= 1)
            {
                for (int i = 0; i < createStep; i++)
                {
                    _pool.Enqueue(InstantiateProjectile(_prefab));
                }
            }
            GameObject obj = _pool.Dequeue();
            obj.transform.position = spawnMsg.startPos;
            StraightMovement sm = obj.GetComponent<StraightMovement>();
            sm.SetDirection(spawnMsg.direction);
            obj.SetActive(true);
            sm.Fire();
        }

        private void Despawn(GameObject obj)
        {
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}

