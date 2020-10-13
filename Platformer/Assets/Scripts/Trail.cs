using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    public float timeBetweenSpawns;
    public float startTimeBetweenSpawns;

    public GameObject trail;

    private Player_Movement player;

    private void Start()
    {
        player = GetComponent<Player_Movement>();
    }
    void Update()
    {
        if (player.isDashing || player.isSmash) {
            if (timeBetweenSpawns <= 0)
            {
                GameObject instance = Instantiate(trail, transform.position, Quaternion.identity);
                Destroy(instance, 0.5f);
                timeBetweenSpawns = startTimeBetweenSpawns;
            }
            else
            {
                timeBetweenSpawns -= Time.deltaTime;
            } 
        }
    }
}
