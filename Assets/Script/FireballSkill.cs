using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Fireball")]
public class FireballSkill : ActiveSkill 
{
    public GameObject projectilePrefab;
    public float speed = 8f;
    public float damage = 20f;

    protected override void Activate(Player player) 
    {
        if (projectilePrefab == null) 
        {
            Debug.LogWarning("No projectile prefab assigned.");
            return;
        }

        var go = GameObject.Instantiate(projectilePrefab, player.transform.position + Vector3.up * 0.5f, Quaternion.identity);
        var proj = go.GetComponent<Projectile>();
        if (proj != null) 
        {
            proj.Initialize(damage, speed, player.gameObject.tag); // tag for friend/foe
        }
    }
}