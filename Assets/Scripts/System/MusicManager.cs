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

   private float[] audioSyncSamples = new float[128];
   private float audioSyncValue;

   private void Awake()
   {
      Instance = this;
      gameMusic.clip.LoadAudioData();
   }

   private void Update()
   {
      GetSpectrumAudioSource();
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
      gameMusic.GetSpectrumData(audioSyncSamples, 0, FFTWindow.Hamming);    
      audioSyncValue = audioSyncSamples[0] * 100 / gameMusic.volume;
   }
   public float GetSynchroData()
   {
      return audioSyncValue;
   }
}