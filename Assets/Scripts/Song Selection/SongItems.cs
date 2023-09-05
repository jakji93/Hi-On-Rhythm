using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SongItems : MonoBehaviour
{
   [SerializeField] private SongNames songName;
   [SerializeField] private AudioClip audioClip;
   [SerializeField] private PulseEffect pulse;
   [SerializeField] private string displayTitle;
   [SerializeField] private EventTrigger trigger;
   [SerializeField] Image coverImage;
   private string artistName;

   private void Awake()
   {
      StopClickTrigger();
      StopImage();
   }

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

   public void SetClickTrigger()
   {
      trigger.enabled = true;
   }

   public void StopClickTrigger()
   {
      trigger.enabled = false;
   }

   public void SetImage()
   {
      var color = coverImage.color;
      color.a = 1f;
      coverImage.color = color;
   }

   public void StopImage()
   {
      var color = coverImage.color;
      color.a = 0.3f;
      coverImage.color = color;
   }

   public void LoadSO(SongItemSO songItemSO)
   {
      songName = songItemSO.songName;
      audioClip = songItemSO.audioClip;
      displayTitle = songItemSO.displayTitle;
      artistName = songItemSO.artistName;
      var secondsPerBeat = 60f / songItemSO.BPM;
      pulse.SetDelayBetweenPulse(secondsPerBeat);
      coverImage.sprite = songItemSO.coverImage;
   }
}
