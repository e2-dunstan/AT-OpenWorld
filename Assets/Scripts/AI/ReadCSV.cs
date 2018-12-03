using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class ReadCSV : MonoBehaviour
{
    public class CSVData
    {
        public int x;
        public int y;
        public int count;
    }

    public GridGenerator gridGenerator;

    private List<string[]> rowData = new List<string[]>();
    private string filePath = "Assets/Resources/aiCount.csv";
    

    public void GenerateTextFile()
    {
        Vector2 size = gridGenerator.gridDimensions;
        
        string[] rowDataTemp = new string[(int)size.x];

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                rowDataTemp[x] = "0";
            }
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();

        Debug.Log("CSV file created at " + filePath);
    }

    public List<CSVData> ReadFile()
    {
        StreamReader inStream = new StreamReader(filePath);
        bool endOfFile = false;
        List<CSVData> csvData = new List<CSVData>();

        int row = 0;

        //Finds all values line by line
        while (!endOfFile)
        {
            string dataString = inStream.ReadLine();
            if (dataString == null)
            {
                endOfFile = true;
                break;
            }

            string[] dataValues = dataString.Split(',');
            for(int column = 0; column < dataValues.Length; column++)
            {
                CSVData newItem = new CSVData();
                newItem.x = column;
                newItem.y = row;
                int c;
                if (int.TryParse(dataValues[column], out c))
                {
                    newItem.count = c;
                }
                csvData.Add(newItem);
            }
            row++;
        }
        return csvData;
    }
}
