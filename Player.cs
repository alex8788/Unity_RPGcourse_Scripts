using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class Player : Entity
{
    [Header("Move")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;


    [Header("Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;  // 衝刺持續時間
    private float dashTimer;  // 衝刺計時器

    [SerializeField] private float dashCoolDown;  // 衝刺冷卻時間
    private float dashCoolDownTimer;  // 衝刺冷卻計時器


    [Header("Attack")]
    [SerializeField] private float comboInterval = .3f;  // combo時間間隔
    private float comboIntervalTimer;  // combo間隔計時器
    private bool isAttacking;

    private int comboCounter;  // 攻擊combo計數器


    private float xInput;


    protected override void Start()
    {
        base.Start();
    }


    protected override void Update()
    {
        base.Update();

        //* 檢測和輸入
        CheckInput();

        //* 計時器
        dashTimer -= Time.deltaTime;
        dashCoolDownTimer -= Time.deltaTime;

        comboIntervalTimer -= Time.deltaTime;
        
        //* 執行函式
        Move();
        FlipController();
        AnimatorController();
    }


    private void CheckInput()
    {
        //* move
        xInput = Input.GetAxis("Horizontal");

        //* jump
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        //* dash
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }
        
        //* attack
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }


    private void Attack()
    {
        if (!isGrounded)
            return;

        if (comboIntervalTimer < 0)  // 檢查與重置combo段數
            comboCounter = 0;

        isAttacking = true;
        comboIntervalTimer = comboInterval;  // 重置combo間隔計時器
    }


    private void AnimatorController()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity", rb.velocity.y);

        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isDashing", dashTimer > 0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);
    }


    private void Move()
    {
        if (isAttacking)  // 攻擊
            rb.velocity = new Vector2(0, 0);
        else if (dashTimer > 0)
            rb.velocity = new Vector2(dashSpeed * facingDir, rb.velocity.y);  // 衝刺
        else
            rb.velocity = new Vector2(moveSpeed * xInput, rb.velocity.y);  // 移動
    }


    private void Dash()
    {
        //* 重置衝刺冷卻時間
        if (dashCoolDownTimer < 0 && !isAttacking)
        {
            dashTimer = dashDuration;
            dashCoolDownTimer = dashCoolDown;
        }
    }


    private void Jump()
    {
        if (isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }


    private void FlipController()
    {
        if (rb.velocity.x > 0 && !isFacingRight)
            Flip();
        else if (rb.velocity.x < 0 && isFacingRight)
            Flip();
    }


    public void AttackOver()
    {
        isAttacking = false;

        comboCounter = (comboCounter + 1) % 3;  // 更新combo段數
    }
}
