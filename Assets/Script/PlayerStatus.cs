using System;
using UnityEngine;

[Serializable]
public class PlayerStatus
{
    // Public state (ตามสเปค)
    public float hp;
    public float mp;
    public float exp;
    public int level;

    // ช่วยเก็บค่าตั้งต้น (ไม่อยู่ในสเปคแต่จำเป็น)
    public float maxHp;
    public float maxMp;

    // ค่า exp ที่ต้องการเพื่อเลเวลถัดไป (สามารถปรับได้)
    private float expToNext = 100f;

    // Event เมื่อเลเวลอัพ (UI/FX จะ subscribe ได้)
    public event Action<int> OnLevelUp; // ส่งเลเวลใหม่

    // Constructor (เรียกตอนสร้าง PlayerStatus)
    public PlayerStatus(float initialHp = 100f, float initialMp = 50f)
    {
        maxHp = initialHp;
        maxMp = initialMp;
        hp = maxHp;
        mp = maxMp;
        exp = 0f;
        level = 1;
    }

    // ลด HP และตรวจ death (caller จัดการการตาย)
    public void TakeDamage(float amount)
    {
        if (amount <= 0f) return;
        hp -= amount;
        if (hp < 0f) hp = 0f;
        // คุณอาจส่ง event ที่นี่ เช่น OnHpChanged
    }

    // เติม HP
    public void Heal(float amount)
    {
        if (amount <= 0f) return;
        hp += amount;
        if (hp > maxHp) hp = maxHp;
        // OnHpChanged event ถ้าต้องการ
    }

    // เพิ่ม exp และเช็คเลเวลอัพ
    public void GainExperience(float amount)
    {
        if (amount <= 0f) return;
        exp += amount;

        // ถ้ามากกว่า threshold ให้ loop เผื่อเก็บหลายเลเวลจาก exp จำนวนมาก
        while (exp >= expToNext)
        {
            exp -= expToNext;
            UpgradeLevel();
            // ปรับ threshold แบบขั้นบันได (ตัวอย่าง)
            expToNext = Mathf.Round(expToNext * 1.2f);
        }
    }

    // เลเวลอัพ: เพิ่มเลเวล ปรับสเตตัสพื้นฐาน และเรียก event
    public void UpgradeLevel()
    {
        level++;
        // ตัวอย่างการเพิ่มสเตตัสเมื่อเลเวลอัพ — ปรับได้ตามเกมบาลานซ์
        maxHp += 10f;
        maxMp += 5f;
        hp = maxHp; // เติมเต็มเมื่อเลเวลอัพ (เลือกเปลี่ยนได้)
        mp = maxMp;

        // แจ้งผู้ฟัง (UI, SFX, Tutorial ฯลฯ)
        OnLevelUp?.Invoke(level);
    }
    public void ConsumeMp(float amount)
    {
        mp -= amount;
        if (mp < 0) mp = 0;
    }
}