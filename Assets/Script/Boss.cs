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
    }

    protected override void Update() 
    {
        if (isDead || (isUsingSkill && !isCharging)) return;
        CheckPhase();
        
        if (playerTransform != null)
        {
            BossAI();
        }
        
        if (isCharging)
        {
            float moveDir = facingRight ? -1f : 1f; 
            rb.velocity = new Vector2(moveDir * chargeSpeed, rb.velocity.y);
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
        
        LookAtPlayer();
        if (Time.time < globalSkillReadyTime)
        {
            MoveTowardsPlayer();
            return;
        }
        
        if (dist > attackRange && dist <= 12f)
        {
            if (Time.time >= nextFireBallTime)
            {
                StartCoroutine(PerformFireBall());
                return;
            }
        }
        
        if (dist <= attackRange * 2.5f) 
        {
            anim.SetBool("isWalking", false); 

            int rand = Random.Range(0, 100); 
            
            if (rand < 30 && Time.time >= nextHeavyAttackTime && dist <= attackRange)
            {
                StartCoroutine(PerformHeavyAttack());
            }
            
            else if (rand < 60 && Time.time >= nextChargeAttackTime)
            {
                StartCoroutine(PerformChargeAttack());
            }
            
            else if (Time.time - lastAttackTime >= attackCooldown && dist <= attackRange)
            {
                PerformNormalAttack();
            }
            else
            {
                MoveTowardsPlayer(); 
            }
            return;
        }

        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        if (isUsingSkill) return; 
        
        float dist = Vector2.Distance(transform.position, playerTransform.position);
        if (dist > attackRange)
        {
            anim.SetBool("isWalking", true); 
            Vector2 dir = (playerTransform.position - transform.position).normalized;
            rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    void PerformNormalAttack()
    {
        Debug.Log("Boss: Normal Slash");
        isUsingSkill = true;
        
        string triggerName = (Random.value > 0.5f) ? "Attack1" : "Attack2";
        anim.SetTrigger(triggerName); 
        
        lastAttackTime = Time.time;
        
        var p = playerTransform.GetComponent<Player>();
        if(p != null) Attack(p); 
        
        StartCoroutine(FinishSkill(0.6f));
    }

    IEnumerator PerformHeavyAttack()
    {
        Debug.Log("Boss: Heavy Slash!");
        isUsingSkill = true;
        anim.SetTrigger("AttackHeavy"); 
        
        yield return new WaitForSeconds(0.6f);
        
        float dist = Vector2.Distance(transform.position, playerTransform.position);
        if (dist <= attackRange * 1.5f) 
        {
            var p = playerTransform.GetComponent<Player>();
            if (p != null) p.TakeDamage(damage * heavyDmgMult);
        }

        nextHeavyAttackTime = Time.time + heavyAttackCooldown;
        StartCoroutine(FinishSkill(1f));
    }

    IEnumerator PerformChargeAttack()
    {
        Debug.Log("Boss: CHARGE!");
        isUsingSkill = true;
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
        Debug.Log("1. Boss กำลังจะยิง (เข้าเงื่อนไขแล้ว)");
        isUsingSkill = true;
        anim.SetTrigger("CastFireball"); 

        yield return new WaitForSeconds(0.4f);

        if (fireBallPrefab != null)
        {
            Debug.Log("2. สร้างลูกไฟแล้ว!");
            GameObject fb = Instantiate(fireBallPrefab, castPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("ยังไม่ได้ใส่ FireBall Prefab ใน Inspector!!!");
        }

        nextFireBallTime = Time.time + fireBallCooldown;
        StartCoroutine(FinishSkill(0.8f));
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCharging && collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            if (p != null)
            {
                p.TakeDamage(damage * chargeDmgMult);
                Rigidbody2D playerRb = p.GetComponent<Rigidbody2D>();
                if(playerRb)
                {
                    Vector2 knockDir = (p.transform.position - transform.position).normalized;
                    playerRb.AddForce(knockDir * 300f);
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
        SetGlobalCooldown(skillGlobalCooldown);
        anim.SetBool("isWalking", true);
    }
}