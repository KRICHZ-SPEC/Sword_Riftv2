using UnityEngine;

using UnityEngine;

public class TutorialUIFollow : MonoBehaviour
{
    public Transform target;      // ตัวละครที่ต้องการให้ UI ตาม
    public Vector3 offset;        // ระยะห่างจากตัวละคร (เช่น เหนือหัว)

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
