using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LogOnScreen : MonoBehaviour

{
    public TMP_Text debugLogText; // Das Textfeld, in das die Debug-Nachrichten ausgegeben werden
    private string logMessages = ""; // Speichert die bisherigen Log-Nachrichten

    void OnEnable()
    {
        // Melde die Methode HandleLog an, um auf Konsolennachrichten zu reagieren
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        // Entferne die Methode HandleLog, um das Abfangen von Nachrichten zu stoppen
        Application.logMessageReceived -= HandleLog;
    }

 
    // Methode zum Hinzufügen einer Debug-Nachricht
   void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Log)
        {
            logMessages += $"{System.DateTime.Now}: {logString}\n";  // Füge nur normale Logs hinzu
            UpdateLogText();  // Aktualisiere das Textfeld
        }
    
    }

    // Aktualisiert das TextMeshPro-Textfeld mit den Log-Nachrichten
    private void UpdateLogText()
    {
        debugLogText.text = logMessages; // Setze den Text des TMP_Text-Objekts
    }
}

