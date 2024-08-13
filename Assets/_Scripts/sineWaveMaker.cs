using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sineWaveMaker : MonoBehaviour
{
    [SerializeField, Range(0,1)] private float amplitude = 0.5f;
    [SerializeField, Range(435.00f,445.00f)] private float frequency = 440.00f;  //middle A
    private int _sampleRate;
    private double _phase;

    private void Awake(){
        _sampleRate = AudioSettings.outputSampleRate;
         Debug.Log("phase is: " + _phase);
    }
    
    private void OnAudioFilterRead(float[] data, int channels){
        double phaseIncrement = frequency / _sampleRate;
       
        for (int sample = 0; sample < data.Length; sample += channels)
        {
            float value = Mathf.Sin((float) _phase * 2 * Mathf.PI) * amplitude;

            _phase = (_phase + phaseIncrement) % 1;

            for (int channel = 0; channel < channels; channel ++){
            data[sample + channel] = value;

        }

        
        }

    }
}
