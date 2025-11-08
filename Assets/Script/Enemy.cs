using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
    public int enemyID;
    public string type;
    public float hp = 50f;
    public float damage = 10f;
    public float speed = 2f;
    public Vector2 position;

    public List<Item> dropTable = new List<Item>(); // simple drops

    protected Transform playerTransform;

    void Start() 
    {
        var player = GameObject.FindWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    protected virtual void Update() 
    {
        PatrolOrChase();
    }

    protected virtual void PatrolOrChase() 
    {
        if (playerTransform != null) 
        {
            var dir = (playerTransform.position - transform.position).normalized;
            transform.position += (Vector3)dir * speed * Time.deltaTime;
        }
    }

    public virtual void Attack(Player player) 
    {
        player.TakeDamage(damage);
    }

    public virtual void TakeDamage(float amount) 
    {
        hp -= amount;
        if (hp <= 0) Die();
    }

    public virtual void Die() 
    {
        DropItem();
        Destroy(gameObject);
    }

    protected virtual void DropItem() 
    {
        // simple: drop first item if exists
        if (dropTable.Count > 0) 
        {
            var itemPrefab = dropTable[Random.Range(0, dropTable.Count)];
            if (itemPrefab != null) 
            {
                Instantiate(itemPrefab.gameObject, transform.position, Quaternion.identity);
            }
        }
    }

    public virtual void ApplyStatus(StatusEffect se) 
    {
        // for enemies, you can store active effects similarly to player
    }
}