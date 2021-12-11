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
    public float attackSpeed;
    public float helmBreakerImpactSpeed;
    public float stingerSpeed;
    public float chargeTimeDelay;
    private float chargeTimeClock;

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
    public DanteAttack hitbox;
    private Ball ballReference;

    protected override void Start()
    {
        base.Start();
        hitbox.OnBallEnterTrigger += OnBallEnter;
        hitbox.OnBallExitTrigger += OnBallExit;
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
        ballReference = null;
        chargeTimeClock = 0f;

        if (!isPlayer1)
        {
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        defaultGravity = rig.gravityScale;
    }

    protected override void Update()
    {
        chargeClock += Time.deltaTime;
        isTryingToJump = (moveSpeed > 0f && isPlayer1) || (moveSpeed < 0f && !isPlayer1);

        if (!isTryingToJump && moveSpeed != 0f && !CheckIfGrounded() && !performingHelmBreaker)
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
            performingStinger = false;
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
            if (!performingStinger && !performingHelmBreaker && !isTryingToJump)
            {
                rig.velocity = Vector2.zero;
                rig.gravityScale = 0f;
                if (ballReference != null)
                    ballReference.BeginDanteAttack();
            }
            if (attackClock >= attackTime || performingStinger || performingHelmBreaker || isTryingToJump)
            {
                isAttacking = false;
                attackClock = 0f;
                rig.gravityScale = defaultGravity;
                PerformAttack();
                currentAttack = 0;
                if (power && !canAttack)
                {
                    power = false;
                    charges -= chargesRequired;
                    if(UpdateCharges!=null)
                        UpdateCharges.Invoke(charges);
                    PerformStinger();
                    attackClock = 0f;
                    performingStinger = true;
                }
            }
        }

        if (performingStinger)
        {
            attackClock += Time.deltaTime;
            if (attackClock > attackTime)
            {
                performingStinger = false;
            }
        }

        if (performingHelmBreaker)
        {
            PerformHelmBreaker();
        }

        if (CheckIfGrounded())
        {
            animator.SetTrigger("IDLE");
            remainingJumps = maxJumps;
            if (performingHelmBreaker)
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

    private void PerformAttack()
    {
        if (currentAttack <= 3 && currentAttack > 0 && ballReference != null)
        {
            Vector2 attackDirection = Vector2.zero;
            switch (currentAttack)
            {
                case 1:
                    attackDirection = new Vector2(1f, 1f);
                    break;

                case 2:
                    attackDirection = new Vector2(0.5f, 0.7f);
                    break;

                case 3:
                    attackDirection = new Vector2(0.5f, 0.2f);
                    break;
            }
            attackDirection.Normalize();
            if (!isPlayer1)
                attackDirection.x = -attackDirection.x;
            if (transform.position.y > ballReference.transform.position.y)
                attackDirection.y = -attackDirection.y;

            ballReference.FinishDanteAttack(attackDirection, attackSpeed, false);
            Charge();
        }
    }

    private void PerformStinger()
    {
        if (ballReference != null)
        {
            animator.SetTrigger("STINGER");
            Vector2 attackDirection = new Vector2(0.1f, 0f);
            attackDirection.Normalize();
            if (!isPlayer1)
                attackDirection.x = -attackDirection.x;

            ballReference.FinishDanteAttack(attackDirection, stingerSpeed, true);
        }
    }

    private void PerformHelmBreaker()
    {
        if (ballReference != null)
        {
            Vector2 attackDirection = new Vector2(0.2f, 0.7f);
            attackDirection.Normalize();
            if (!isPlayer1)
                attackDirection.x = -attackDirection.x;

            ballReference.FinishDanteAttack(attackDirection, helmBreakerImpactSpeed, false);
        }
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

    private void OnBallEnter(GameObject ball)
    {
        ballReference = ball.GetComponent<Ball>();
    }

    private void OnBallExit(GameObject ball)
    {
        ballReference = null;
    }
}