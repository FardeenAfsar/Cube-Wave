using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgAnim : MonoBehaviour
{
    Material material;
    public float step;
    public float scale;
    public float startVal;
    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    private void FixedUpdate()
    {
        material.SetFloat("_Step", Mathf.PingPong(Time.time * scale, step) + startVal);
    }
}
