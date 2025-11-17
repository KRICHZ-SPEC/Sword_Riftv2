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
        Vector2 dir = player.lastMoveDirection;
        if (dir == Vector2.zero) dir = Vector2.right;

        GameObject fb = GameObject.Instantiate(fireBallPrefab, player.transform.position, Quaternion.identity);
        
        FireBall fireBallScript = fb.GetComponent<FireBall>();
        if (fireBallScript != null)
        {
            fireBallScript.Setup(dir);
        }
        else
        {
            Debug.LogError("FireBall Prefab ไม่มีสคริปต์ FireBall.cs!");
        }
    }
}