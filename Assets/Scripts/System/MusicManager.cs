using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
   public static MusicManager Instance { get; private set; }

   [SerializeField] private AudioSource gameMusic;
   [SerializeField] private AudioSource failedMusic;
   [SerializeField] private AudioSource victoryMusic;
   [SerializeField] private AudioSource introMusic;

   private float[] audioSamples = new float[512];
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
      gameMusic.clip.LoadAudioData();
   }

   private void Update()
   {
      GetSpectrumAudioSource();
      MakeFrequencyBand();
      BandBuffer();
      CreateAudioBandsNormalized();
   }

   public void StartMusic()
   {
      if (!gameMusic.isPlaying) {
         gameMusic.Play(0);
      }
   }

   public void StopMusic()
   {
      gameMusic.Stop();
   }

   public void PauseMusic()
   {
      gameMusic.Pause();
   }

   public bool IsPlaying()
   {
      return gameMusic.isPlaying;
   }

   public void StartFailedTheme()
   {
      if (!failedMusic.isPlaying) {
         failedMusic.Play(0);
      }
   }

   public void StartVictoryTheme()
   {
      if (!victoryMusic.isPlaying) {
         victoryMusic.Play(0);
      }
   }

   public void StartIntroTheme()
   {
      if (!introMusic.isPlaying) {
         introMusic.Play(0);
      }
   }

   public float GetGameMusicPlaytime()
   {
      return gameMusic.time;
   }

   private void GetSpectrumAudioSource()
   {
      if (gameMusic.volume <= 0) gameMusic.volume = 0.001f;
      gameMusic.GetSpectrumData(audioSamples, 0, FFTWindow.Blackman);
   }

   private void MakeFrequencyBand()
   {
      int count = 0;
      for (int i = 0; i < 8; i++) {
         float average = 0;
         int sampleCount = (int)Mathf.Pow(2, i) * 2;
         if (i == 7) sampleCount += 2;
         for (int j = 0; j < sampleCount; j++) {
            average += audioSamples[count] * (count + 1);
            count++;
         }
         average /= count;
         audio8Bands[i] = average / gameMusic.volume;
      }
   }

   private void BandBuffer()
   {
      for(int i = 0; i < 8; i++) {
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
         if(freqBandHighest[i] != 0) {
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
}