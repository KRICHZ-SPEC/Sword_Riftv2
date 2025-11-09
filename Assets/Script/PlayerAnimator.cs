using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator anim;
    private PlayerStatus status;

    void Awake()
    {
        anim = GetComponent<Animator>();
        // สมมติว่า PlayerStatus ถูกประกาศใน PlayerController หรือแนบไว้ใน GameObject เดียวกัน
        status = GetComponent<PlayerController2D>()?.status;
    }

    public void PlayHurt()
    {
        anim.SetTrigger("Hurt");
    }

    public void PlayDie()
    {
        anim.SetTrigger("Die");
    }
}