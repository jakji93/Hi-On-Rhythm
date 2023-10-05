using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongLoader : MonoBehaviour
{
   public static SongLoader Instance { get; private set; }

   [SerializeField] private string songName;
   [SerializeField] private GameObject chart;
   [SerializeField] private GameObject spawner;
   [SerializeField] private int BPM;
   [SerializeField] private string displaySongName;
   [SerializeField] private AudioClip gameMusic;

   private void Awake()
   {
      DontDestroyOnLoad(this);
      if (Instance == null) {
         Instance = this;
      }
      else {
         Destroy(gameObject);
      }
   }

   public GameObject GetChart()
   {
      return chart;
   }

   public GameObject GetSpawner()
   {
      return spawner;
   }

   public int GetBPM()
   {
      return BPM;
   }

   public string GetDisplaySongName()
   {
      return displaySongName;
   }

   public string GetSongName()
   {
      return songName;
   }

   public AudioClip GetGameMusic()
   {
      return gameMusic;
   }

   public void SetSongName(string name)
   {
      songName = name;
   }

   public void SetChart(GameObject chart)
   {
      this.chart = chart;
   }

   public void SetSpawner(GameObject spawner)
   {
      this.spawner = spawner;
   }

   public void SetBPM(int bpm)
   {
      this.BPM = bpm;
   }

   public void SetDisplayName(string name)
   {
      this.displaySongName = name;
   }

   public void SetGameMusic(AudioClip gameMusic)
   {
      this.gameMusic = gameMusic;
   }
}
