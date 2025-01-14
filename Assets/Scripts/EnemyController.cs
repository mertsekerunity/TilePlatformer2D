using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;

    Rigidbody2D rb2d;
    CapsuleCollider2D capsuleCollider;
    Animator animator;
    PlayerController player;

    float attackAnimationDelay = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = new Vector2(moveSpeed, 0);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipSkeletonSprite();
    }

    void FlipSkeletonSprite()
    {
        //transform.localScale = new Vector2(-(Mathf.Sign(rb2d.velocity.x)), 1);
        transform.localScale = new Vector2((-Mathf.Sign(rb2d.velocity.x) * Mathf.Abs(transform.localScale.x)), transform.localScale.y);
    }

    void Attack()
    {
        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            if((transform.position.x - player.transform.position.x) < 0)
            {
                if (Mathf.Sign(rb2d.velocity.x) < 0)
                {
                    FlipSkeletonSprite();
                }
            }
            else
            {
                if (Mathf.Sign(rb2d.velocity.x) > 0)
                {
                    FlipSkeletonSprite();
                }
            }
            animator.SetTrigger("Attack");
        }

    }
}
