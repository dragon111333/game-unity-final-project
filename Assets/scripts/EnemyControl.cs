using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public long lastSpawn;
    public long spawnCooldown = 30;

    public int enLimit = 1;

    public GameObject em;
    private GlobalScript gs;

    void Start()
    {
        gs = GameObject.FindObjectOfType<GlobalScript>();
        lastSpawn = gs.GetNow()+ spawnCooldown;
    }

    void Update()
    {
        if(gs.GetNow() > lastSpawn && this.transform.childCount < enLimit)
        {
            lastSpawn = gs.GetNow() + spawnCooldown;
            GameObject newEn = GameObject.Instantiate(em);
            newEn.transform.parent = this.transform;
        }
    }
}
