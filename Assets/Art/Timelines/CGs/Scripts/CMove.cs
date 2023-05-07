using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMove : MonoBehaviour
{
    public float Speedz;
    public float Speedy;
    public float Speedx;
    public float StartTime;
    public float EndTime;
    float t = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (t < StartTime)
        {
            t = t + Time.deltaTime;
        }
        else if (t < EndTime)
        {
            //ÐÐ¶¯
            this.transform.Translate(Vector3.right * Time.deltaTime * Speedx);
            this.transform.Translate(Vector3.up * Time.deltaTime * Speedy);
            this.transform.Translate(Vector3.forward * Time.deltaTime * Speedz);
            t = t + Time.deltaTime;
        }
        else
        {

        }
    }
}