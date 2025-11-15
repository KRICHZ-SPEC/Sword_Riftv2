using UnityEngine;

[System.Serializable]
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
        fb.GetComponent<FireBall>().Setup(dir);
    }
}