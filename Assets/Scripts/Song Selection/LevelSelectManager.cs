using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
   public static LevelSelectManager Instance { get; private set; }

   [SerializeField] private AudioSource audioSource;
   [SerializeField] private SongSelector[] tracks;
   [SerializeField] private TextMeshProUGUI songName;
   [SerializeField] private DifficultySelector difficultySelector;
   [SerializeField] private TrackSelector trackSelector;
   [SerializeField] private AnimationCurve moveCurve;
   [SerializeField] private float moveSpeed = 10f;
   [SerializeField] private Vector3 backPosition;
   [SerializeField] private Vector3 inPosition;
   [SerializeField] private GameInput gameInput;
   [SerializeField] private AudioClip clickClip;

   [Header("Text")]
   [SerializeField] private TextMeshProUGUI score;
   [SerializeField] private TextMeshProUGUI grade;
   [SerializeField] private TextMeshProUGUI combo;
   [SerializeField] private TextMeshProUGUI enemyKilled;
   [SerializeField] private TextMeshProUGUI bossHP;
   [SerializeField] private TextMeshProUGUI playerHP;

   private float elapsedTime;
   private bool isMoving = false;
   private bool firstUpdate = true;

   private SongNames curSongName;
   private Difficulties curDifficuly;

   private int curSelectTrack = 0;

   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      for(int i = 0; i < tracks.Length; i++) {
         if(i == curSelectTrack) {
            tracks[i].gameObject.transform.localPosition = inPosition;
         } else {
            tracks[i].gameObject.transform.localPosition = backPosition;
         }
      }
      gameInput.OnPrevTrackPressed += GameInput_OnPrevTrackPressed;
      gameInput.OnNextTrackPressed += GameInput_OnNextTrackPressed;
      gameInput.OnPrevSongPressed += GameInput_OnPrevSongPressed;
      gameInput.OnNextSongPressed += GameInput_OnNextSongPressed;
      gameInput.OnPrevDifficultyPressed += GameInput_OnPrevDifficultyPressed;
      gameInput.OnNextDifficultyPressed += GameInput_OnNextDifficultyPressed;
   }

   private void GameInput_OnNextDifficultyPressed(object sender, System.EventArgs e)
   {
      IncreaseDifficulty();
   }

   private void GameInput_OnPrevDifficultyPressed(object sender, System.EventArgs e)
   {
      DecreaseDifficulty();
   }

   private void GameInput_OnNextSongPressed(object sender, System.EventArgs e)
   {
      NextSong();
   }

   private void GameInput_OnPrevSongPressed(object sender, System.EventArgs e)
   {
      PrevSong();
   }

   private void GameInput_OnNextTrackPressed(object sender, System.EventArgs e)
   {
      NextTrack();
   }

   private void GameInput_OnPrevTrackPressed(object sender, System.EventArgs e)
   {
      PrevTrack();
   }

   private void Update()
   {
      if(isMoving) {
         elapsedTime += Time.deltaTime;
         float normalizedTime = elapsedTime / moveSpeed;
         float curveValue = moveCurve.Evaluate(normalizedTime);

         tracks[curSelectTrack].gameObject.transform.localPosition = Vector3.Lerp(backPosition, inPosition, curveValue);

         if (normalizedTime >= 1f) {
            elapsedTime = 0f;
            tracks[curSelectTrack].gameObject.transform.localPosition = inPosition;
            isMoving = false;
         }
      }
   }

   private void OnEnable()
   {
      if (SaveSystem.Instance.TryLoadPrevSong(out PrevSongStruct prevSong)) {
         curSelectTrack = prevSong.trackIndex;
         trackSelector.SetPrevTrack(prevSong.trackIndex);
         tracks[prevSong.trackIndex].SetAsCurrentTrack(prevSong.songIndex);
      }
      else {
         tracks[curSelectTrack].SetAsCurrentTrack();
         trackSelector.SetCurrentTrack(curSelectTrack);
      }
   }

   public void SetSongName(SongNames name)
   {
      curSongName = name;
      songName.text = curSongName.ToString();
      UpdateScore();
   }

   public void SetDifficulty(Difficulties difficulty)
   {
      curDifficuly = difficulty;
      UpdateScore();
   }

   public void GoToSong()
   {
      PrevSongStruct prevSong = new();
      prevSong.trackIndex = curSelectTrack;
      prevSong.songIndex = tracks[curSelectTrack].GetCurrentSongIndex();
      SaveSystem.Instance.SavePrevSong(prevSong);
      ClipPlayer.Instance.PlayClip(clickClip);
      var name = curSongName.ToString();
      var diff = curDifficuly.ToString();
      Loader.Load(name + "_" + diff);
   }

   public void PlayThisSong(AudioClip clip)
   {
      audioSource.Stop();
      audioSource.clip = clip;
      audioSource.Play();
   }

   public void NextTrack()
   {
      if (isMoving) return;
      tracks[curSelectTrack].gameObject.transform.localPosition = backPosition;
      tracks[curSelectTrack].DeselectAsTrack();
      curSelectTrack++;
      curSelectTrack %= tracks.Length;
      tracks[curSelectTrack].SetAsCurrentTrack();
      trackSelector.SetCurrentTrack(curSelectTrack);
      elapsedTime = 0;
      isMoving = true;
   }

   public void PrevTrack() 
   {
      if (isMoving) return;
      tracks[curSelectTrack].gameObject.transform.localPosition = backPosition;
      tracks[curSelectTrack].DeselectAsTrack();
      curSelectTrack--;
      if( curSelectTrack < 0 ) curSelectTrack = tracks.Length - 1;
      tracks[curSelectTrack].SetAsCurrentTrack();
      trackSelector.SetCurrentTrack(curSelectTrack);
      elapsedTime = 0;
      isMoving = true;
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

   public void GoToSettings()
   {
      PrevSongStruct prevSong = new();
      prevSong.trackIndex = curSelectTrack;
      prevSong.songIndex = tracks[curSelectTrack].GetCurrentSongIndex();
      SaveSystem.Instance.SavePrevSong(prevSong);
      SceneManager.LoadScene("Settings");
   }

   private void UpdateScore()
   {
      if(SaveSystem.Instance.TryLoadHighScore(curSongName, curDifficuly, out ScoreStruct score)) {
         this.score.text = score.score.ToString();
         grade.text = score.letterGrade;
         combo.text = score.maxCombo;
         enemyKilled.text = score.enemyKilled;
         bossHP.text = score.bossHP;
         playerHP.text = score.playerHP;
      } else {
         this.score.text = "-";
         grade.text = "-";
         combo.text = "-";
         enemyKilled.text = "-";
         bossHP.text = "-";
         playerHP.text = "-";
      }
   }
}
