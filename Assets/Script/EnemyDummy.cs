using UnityEngine;
using System.Collections; // << เพิ่มบรรทัดนี้ เพื่อใช้ Coroutines

public class EnemyDummy : MonoBehaviour
{
    [Header("Status")]
    public float maxHp = 30f;
    public float currentHp;

    [Header("Drop Settings")]
    public GameObject pickupPrefab;
    public Transform pickupSpawnPoint;

    [Header("Hit Effects")]
    public Color hitColor = Color.red;
    public float flashDuration = 0.15f;
    private Animator anim;
    private SpriteRenderer sr;
    private Coroutine flashCoroutine;

    void Awake()
    {
        currentHp = maxHp;
        
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (sr == null) {
            Debug.LogError("EnemyDummy ไม่มี SpriteRenderer ทำให้กระพริบสีแดงไม่ได้!");
        }
        if (anim == null) {
            Debug.LogError("EnemyDummy ไม่มี Animator ทำให้เล่นท่าเจ็บไม่ได้!");
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHp -= dmg; 
        
        if (anim != null)
        {
            anim.SetTrigger("Hurt");
        }
        
        if (sr != null)
        {
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }
            flashCoroutine = StartCoroutine(HitFlash()); 
        }

        if (currentHp <= 0) 
        {
            Die(); 
        }
    }
    
    IEnumerator HitFlash()
    {
        sr.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        sr.color = Color.white;
        flashCoroutine = null;
    }

    void Die()
    {
        SpawnPickup();
        Destroy(gameObject); 
    }

    void SpawnPickup()
    {
        if (pickupPrefab == null) 
        {
            Debug.LogError("EnemyDummy ไม่มี Prefab ไอเทมที่จะ Drop!");
            return;
        }

        Vector3 pos;

        if (pickupSpawnPoint != null)
        {
            pos = pickupSpawnPoint.position; 
        }
        else
        {
            pos = transform.position; 
        }

        Instantiate(pickupPrefab, pos, Quaternion.identity);
    }
}