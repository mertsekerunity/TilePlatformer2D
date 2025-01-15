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

    float attackAnimationDelay = 0.8f;
    bool isAttacking =false;
    bool isFlippedDuringAttack =false;

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
        FlipSkeletonSpriteForWalking();
    }

    void FlipSkeletonSpriteForWalking()
    {
        transform.localScale = new Vector2((-Mathf.Sign(rb2d.velocity.x) * Mathf.Abs(transform.localScale.x)), transform.localScale.y);
    }

    void FlipSkeletonSpriteForAttacking()
    {
        if (player.moveInput[0] != 0)
        {
            if (player.moveInput[0] == Mathf.Sign(rb2d.velocity.x))
            {
                transform.localScale = new Vector2((-Mathf.Sign(rb2d.velocity.x) * Mathf.Abs(transform.localScale.x)), transform.localScale.y);
                isFlippedDuringAttack = true;
                Debug.Log("Skeleton is flipped during attack!");
            }
        }
        else
        {
            if (player.lastMoveInput[0] == Mathf.Sign(rb2d.velocity.x))
            {
                transform.localScale = new Vector2((-Mathf.Sign(rb2d.velocity.x) * Mathf.Abs(transform.localScale.x)), transform.localScale.y);
                isFlippedDuringAttack = true;
                Debug.Log("Skeleton is flipped during attack!, player is stopped");
            }
        }
    }

    void Attack()
    {

        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Player")))

        {
            animator.SetBool("isWalking", false);

            FlipSkeletonSpriteForAttacking();

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
            if (isFlippedDuringAttack)
            {
                isFlippedDuringAttack = false;
                FlipSkeletonSpriteForWalking();
            }
        }
    }

    IEnumerator ApplyAttackAnimation()
    {
        yield return new WaitForSecondsRealtime(attackAnimationDelay);
        isAttacking = false;

    }
}
