using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTest : MonoBehaviour
{
    public int x_max;
    public int y_max;

    public int x_min;
    public int y_min;

    public float cam_speed;

    void FixedUpdate()
    {
        Vector2 pos = transform.position;

        pos += new Vector2(Input.GetAxisRaw("Horizontal") * cam_speed, Input.GetAxisRaw("Vertical") * cam_speed);

        pos = Math.restrain(pos, x_max, y_max, x_min, y_min);

        Vector3 temp = pos;
        temp.z = -10f;

        transform.position = temp;
    }
}