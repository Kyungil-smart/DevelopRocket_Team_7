using System.Collections.Generic;
using UnityEngine;


namespace NewWeaponSystem
{
    public struct ProjectileSpwanMsg
    {
        public Vector2 startPos;
        public List<Vector2> direction;
        public WeaponBlackboard blackboard;
    }

    public struct SelectProjectileMsg
    {
        public GameObject projectilePrefab;
    }
    
    public class ProjectilePoolManager : MonoBehaviour
    {
        [SerializeField] [Range (20, 50)] private int createStep = 20;
        private Queue<GameObject> _pool = new();
        [SerializeField] private GameObject _prefab;
        
        private void OnEnable()
        {
            PostManager.Instance.Subscribe<ProjectileSpwanMsg>(PostMessageKey.ProjectileSpawned, Spawn);
            PostManager.Instance.Subscribe<GameObject>(PostMessageKey.ProjectileDespawned, Despawn);
            PostManager.Instance.Subscribe<SelectProjectileMsg>(PostMessageKey.ProjectileSelection, SetUp);
        }

        private void OnDisable()
        {
            PostManager.Instance.Unsubscribe<ProjectileSpwanMsg>(PostMessageKey.ProjectileSpawned, Spawn);
            PostManager.Instance.Unsubscribe<GameObject>(PostMessageKey.ProjectileDespawned, Despawn);
            PostManager.Instance.Unsubscribe<SelectProjectileMsg>(PostMessageKey.ProjectileSelection, SetUp);
        }
        
        private void SetUp(SelectProjectileMsg data)
        {
            _prefab = data.projectilePrefab;
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
            foreach (var direction in spawnMsg.direction)
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
                BulletMovement bm = obj.GetComponent<BulletMovement>();
                bm.SetDirection(direction);
                if (spawnMsg.blackboard.origin.WeaponType == WeaponType.Sniper)
                    obj.GetComponent<SniperProjectile>().SetUpData(spawnMsg.blackboard);
                else
                    obj.GetComponent<BulletProjectile>().SetUpData(spawnMsg.blackboard);
                obj.SetActive(true);
                bm.Fire();    
            }
        }

        private void Despawn(GameObject obj)
        {
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}

