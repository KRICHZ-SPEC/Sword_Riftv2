using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CastFireBall();
        }
    }

    void CastFireBall()
    {
        Vector3 spawnPos = firePoint.position;

        Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
    }
}