using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
   public static MusicManager Instance { get; private set; }

   [SerializeField] private AudioSource musicSource;

   private void Awake()
   {
      Instance = this;
   }

   public void StartMusic()
   {
      if (!musicSource.isPlaying) {
         musicSource.Play(0);
      }
   }

   public void StopMusic()
   {
      musicSource.Stop();
   }

   public bool IsPlaying()
   {
      return musicSource.isPlaying;
   }
}
