using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante : Palette
{
    private float builtUpAcceleration;
    private int remainingJumps;
    public int maxJumps;

    public float attackTime;
    private float attackClock;
    private float defaultGravity;

    public float helmBreakerSpeed;

    private bool canJump;
    private bool isTryingToJump;
    private bool isAttacking;
    private bool canAttack;
    private bool performingStinger;

    private bool performingHelmBreaker;
    private int currentAttack;
    public float groundCheckDistance;


    private Rigidbody2D rig;
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        builtUpAcceleration = 0f;
        canJump = true;
        remainingJumps = 0;
        rig = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isAttacking = false;
        canAttack = false;
        currentAttack = 0;
        attackClock = 0f;
        performingStinger = false;
        performingHelmBreaker = false;
        if (!isPlayer1)
        {
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        defaultGravity = rig.gravityScale;
    }

    protected override void Update()
    {

        isTryingToJump = (moveSpeed > 0f && isPlayer1) || (moveSpeed < 0f && !isPlayer1);
        
        if(!isTryingToJump && moveSpeed != 0f && !CheckIfGrounded() && !performingHelmBreaker)
        {
            rig.velocity = Vector2.zero;
            rig.AddForce(Vector2.down * helmBreakerSpeed, ForceMode2D.Impulse);
            animator.SetTrigger("HELM BREAKER");
            performingHelmBreaker = true;
        }

        if (canJump && remainingJumps > 0 && isTryingToJump && !isAttacking)
        {
            builtUpAcceleration = speed;
            remainingJumps -= 1;
            canJump = false;
            rig.velocity = Vector2.zero;
            rig.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
            animator.SetTrigger("JUMP");
            performingHelmBreaker = false;
        }
        else
        if (moveSpeed == 0f)
            canJump = true;

        if (action)
        {
            if (canAttack && moveSpeed == 0f && !performingStinger && !performingHelmBreaker)
            {
                currentAttack++;
                if (ComboAttack(currentAttack))
                {
                    canAttack = false;
                    isAttacking = true;
                    attackClock = 0f;
                }
            }
        }
        else
        {
            canAttack = true;
        }

        if (isAttacking)
        {
            attackClock += Time.deltaTime;
            if(!performingStinger && !performingHelmBreaker && !isTryingToJump)
            {
                rig.velocity = Vector2.zero;
                rig.gravityScale = 0f;
            }
            if (attackClock >= attackTime || performingStinger || performingHelmBreaker || isTryingToJump)
            {
                isAttacking = false;
                currentAttack = 0;
                attackClock = 0f;
                rig.gravityScale = defaultGravity;
            }
        }

        if (CheckIfGrounded())
        {
            animator.SetTrigger("IDLE");
            remainingJumps = maxJumps;
            if(performingHelmBreaker)
                performingHelmBreaker = false;
        }
        else
        {
            animator.ResetTrigger("IDLE");
        }
    }

    private bool ComboAttack(int attack)
    {
        if (attack <= 3 && attack > 0)
        {
            animator.SetTrigger("COMBO" + attack);
            return true;
        }
        return false;
    }

    private bool CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, (sprite.bounds.extents.y / 2) + groundCheckDistance, raycastMask);
        Debug.DrawRay(transform.position, Vector3.down * (sprite.bounds.extents.y / 2 + groundCheckDistance), Color.red, 0.1f);
        if (hit)
        {
            return true;
        }
        else
            return false;
    }
}