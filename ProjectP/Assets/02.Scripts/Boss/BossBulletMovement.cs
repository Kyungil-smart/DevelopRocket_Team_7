using System.Collections;
using UnityEngine;

public class BossBulletMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] [Range(1, 10)] private float Speed;
    [SerializeField] private LayerMask _layerMask;
    private bool isMoving;
    private Vector2 direction;
    private Coroutine aliveRoutine;

    // ToDo. Player 에게 데미지 추가 필요
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void FixedUpdate()
    {
        if (isMoving) Movement();
    }

    private void OnEnable()
    {
        if (aliveRoutine == null) aliveRoutine = StartCoroutine(AliveRoutine());
    }

    private void OnDisable()
    {
        if (aliveRoutine != null) StopCoroutine(aliveRoutine);
        aliveRoutine = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & _layerMask) != 0)
            PostManager.Instance.Post<GameObject>(PostMessageKey.BossBulletDespawned, gameObject);
    }
    
    private void Movement()
    {
        rb.linearVelocity = direction.normalized * Speed;
    }
    
    public void OnMove(Vector2 startPos, Vector2 dir)
    {
        transform.position = startPos;
        isMoving = true;
        direction = dir;
    }

    private IEnumerator AliveRoutine()
    {
        yield return new WaitForSeconds(5);
        PostManager.Instance.Post<GameObject>(PostMessageKey.BossBulletDespawned, gameObject);
    }
    
}