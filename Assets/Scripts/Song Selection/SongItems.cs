using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongItems : MonoBehaviour
{
   [SerializeField] private SongNames songName;
   [SerializeField] private AudioClip audioClip;
   [SerializeField] private PulseEffect pulse;

   public AudioClip GetAudioClip() { return audioClip; }
   public SongNames GetSongNames() { return songName; }

   public void StartPulse()
   {
      if (pulse == null) return;
      pulse.keepConstantPulse = true;
      pulse.StartConstantPulse();
   }

   public void StopPulse()
   {
      if (pulse == null) return;
      pulse.keepConstantPulse = false;
   }
}
