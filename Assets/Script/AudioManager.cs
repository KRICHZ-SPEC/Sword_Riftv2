using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
    }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Sound List")]
    public Sound[] musicSounds;
    public Sound[] sfxSounds;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMusic("ThemeSong");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.LogError("ไม่เจอเพลงชื่อ: " + name + " ในรายการ Music Sounds!");
            return;
        }
        
        Debug.Log("กำลังเล่นเพลง: " + name);

        musicSource.clip = s.clip;
        musicSource.volume = s.volume;
        musicSource.pitch = s.pitch;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.LogError("ไม่เจอเสียง Effect ชื่อ: " + name + " เช็คตัวสะกดดีๆ!");
            return;
        }
        
        if (name == "Fireball") Debug.Log("ยิง Fireball! (เล่นเสียง)");

        sfxSource.PlayOneShot(s.clip, s.volume);
    }
}