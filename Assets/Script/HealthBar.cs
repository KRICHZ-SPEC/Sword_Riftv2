using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private RectTransform _barRect;
    [SerializeField] private RectMask2D _mask;
    [SerializeField] private TMP_Text _hpIndicator;
    [SerializeField] private Camera mainCamera;

    private float _maxRightMask;
    private float _initialRightMask;

    // ระยะ offset จากมุมซ้ายบนหน้าจอ
    [SerializeField] private Vector3 screenOffset = new Vector3(50, -50, 10);

    private void Start()
    {
        if (!mainCamera) mainCamera = Camera.main;

        _maxRightMask = _barRect.rect.width - _mask.padding.x - _mask.padding.z;
        _initialRightMask = _mask.padding.z;

        UpdateBar();
    }

    private void LateUpdate()
    {
        // แปลงตำแหน่งมุมซ้ายบนหน้าจอ → World Position
        Vector3 screenPos = new Vector3(screenOffset.x, Screen.height + screenOffset.y, screenOffset.z);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);

        transform.position = worldPos;

        // ให้ UI หันหน้าเข้ากล้อง
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }

    public void UpdateBar()
    {
        float hp = _player.status.hp;
        float maxHp = _player.status.maxHp;

        float targetWidth = hp * _maxRightMask / maxHp;
        float newRightMask = _maxRightMask + _initialRightMask - targetWidth;

        var padding = _mask.padding;
        padding.z = newRightMask;
        _mask.padding = padding;

        _hpIndicator.SetText($"{Mathf.CeilToInt(hp)}/{Mathf.CeilToInt(maxHp)}");
    }
}