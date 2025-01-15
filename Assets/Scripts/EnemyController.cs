using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    //[SerializeField] PlayerController player;
    PlayerController player;

    Rigidbody2D rb2d;
    CapsuleCollider2D capsuleCollider;
    Animator animator;

    float attackAnimationDelay = 4f;
    bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = new Vector2(moveSpeed, 0);
        Attack();
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
            animator.SetBool("isWalking", false);

            if ((gameObject.transform.position.x - player.transform.position.x) < 0)
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

            if(!isAttacking)
            {
                isAttacking = true;
                animator.SetTrigger("Attack");
                StartCoroutine(ApplyAttackAnimation());
            }
        }
        else
        {
            animator.SetBool("isWalking", true);
        }
    }

    IEnumerator ApplyAttackAnimation()
    {
        yield return new WaitForSecondsRealtime(attackAnimationDelay);
        isAttacking = false;

    }
}
