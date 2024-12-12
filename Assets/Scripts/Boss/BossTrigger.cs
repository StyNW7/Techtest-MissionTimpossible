using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BossAreaScript : MonoBehaviour
{

    public GameObject bossUI;
    public AudioSource battlesound;
    public AudioClip bossMusicClip;
    public GameObject player;
    public GameObject DoorLocked;
    float StartBossRange;
    private bool bossStarted;

    void Start()
    {
        player = GameObject.Find("char");
        StartBossRange = 40f;
        bossStarted = false;
        DoorLocked.SetActive(false);
        bossUI.SetActive(false);
        if (battlesound != null)
        {
            battlesound.Stop();
        }
    }

    //void Update()
    //{
    //    StartBoss();
    //}
    
    public bool GetBossStarted()
    {
        return bossStarted;
    }

    void StartBoss()
    {

        if (bossStarted) return;

        bossStarted = true;

        if (AudioManager.Instance != null && bossMusicClip != null)
        {
            AudioManager.Instance.PlayMusic(bossMusicClip);
        }

        Debug.Log("Boss Active");
        DoorLocked.SetActive(true);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }

        if (battlesound != null)
        {
            battlesound.Play();
        }

        bossUI.SetActive(true);

        //BossScript.BossStart = true;

        BossEnemyModified.BossStart = true;

    }

    void OnTriggerEnter()
    {
        StartBoss();
    }

}