using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SongItemSO : ScriptableObject
{
   [Serializable]
   public struct SongSets
   {
      public Difficulties difficulty;
      public GameObject enemySpawn;
      public GameObject chart;
   }
   public string songName;
   public PlayerName playerName;
   public AudioClip audioClip;
   public string displayTitle;
   public int BPM;
   public string artistName;
   public float initialDelay;
   public Sprite coverImage;
   public SongSets[] songSets;
   public AudioClip gameMusic;
}
