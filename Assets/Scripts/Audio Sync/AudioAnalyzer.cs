using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnalyzer : MonoBehaviour
{
   public static AudioAnalyzer Instance { get; private set; }

   [SerializeField] private AudioSource targetAudioSource;
   [SerializeField] private FFTWindow FFTWindow;

   private float[] audioSamples = new float[1024];
   private float[] audio8Bands = new float[8];
   private float[] audio8BuffBands = new float[8];
   private float[] bufferDecreases = new float[8];
   private float[] freqBandHighest = new float[8];
   private float[] audio8BandsNormalized = new float[8];
   private float[] audio8BuffBandsNormalized = new float[8];

   private float audioBandAmplitude;
   private float audioBufferBandAmplitude;
   private float amplitudeHighest;

   private void Awake()
   {
      Instance = this;
   }

   private void Update()
   {
      GetSpectrumAudioSource();
      MakeFrequencyBand();
      BandBuffer();
      CreateAudioBandsNormalized();
      CreateAudioAmplitube();
   }

   private void GetSpectrumAudioSource()
   {
      if (targetAudioSource.volume <= 0) targetAudioSource.volume = 0.001f;
      targetAudioSource.GetSpectrumData(audioSamples, 0, FFTWindow);
   }

   private void MakeFrequencyBand()
   {
      int count = 0;
      for (int i = 0; i < 8; i++) {
         float average = 0;
         int sampleCount = (int)Mathf.Pow(2, i) * 4;
         if (i == 7) sampleCount += 4;
         for (int j = 0; j < sampleCount; j++) {
            average += audioSamples[count] * (count + 1);
            count++;
         }
         average /= count;
         audio8Bands[i] = average / targetAudioSource.volume;
      }
   }

   private void BandBuffer()
   {
      for (int i = 0; i < 8; i++) {
         if (audio8Bands[i] > audio8BuffBands[i]) {
            audio8BuffBands[i] = audio8Bands[i];
            bufferDecreases[i] = 0.005f;
         }

         if (audio8Bands[i] < audio8BuffBands[i]) {
            audio8BuffBands[i] -= bufferDecreases[i];
            bufferDecreases[i] *= 1.2f;
         }
      }
   }

   private void CreateAudioBandsNormalized()
   {
      for (int i = 0; i < 8; i++) {
         if (audio8Bands[i] > freqBandHighest[i]) {
            freqBandHighest[i] = audio8Bands[i];
         }
         if (freqBandHighest[i] != 0) {
            audio8BandsNormalized[i] = audio8Bands[i] / freqBandHighest[i];
            audio8BuffBandsNormalized[i] = audio8BuffBands[i] / freqBandHighest[i];
         }
      }
   }

   private void CreateAudioAmplitube()
   {
      float curAmp = 0f;
      float curBuffAmp = 0f;
      for (int i = 0; i < 8; i++) {
         curAmp += audio8Bands[i];
         curBuffAmp += audio8BuffBands[i];
      }
      if (curAmp > amplitudeHighest) {
         amplitudeHighest = curAmp;
      }
      if (amplitudeHighest != 0f) {
         audioBandAmplitude = curAmp / amplitudeHighest;
         audioBufferBandAmplitude = curBuffAmp / amplitudeHighest;
      }
   }

   public float Get8BandData(int band)
   {
      return audio8Bands[band];
   }

   public float GetBufferBandData(int band)
   {
      return audio8BuffBands[band];
   }

   public float GetBandNormalizedData(int band)
   {
      return audio8BandsNormalized[band];
   }

   public float GetBufferBandNormalizedData(int band)
   {
      return audio8BuffBandsNormalized[band];
   }

   public float GetAmplitude()
   {
      return audioBandAmplitude;
   }

   public float GetBufferAmplitude()
   {
      return audioBufferBandAmplitude;
   }
}
