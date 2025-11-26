using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    [Header("Basic Info")]
    public string playerName;
    public PlayerStatus status;
    
    [Header("Skill Settings")]
    public ActiveSkill initialSkill; 
    
    public List<SkillInstance> skillInstances = new List<SkillInstance>();
    
    private SkillInstance fireBallInstance;

    [Header("Linked UI")]
    public HealthBar healthBar;

    [Header("Inventory")]
    public List<Item> inventory = new List<Item>();
    public List<StatusEffect> activeEffects = new List<StatusEffect>();

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private bool isDead = false;
    public Vector2 lastMoveDirection = Vector2.right;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();     
        sr = GetComponent<SpriteRenderer>();
        status = new PlayerStatus(100, 50);
    }

    void Start()
    {
        if (status.maxHp <= 0) status.maxHp = 100f;
        status.hp = status.maxHp;
        
        if (healthBar != null) 
            healthBar.UpdateBar();
        
        if (initialSkill != null)
        {
            UnlockSkill(initialSkill);
        }
    }
    
    public void UnlockSkill(ActiveSkill newSkill)
    {
        if (newSkill == null) return;
        
        foreach(var s in skillInstances)
        {
            if(s.skillAsset == newSkill) return; 
        }
        
        SkillInstance newInstance = new SkillInstance(newSkill);
        skillInstances.Add(newInstance);
        
        if (newSkill is FireBallSkill)
        {
            fireBallInstance = newInstance;
            Debug.Log("Player unlocked Fireball!");
        }
    }

    void Update()
    {
        float moveH = Input.GetAxisRaw("Horizontal");
        float moveV = Input.GetAxisRaw("Vertical");
        
        if (moveH != 0 || moveV != 0)
        {
            if (moveH != 0)
                lastMoveDirection = new Vector2(moveH, 0).normalized;
            else
                lastMoveDirection = new Vector2(0, moveV).normalized;
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (fireBallInstance != null && fireBallInstance.CanUse())
            {
                fireBallInstance.Use(this);
            }
        }
    }
    
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        status.TakeDamage(amount);

        if (healthBar != null)
            healthBar.UpdateBar();
            
        StartCoroutine(FlashRed());
        anim.SetTrigger("Hurt");

        if (status.hp <= 0) Die();
    }

    IEnumerator FlashRed()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        sr.color = Color.white;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        anim.SetBool("isDead", true);
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
    }
    
    public void AddHp(float amount)
    {
        if (isDead) return;
        status.Heal(amount);
        if (healthBar != null) healthBar.UpdateBar();
    }

    public void ApplyStatus(StatusEffect effect)
    {
        effect.Start();
        activeEffects.Add(effect);
    }
}