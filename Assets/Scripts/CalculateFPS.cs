using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class CalculateFPS : MonoBehaviour
{
    private Text t;
    private float renderTime;

    private void Start()
    {
        t = GetComponent<Text>();
        StartCoroutine(UpdateRenderTime());
    }
    //void Update ()
    //{
    //    Currently capped at 60fps
    //    t.text = (Time.deltaTime * 100).ToString() + " ms\n";
    //    t.text += Mathf.RoundToInt(1 / Time.deltaTime).ToString() + " FPS";
    //}

    IEnumerator UpdateRenderTime()
    {
        while (true)
        {
            float renderTime = Mathf.RoundToInt(UnityStats.renderTime * 10000);
            t.text = renderTime.ToString() + " ms";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
