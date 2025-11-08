using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour 
{
    public string playerName;
    public Vector2 velocity;
    public PlayerStatus status;
    public List<ActiveSkill> skills = new List<ActiveSkill>();
    public List<Item> inventory = new List<Item>();
    public List<StatusEffect> activeEffects = new List<StatusEffect>();

    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        status = new PlayerStatus(100, 50);
    }

    void Update() 
    {
        HandleMovementInput();
        UpdateStatusEffects();
    }

    void HandleMovementInput() 
    {
        float h = Input.GetAxisRaw("Horizontal");
        Vector2 move = new Vector2(h * moveSpeed, rb.velocity.y);
        rb.velocity = move;
    }

    public void Move(Vector2 dir) 
    {
        rb.velocity = dir * moveSpeed;
    }

    public void Attack() 
    {
        // basic melee attack: raycast or spawn hitbox
        Debug.Log("Player attack");
    }

    public void UseSkill(int index) 
    {
        if (index < 0 || index >= skills.Count) return;
        skills[index].Use(this);
    }

    public void TakeDamage(float amount) 
    {
        status.TakeDamage(amount);
        if (status.hp <= 0) 
        {
            Die();
        }
    }

    public void PickupItem(Item item) 
    {
        inventory.Add(item);
        item.OnPickup(this);
    }

    public void ApplyStatus(StatusEffect effect) 
    {
        effect.Start();
        activeEffects.Add(effect);
    }

    void UpdateStatusEffects() 
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--) {
            activeEffects[i].ApplyTo(status);
            activeEffects[i].Update(Time.deltaTime);
            if (activeEffects[i].IsExpired()) 
            {
                activeEffects.RemoveAt(i);
            }
        }
    }

    void Die() 
    {
        Debug.Log("Player died");
        // respawn or game over
    }
}