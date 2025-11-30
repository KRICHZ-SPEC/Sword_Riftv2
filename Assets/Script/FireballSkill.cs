using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewFireBallSkill", menuName = "Skills/FireBallSkill")]
public class FireBallSkill : ActiveSkill
{
    public GameObject fireBallPrefab;
    public float damage = 20f;
    public float speed = 10f;
    public float range = 5f;

    public override void Activate(Player player)
    {
        AudioManager.Instance.PlaySFX("Fireball");

        Vector2 dir = player.lastMoveDirection;
        if (dir == Vector2.zero) dir = Vector2.right;

        GameObject fb = ObjectPoolManager.Instance.SpawnFromPool(
            fireBallPrefab,
            player.transform.position,
            Quaternion.identity
        );

        FireBall fireBallScript = fb.GetComponent<FireBall>();
        if (fireBallScript != null)
        {
            fireBallScript.speed = speed;
            fireBallScript.damage = damage;

            float safeSpeed = (speed > 0) ? speed : 0.001f;
            fireBallScript.lifeTime = range / safeSpeed;

            fireBallScript.Setup(dir);
        }
    }
}