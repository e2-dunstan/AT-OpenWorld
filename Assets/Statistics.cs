using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Statistics : MonoBehaviour
{
    public static Statistics instance;

    private List<string[]> fpsData = new List<string[]>();
    private List<string> pathfindingTime = new List<string>();

    private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
    
	private void Start ()
    {
        if (instance == null)
            instance = this;

        stopwatch.Start();

        string[] fpsTemp = new string[2];
        fpsTemp[0] = "Time";
        fpsTemp[1] = "ms";
        fpsData.Add(fpsTemp);
	}

    public void SaveFPS(float ms)
    {
        string[] temp = new string[2];
        temp[0] = stopwatch.ElapsedMilliseconds.ToString();
        temp[1] = ms.ToString();
        fpsData.Add(temp);
    }
    public void SavePathfindingTime(float ms)
    {
        pathfindingTime.Add(ms.ToString());
    }

    private void OnApplicationQuit()
    {
        SaveFPSData();
        SavePathfindingData();
    }

    private void SaveFPSData()
    {
        string[][] output = new string[fpsData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = fpsData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = "RuntimeStatistics.csv";

        StreamWriter outStream = File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    private void SavePathfindingData()
    {
        string[] output = new string[pathfindingTime.Count];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = pathfindingTime[i];
        }

        int length = output.Length;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine(string.Join(",", output));
        
        string filePath = "PathfindingStatistics.csv";

        StreamWriter outStream = File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }
}
