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

   private void Awake()
   {
      Instance = this;
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
}
