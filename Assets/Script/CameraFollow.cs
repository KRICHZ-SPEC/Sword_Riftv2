using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // ลาก Player มาวางที่นี่
    public float smoothSpeed = 0.125f;  // ความเร็วในการติดตาม (ปรับได้)
    public Vector3 offset;  // ระยะห่างจากตัวละคร (เช่น Vector3(0, 0, -10))
    void LateUpdate()
    {
        if (target == null) return;
        // คำนวณตำแหน่งที่ต้องการ
        Vector3 desiredPosition = target.position + offset;
        // ทำให้กล้องเคลื่อนไหวอย่างนุ่มนวล
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
