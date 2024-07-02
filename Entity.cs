using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;


    protected int facingDir = 1;
    protected bool isFacingRight = true;


    [Header("Collision")]
    [SerializeField] protected LayerMask groundMask;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [Space]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    protected bool isGrounded;
    protected bool isWalled;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }


    protected virtual void Update()
    {
        // CollisionCheck();
    }
    
    
    protected virtual void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundMask);
        isWalled = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance * facingDir, groundMask);
    }


    protected virtual void Flip()
    {
        facingDir *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }


    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
    }
}
