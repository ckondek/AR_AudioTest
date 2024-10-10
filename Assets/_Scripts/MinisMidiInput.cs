using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

// NoteCallback.cs - This script shows how to define a callback to get notified
// on MIDI note-on/off events.

public class MinisMidiInput : MonoBehaviour
{
   private bool hasExecuted = false;
    private int hittedNote = 0; // Beispielinitialisierung, sollte durch deinen Code gesetzt werden

    // Liste von Noten, die für den Vergleich verwendet werden
    public List <int> noteList = new List<int> { 60, 62, 64 }; // Beispielnoten: Mittel-C, D, E
    public MQTTManager MQTTManager;
    public string topic = "derfabs/unity/midi1/activate"; // Dein MQTT-Topic


    void Start()
    {
MQTTManager = GameObject.FindGameObjectWithTag("MQTT").GetComponentInChildren<MQTTManager>();

        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change != InputDeviceChange.Added) return;

            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;

            midiDevice.onWillNoteOff += (note) => { // Wird die Note losgelassen, dann
            hittedNote = note.noteNumber; 
            hasExecuted = false; 
            }; 
        };
        MQTTManager = GameObject.FindGameObjectWithTag("MQTT").GetComponentInChildren<MQTTManager>();
    }

    void Update ()
    {
     if (noteList.Contains(hittedNote) && !hasExecuted)
        {
            Debug.Log("Yay");
            // Nachricht senden
            SendMqttMessage(hittedNote.ToString());
            SendMqttMessage("activate");
            
            // Status auf 'true' setzen, um anzuzeigen, dass die Funktion ausgeführt wurde
            hasExecuted = true;
        }
        }
    

    void SendMqttMessage(string message)
    { Debug.Log ("Tried to Send Message");
        // Nachricht an das MQTT-Topic senden
        MQTTManager.client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(message));
        
        Debug.Log("MQTT Message Sent: " + message);
    }
    
    }


