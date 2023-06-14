using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongItems : MonoBehaviour
{
   [SerializeField] private SongNames songName;
   [SerializeField] private AudioClip audioClip;

   public AudioClip GetAudioClip() { return audioClip; }
   public SongNames GetSongNames() { return songName; }
}
