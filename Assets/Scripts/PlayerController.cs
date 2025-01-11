using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2 (10f, 10f);
    [SerializeField] PhysicsMaterial2D bouncinessMaterial;
    [SerializeField] float deathKickDelay = 2f;

    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator animator;
    BoxCollider2D boxCollider;
    CapsuleCollider2D capsuleCollider;
    bool isAlive;


    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rb2d.bodyType = RigidbodyType2D.Dynamic;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) {  return; } // return is like break in
        Walk();
        FlipSprite();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; } // return is like break in here
        moveInput = value.Get<Vector2>();
        //Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; } // return is like break in here
        if (!boxCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; } // return is like break in here

        if (value.isPressed)
        {
            rb2d.velocity += new Vector2(0, jumpSpeed);
        }
    }

    void Walk()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rb2d.velocity.y);
        rb2d.velocity = playerVelocity;
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isWalking", playerHasHorizontalSpeed);

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2((Mathf.Sign(rb2d.velocity.x) * Mathf.Abs(transform.localScale.x)), transform.localScale.y);
        }
    }

    void Die()
    {
        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("Death");
            rb2d.velocity = deathKick;
            StartCoroutine(ApplyDeathKick());
        }
    }

    IEnumerator ApplyDeathKick() 
    {
        yield return new WaitForSecondsRealtime(deathKickDelay);
        rb2d.bodyType = RigidbodyType2D.Static; //stop sliding after deathKick is applied
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    void SetBounciness()
    {
        if (isAlive)
        {
            bouncinessMaterial.bounciness = 1.2f;
        }
        else 
        {
            bouncinessMaterial.bounciness = 0f;
        }
    }
}
