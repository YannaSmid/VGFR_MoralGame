using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DecisionLogger : MonoBehaviour
{
    private string filePath;

    public void InitializeLogger(string gameVersion, string playerID)
    {
        string folderPath = Path.Combine(Application.dataPath, "DecisionLogs");

        // Ensure the folder exists
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Construct the file name
        string fileName = $"GameVersion_{gameVersion}_Player_{playerID}.txt";
        filePath = Path.Combine(folderPath, fileName);

        // If the file exists, add "Copy" to the name
        int copyIndex = 1;
        while (File.Exists(filePath))
        {
            fileName = $"GameVersion_{gameVersion}_Player_{playerID}_Copy{copyIndex}.txt";
            filePath = Path.Combine(folderPath, fileName);
            copyIndex++;
        }

        // Create the new log file
        using (FileStream fs = File.Create(filePath))
        {
            Debug.Log($"New log file created: {filePath}");
        }
    }

    // Log a decision or dialogue
    public void LogDecision(string logEntry)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"[{System.DateTime.Now}] {logEntry}");
            }
            Debug.Log($"Successfully logged: {logEntry}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to write to file: {e.Message}");
        }
    }
}
