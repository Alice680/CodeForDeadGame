using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRemoval : MonoBehaviour
{
    private float spawn;
    public int life_span;

    void Start()
    {
        spawn = Time.time;
    }

    void Update()
    {
        if (Time.time > spawn + life_span)
            Destroy(gameObject);
    }
}
