using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SongItemSO : ScriptableObject
{
   public SongNames songName;
   public AudioClip audioClip;
   public string displayTitle;
   public int BPM;
   public string artistName;
}
