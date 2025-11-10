using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("Link Enemy")]
    public Enemy enemy;                 // ลิงก์ Enemy
    [Header("UI Elements")]
    public RectTransform barRect;       // แถบสีแดง/Green
    public RectMask2D mask;
    public TMP_Text hpText;
    public Vector3 offset = new Vector3(0, 1.5f, 0); // ระยะเหนือหัว

    private float maxRightMask;
    private float initialRightMask;
    private Camera mainCamera;

    private void Start()
    {
        if (!mainCamera)
            mainCamera = Camera.main;

        maxRightMask = barRect.rect.width - mask.padding.x - mask.padding.z;
        initialRightMask = mask.padding.z;

        UpdateBar();
    }

    private void LateUpdate()
    {
        if (enemy == null) return;

        // ทำให้ HealthBar อยู่เหนือหัว Enemy
        transform.position = enemy.transform.position + offset;

        // หมุนหน้าเข้ากล้อง (optional)
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }

    public void UpdateBar()
    {
        if (enemy == null) return;

        float hp = enemy.hp;
        float maxHp = enemy.maxHp;

        float targetWidth = hp * maxRightMask / maxHp;
        float newRightMask = maxRightMask + initialRightMask - targetWidth;

        var padding = mask.padding;
        padding.z = newRightMask;
        mask.padding = padding;

        if (hpText != null)
            hpText.SetText($"{Mathf.CeilToInt(hp)}/{Mathf.CeilToInt(maxHp)}");
    }
}