## สรุปการตรวจโค้ด (Wave system & Skill)

วันที่: 2025-11-24

ขอบเขตการตรวจ: ระบบเวฟ (Wave) และระบบสกิล (FireBall skill) — ไฟล์ที่อ่าน: `Assets/Script/Wave.cs`, `WaveManager.cs`, `Wave1Manager.cs`, `Wave2Manager.cs`, `ActiveSkill.cs`, `FireballSkill.cs`, `Player.cs`, `Enemy.cs`, `FireBall.cs`.

**ภาพรวมสั้น ๆ**:

- โค้ดอ่านง่าย มีการใช้ `Header` และ field ที่ชัดเจนสำหรับ Inspector ซึ่งดี
- โครงสร้าง tutorial (Wave1, Wave2) อ่านเข้าใจได้ แต่มีปัญหาด้านความถูกต้องของ gameplay ที่อาจทำให้เวฟไม่เดินต่อหรือสกิลไม่ทำงานตามที่คาด

**ประเด็นสำคัญ (High impact)**

- **ไม่มีการแจ้งเมื่อศัตรูตายให้ WaveManager ทราบ**: `WaveManager.OnEnemyKilled()` มีอยู่แต่ไม่มีใครเรียก ทำให้เวฟไม่ก้าวหน้า
- **การใช้สกิลข้ามการเช็ค cooldown**: ใน `Player.Update()` เรียก `fireBallSkill.Activate(this)` โดยตรง แทนที่จะเรียก `UseSkill()` ซึ่งอัปเดต `lastUsedTime` และจัดการ cooldown
- **เก็บสถานะ runtime บน ScriptableObject**: `ActiveSkill.lastUsedTime` อยู่บน `ScriptableObject` — asset แบบนี้จะถูกแชร์ระหว่างทุก instance ที่อ้างอิงสกิลเดียวกัน (ไม่เหมาะสำหรับสถานะต่อผู้เล่น/ต่อ instance)
- **พารามิเตอร์สกิลไม่ถูกส่งให้ projectile**: `FireBallSkill` มี `damage/speed/range` แต่ตอนสร้าง `FireBall` ไม่ได้ตั้งค่า properties เหล่านี้ ทำให้ projectile ใช้ค่าดีฟอลต์ของตัวมันเอง
- **nested-loop ที่อาจไม่ตั้งใจ**: `Waves.SpawnEnemies()` เป็น for(each prefab) × for(each spawn) → เหมือนมันจะทำให้สร้าง enemy ทุกแบบ ในแต่ละจุด spawnpoint ครับ

**ประเด็นรอง (Medium / Low impact)**

- singletons (`Wave1Manager.Instance`, `Wave2Manager.Instance`) ไม่มี guard ป้องกัน duplicate instances อันนี้ถ้าจะทำเป็น singleton ต้องเขียนให้ถูกต้องตาม pattern นะครับ
- `WaveManager` มี field `fireballSkill` และ `player` และยังเพิ่มสกิลเข้า `player.skills` ทำให้มีความไม่สอดคล้องของแหล่งข้อมูลสกิล
- การตรวจสอบ `aliveEnemies` ด้วยการ RemoveAll(e => e == null) เหมาะกับการทำลาย (Destroy) แต่ไม่ครอบคลุมกรณีที่ใช้ object pooling หรือการ deactivate

**ข้อเสนอแนะการแก้ไข (Recommended fixes)**

1. เชื่อมการตายของศัตรู → ระบบเวฟ (แนะนำแบบ event-driven)

- เพิ่ม event ใน `Enemy`:

```csharp
public static System.Action<Enemy> OnEnemyDied;

void Die()
{
    isDead = true;
    anim.SetBool("isDead", true);
    rb.velocity = Vector2.zero;
    rb.bodyType = RigidbodyType2D.Static;
    GetComponent<Collider2D>().enabled = false;
    OnEnemyDied?.Invoke(this);
    Destroy(gameObject, 2f);
}
```

- ใน `WaveManager` ให้ subscribe/unsubscribe ใน `OnEnable` / `OnDisable` แล้วเรียก `OnEnemyKilled()` หรือจัดการ `aliveEnemies` ตามที่ออกแบบไว้

2. ใช้ path การเรียกสกิลที่ถูกต้อง (respect cooldown)

- แทนที่จะเรียก `Activate` โดยตรง ให้เปลี่ยนเป็น `UseSkill(player)` ซึ่งจะตรวจสอบ `CanUse()` และตั้งค่า `lastUsedTime`:

```csharp
// Player.Update()
if (Input.GetKeyDown(KeyCode.F))
{
    if (fireBallSkill != null)
    {
        fireBallSkill.UseSkill(this); // ใช้ method นี้แทน Activate
        if (Wave2Manager.Instance != null)
            Wave2Manager.Instance.OnUseSkill();
    }
}
```

3. อย่าเก็บ runtime state บน `ScriptableObject` (ถ้าต้องการหลายผู้เล่นหรือตัวอย่างแยกกัน)

- ทางเลือกที่แนะนำ: สร้าง wrapper runtime เช่น `SkillInstance` เก็บ `ActiveSkill skillAsset` และ `float lastUsedTime` ของแต่ละผู้เล่น:

```csharp
public class SkillInstance
{
    public ActiveSkill skillAsset;
    public float lastUsedTime = -999f;

    public SkillInstance(ActiveSkill asset) { skillAsset = asset; }

    public bool CanUse() => Time.time >= lastUsedTime + skillAsset.cooldown;

    public void Use(Player player)
    {
        if (!CanUse()) return;
        lastUsedTime = Time.time;
        skillAsset.Activate(player);
    }
}
```

- `Player` เก็บ `List<SkillInstance> skillInstances` แทนการเก็บ `ScriptableObject` runtime fields

4. ส่งพารามิเตอร์สกิลไปยัง projectile

- ใน `FireBallSkill.Activate` หลัง Instantiate ให้ตั้งค่าค่าให้ `FireBall`:

```csharp
var fb = Instantiate(fireBallPrefab, player.transform.position, Quaternion.identity);
var fbScript = fb.GetComponent<FireBall>();
if (fbScript != null)
{
    fbScript.Setup(dir);
    fbScript.speed = speed;
    fbScript.damage = damage;
    fbScript.lifeTime = Mathf.Max(range / Mathf.Max(speed, 0.0001f), fbScript.lifeTime);
}
```

5. ปรับปรุง `Waves.SpawnEnemies()` ให้ configurable

- ถ้าต้องการสร้างจำนวน enemy ตาม spawn point สำหรับแต่ละ prefab ให้แยกชัดเจนหรือเพิ่มตัวเลือก เช่น `spawnPerPoint` หรือใช้ mapping `spawnPoint -> prefab`

6. ปรับปรุง singletons ให้ปลอดภัย

- ใน `Awake()` ของ manager ให้ตรวจ Instance duplicate แล้ว `Destroy(gameObject)` หรือ `DontDestroyOnLoad` ตามต้องการ

**ข้อเสนอแนะเชิงการออกแบบ (ถ้าอยากต่อยอด)**

- ใช้ event bus / C# events สำหรับการสื่อสารระหว่างระบบ (Enemy ↔ WaveManager) เพื่อลด coupling หรือส่วนของ code ที่เกี่ยวพันกันมากเกินไปทำให้ยากต่อการจัดการโครงสร้าง
- ใช้ Object Pooling สำหรับ projectile และศัตรู ถ้าตัวเกมมีการ spawn ซ้ำบ่อย ๆ เพื่อประสิทธิภาพ
- สร้างระบบ `WaveConfig` ที่เก็บข้อมูลว่าแต่ละ wave จะ spawn อะไร เมื่อไร และมีการปลดล็อกสกิลใด เพื่อให้ `WaveManager` ไม่ต้องมี logic แบบ hard-coded จะได้ไม่ต้องเขียน class WaveManager2, WaveManager3, .... WaveManagerXXX
