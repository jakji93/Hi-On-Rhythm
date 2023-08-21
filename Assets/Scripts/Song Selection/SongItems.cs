using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongItems : MonoBehaviour
{
   [SerializeField] private SongNames songName;
   [SerializeField] private AudioClip audioClip;
   [SerializeField] private PulseEffect pulse;
   [SerializeField] private string displayTitle;
   private string artistName;

   public AudioClip GetAudioClip() { return audioClip; }
   public SongNames GetSongNames() { return songName; }
   public string GetDisplayTitle() { return displayTitle; }
   public string GetArtistName() { return artistName; }

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

   public void LoadSO(SongItemSO songItemSO)
   {
      songName = songItemSO.songName;
      audioClip = songItemSO.audioClip;
      displayTitle = songItemSO.displayTitle;
      artistName = songItemSO.artistName;
      var secondsPerBeat = 60f / songItemSO.BPM;
      pulse.SetDelayBetweenPulse(secondsPerBeat);
   }
}
