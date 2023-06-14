using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
   public static LevelSelectManager Instance;

   [SerializeField] private AudioSource audioSource;
   [SerializeField] private TestRotate[] tracks;
   [SerializeField] private TextMeshProUGUI songName;
   [SerializeField] private DifficultySelector difficultySelector;

   private SongNames curSongName;
   private Difficulties curDifficuly;

   private int curSelectTrack = 0;

   private void Awake()
   {
      Instance = this;
   }

   private void OnEnable()
   {
      tracks[curSelectTrack].SetAsCurrentTrack();
   }

   public void SetSongName(SongNames name)
   {
      curSongName = name;
      songName.text = curSongName.ToString();
   }

   public void SetDifficulty(Difficulties difficulty)
   {
      curDifficuly = difficulty;
   }

   public void GoToSong()
   {
      var name = curSongName.ToString();
      var diff = curDifficuly.ToString();
      SceneManager.LoadScene(name + '_' + diff);
   }

   public void PlayThisSong(AudioClip clip)
   {
      audioSource.Stop();
      audioSource.clip = clip;
      audioSource.Play();
   }

   public void NextTrack()
   {
      curSelectTrack++;
      curSelectTrack %= tracks.Length;
      tracks[curSelectTrack].SetAsCurrentTrack();
   }

   public void PrevTrack() 
   {
      curSelectTrack--;
      if( curSelectTrack < 0 ) curSelectTrack = tracks.Length - 1;
      tracks[curSelectTrack].SetAsCurrentTrack();
   }

   public void NextSong()
   {
      tracks[curSelectTrack].NextItem();
   }

   public void PrevSong()
   {
      tracks[curSelectTrack].PrevItem();
   }

   public void IncreaseDifficulty()
   {
      difficultySelector.Increase();
   }

   public void DecreaseDifficulty()
   {
      difficultySelector.Decrease();
   }
}
