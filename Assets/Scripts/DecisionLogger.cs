using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DecisionLogger : MonoBehaviour
{
    private string filePath;

    // Initialize the logger with game version and player ID
    public void InitializeLogger(string gameVersion, string playerID)
    {
        // Define the folder path inside the Unity project
        string folderPath = Path.Combine(Application.dataPath, "DecisionLogs");

        // Ensure the folder exists; create it if not
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Set file path
        string fileName = $"GameVersion{gameVersion}_Player{playerID}.txt";
        filePath = Path.Combine(folderPath, fileName);

        // Delete the file if it already exists
        if (File.Exists(filePath))
        {
            Debug.Log($"File already exists. Deleting: {filePath}");
            File.Delete(filePath);
        }

        // Create a new, empty file
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
