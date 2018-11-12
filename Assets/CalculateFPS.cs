using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculateFPS : MonoBehaviour
{
    private Text t;

    private void Start()
    {
        t = GetComponent<Text>();
    }
    void Update ()
    {
        //Currently capped at 60fps
        t.text = (Time.deltaTime * 100).ToString() + " ms\n";
        t.text += Mathf.RoundToInt(1 / Time.deltaTime).ToString() + " FPS";
    }
}
