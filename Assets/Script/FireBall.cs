using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour, IPooledObject
{
    public float speed = 10f;
    public float damage = 20f;
    public float lifeTime = 2f;
    private Vector2 direction;
    
    public void OnObjectSpawn()
    {
        
    }

    public void Setup(Vector2 dir)
    {
        direction = dir.normalized;
        
        if (dir.x < 0) transform.localScale = new Vector3(-1, 1, 1);
        else transform.localScale = new Vector3(1, 1, 1);
        
        StopAllCoroutines();
        StartCoroutine(DisableAfterTime());
    }

    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            gameObject.SetActive(false);
        }
        
        if (collision.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
    }
}