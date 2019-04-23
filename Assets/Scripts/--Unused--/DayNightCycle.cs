using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    //public float timeMultiplier;
    //private float timer = 0;
    //private float intensityPerc = 0;

    //change this to another method later on
    private Light mainLight;
    public float minIntensity = 0.0f;
    public float maxIntensity = 1.2f;
    public float frequency = 0.01f;

    void Start ()
    {
        mainLight = GetComponent<Light>();
        //mainLight.intensity = 1;
	}	

	void Update ()
    {
        float angle = Time.time * frequency;
        angle -= Mathf.Floor(angle);
        mainLight.intensity = maxIntensity * Mathf.Sin(2 * Mathf.PI * angle) + minIntensity;
    }
}
