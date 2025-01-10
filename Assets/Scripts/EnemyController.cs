using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;

    Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
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
        transform.localScale = new Vector2(-(Mathf.Sign(rb2d.velocity.x)), 1);
    }
}
