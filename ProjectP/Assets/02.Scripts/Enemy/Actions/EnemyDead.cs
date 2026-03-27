using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    // ToDo. 주로 사망 애니메이션 및 소리등이 들어갈 것으로 예상됨.

    public void Dead()
    {
        string name = gameObject.name.Split('_')[0];
        EnemyDespawnMsg msg = new EnemyDespawnMsg()
        {
            name = name,
            obj = gameObject
        };
        PostManager.Instance.Post<EnemyDespawnMsg>(PostMessageKey.EnemyDespawned, msg);
    }
}
