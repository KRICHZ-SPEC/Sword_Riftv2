using UnityEngine;
using System.Collections;

public class Boss : Enemy 
{
    [Header("Boss Identity")]
    public string bossName = "NightBorne";
    public int phase = 1;
    public float enragedThreshold = 0.4f; 

    [Header("Skill Settings")]
    public GameObject fireBallPrefab;       
    public Transform castPoint;             
    public float chargeSpeed = 12f;         
    public float chargeDuration = 0.5f;

    [Header("Attack Settings (Hitbox)")]
    public Transform attackPoint;           
    public float attackRadius = 1.5f;       
    public LayerMask playerLayer;          

    [Header("Combat Config (Cooldowns)")]
    public float skillGlobalCooldown = 1.5f; 
    public float heavyAttackCooldown = 6f;
    public float chargeAttackCooldown = 9f;
    public float fireBallCooldown = 5f;
    
    [Header("Damage Multipliers")]
    public float heavyDmgMult = 1.5f;
    public float chargeDmgMult = 1.2f;
    
    private float nextHeavyAttackTime;
    private float nextChargeAttackTime;
    private float nextFireBallTime;
    private float globalSkillReadyTime;
    
    private bool isUsingSkill = false;
    private bool isCharging = false; 
    
    protected override void Start()
    {
        base.Start();
        if (castPoint == null) castPoint = transform;
        if (attackPoint == null) attackPoint = transform;
    }

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        phase = 1;
        isUsingSkill = false;
        isCharging = false;
        if(sr != null) sr.color = Color.white; 
    }

    protected override void Update() 
    {
        if (isDead) 
        {
            rb.velocity = Vector2.zero;
            return;
        }
        
        if (isUsingSkill && !isCharging) 
        {
            currentMovementInput = Vector2.zero;
            return;
        }

        CheckPhase();
        
        if (playerTransform != null)
        {
            BossAI();
        }
    }

    protected override void FixedUpdate()
    {
        if (isDead) return;
        if (isCharging)
        {
            float moveDir = facingRight ? -1f : 1f; 
            rb.velocity = new Vector2(moveDir * chargeSpeed, rb.velocity.y);
        }
        else
        {
            base.FixedUpdate();
        }
    }

    void CheckPhase() 
    {
        if (maxHp <= 0) return;
        float hpPercent = hp / maxHp; 
        
        if (hpPercent <= enragedThreshold && phase == 1) 
        {
            phase = 2;
            Debug.Log(bossName + " : ENRAGED MODE!");
            if(sr != null) sr.color = new Color(1f, 0.5f, 1f); 
            speed *= 1.4f; 
            attackCooldown *= 0.7f; 
        }
    }

    void BossAI()
    {
        if (isCharging) return;
        float dist = Vector2.Distance(transform.position, playerTransform.position);
        
        if (!isUsingSkill || Time.time >= nextChargeAttackTime) 
        {
            LookAtPlayer();
        }

        if (Time.time < globalSkillReadyTime)
        {
            MoveTowardsPlayer();
            return;
        }
        
        if (dist <= 12f) 
        {
            anim.SetBool("isWalking", false);
            bool canHeavy = (Time.time >= nextHeavyAttackTime && dist <= attackRange * 1.5f);
            bool canCharge = (Time.time >= nextChargeAttackTime && dist <= attackRange * 3f);
            bool canFireball = (Time.time >= nextFireBallTime);
            int rand = Random.Range(0, 100);

            if (canHeavy && rand < 40) 
            {
                StartCoroutine(PerformHeavyAttack());
            }
            else if (canCharge && rand < 70) 
            {
                StartCoroutine(PerformChargeAttack());
            }
            else if (canFireball && rand < 90) 
            {
                StartCoroutine(PerformFireBall());
            }
            else if (dist <= attackRange && Time.time - lastAttackTime >= attackCooldown)
            { 
                StartCoroutine(PerformNormalAttack());
            }
            else
            {
                MoveTowardsPlayer(); 
            }
        }
        else
        {
            MoveTowardsPlayer();
        }
    }
    
    void MoveTowardsPlayer()
    {
        if (isUsingSkill) 
        {
            currentMovementInput = Vector2.zero;
            return; 
        }
        
        float dist = Vector2.Distance(transform.position, playerTransform.position);
        if (dist > attackRange)
        {
            anim.SetBool("isWalking", true); 
            currentMovementInput = (playerTransform.position - transform.position).normalized;
        }
        else
        {
            anim.SetBool("isWalking", false);
            currentMovementInput = Vector2.zero;
        }
    }

    IEnumerator PerformNormalAttack()
    {
        isUsingSkill = true;
        currentMovementInput = Vector2.zero; 
        string triggerName = (Random.value > 0.5f) ? "Attack1" : "Attack2";
        anim.SetTrigger(triggerName); 
        lastAttackTime = Time.time;
        yield return new WaitForSeconds(0.4f);
        CheckHitbox(damage);
        StartCoroutine(FinishSkill(0.6f));
    }

    IEnumerator PerformHeavyAttack()
    {
        isUsingSkill = true;
        currentMovementInput = Vector2.zero;
        anim.SetTrigger("AttackHeavy"); 
        yield return new WaitForSeconds(0.6f);
        CheckHitbox(damage * heavyDmgMult, attackRadius * 1.5f);
        nextHeavyAttackTime = Time.time + heavyAttackCooldown;
        StartCoroutine(FinishSkill(1f));
    }
    
    void CheckHitbox(float dmgAmount, float radiusScale = 1f)
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius * radiusScale, playerLayer);

        foreach(Collider2D playerHit in hitPlayers)
        {
            if (playerHit.CompareTag("Player"))
            {
                if(playerHit.TryGetComponent(out Player p))
                {
                    p.TakeDamage(dmgAmount);
                    Debug.Log("Boss Hit Player! Damage: " + dmgAmount);
                }
            }
        }
    }

    IEnumerator PerformChargeAttack()
    {
        isUsingSkill = true;
        currentMovementInput = Vector2.zero;
        anim.SetTrigger("AttackCharge"); 
        
        yield return new WaitForSeconds(0.4f);
        isCharging = true; 
        yield return new WaitForSeconds(chargeDuration);
        isCharging = false;
        rb.velocity = Vector2.zero;
        nextChargeAttackTime = Time.time + chargeAttackCooldown;
        StartCoroutine(FinishSkill(0.5f));
    }
    
    IEnumerator PerformFireBall()
    {
        Debug.Log("Boss: Cast Fireball!");
        isUsingSkill = true;
        currentMovementInput = Vector2.zero;
        anim.SetBool("isWalking", false);
        rb.velocity = Vector2.zero;
        LookAtPlayer();
        anim.SetTrigger("CastFireball"); 
        yield return new WaitForSeconds(0.5f);

        if (fireBallPrefab != null)
        {
            GameObject fireball = Instantiate(fireBallPrefab, castPoint.position, Quaternion.identity);
            
            float direction = transform.localScale.x > 0 ? -1f : 1f;
            
            Vector2 launchVelocity = new Vector2(direction * 15f, 0);
            
            Vector3 fbScale = fireball.transform.localScale;
            fbScale.x = Mathf.Abs(fbScale.x) * direction; 
            fireball.transform.localScale = fbScale;
            
            BossProjectile projectileScript = fireball.GetComponent<BossProjectile>();
            if (projectileScript != null)
            {
                projectileScript.Setup(launchVelocity, damage, GetComponent<Collider2D>());
            }
        }
        else
        {
            Debug.LogError("Boss ไม่มี FireBall Prefab!");
        }

        nextFireBallTime = Time.time + fireBallCooldown;
        StartCoroutine(FinishSkill(0.8f));
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCharging && collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent(out Player p))
            {
                p.TakeDamage(damage * chargeDmgMult);
                if(collision.gameObject.TryGetComponent(out Rigidbody2D playerRb))
                {
                    Vector2 knockDir = (p.transform.position - transform.position).normalized;
                    playerRb.AddForce(knockDir * 15f, ForceMode2D.Impulse);
                    
                    if (p.TryGetComponent(out PlayerController2D pc)) 
                        StartCoroutine(pc.DisableMovement(0.3f));
                }
            }
            isCharging = false; 
            rb.velocity = Vector2.zero;
        }
    }

    void SetGlobalCooldown(float time)
    {
        globalSkillReadyTime = Time.time + time;
    }

    IEnumerator FinishSkill(float delay)
    {
        yield return new WaitForSeconds(delay);
        isUsingSkill = false;
        isCharging = false;
        rb.velocity = Vector2.zero;
        currentMovementInput = Vector2.zero;
        SetGlobalCooldown(skillGlobalCooldown);
        anim.SetBool("isWalking", true);
    }
    
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }
}