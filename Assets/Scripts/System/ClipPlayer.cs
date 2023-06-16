using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipPlayer : MonoBehaviour
{
   public static ClipPlayer Instance;

   [SerializeField] private AudioSource audioSource;

   private void Awake()
   {
      DontDestroyOnLoad(this);
      if (Instance == null) {
         Instance = this;
      } else {
         Destroy(gameObject);
      }
   }

   public void PlayClip(AudioClip clip)
   {
      audioSource.Stop();
      audioSource.clip = clip;
      audioSource.Play();
   }
}
