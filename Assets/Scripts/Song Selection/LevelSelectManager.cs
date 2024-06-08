using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
   public static LevelSelectManager Instance { get; private set; }

   [SerializeField] private AudioSource audioSource;
   [Header("Song selection")]
   [SerializeField] private SongSelector[] tracks;
   [SerializeField] private TextMeshProUGUI songName;
   [SerializeField] private TextMeshProUGUI artistName;
   [SerializeField] private TextMeshProUGUI bpmNum;
   [SerializeField] private DifficultySelector difficultySelector;
   [SerializeField] private TrackSelector trackSelector;
   [SerializeField] private AnimationCurve moveCurve;
   [SerializeField] private float moveSpeed = 10f;
   [SerializeField] private Vector3 backPosition;
   [SerializeField] private Vector3 inPosition;
   [SerializeField] private TextMeshProUGUI songIndexNumber;
   [Header("Utility")]
   [SerializeField] private GameInput gameInput;
   [SerializeField] private AudioClip buttonClip;
   [SerializeField] private AudioClip sceneClip;

   [Header("Best Score")]
   [SerializeField] private TextMeshProUGUI score;
   [SerializeField] private TextMeshProUGUI grade;
   [SerializeField] private TextMeshProUGUI combo;
   [SerializeField] private TextMeshProUGUI enemyKilled;
   [SerializeField] private TextMeshProUGUI bossHP;
   [SerializeField] private TextMeshProUGUI playerHP;
   [SerializeField] private TextMeshProUGUI accuracy;
   [Header("Difficulty Panel")]
   [SerializeField] private CanvasGroup difficultyPanel;
   [SerializeField] private TextMeshProUGUI difficultSongName;
   [SerializeField] private TextMeshProUGUI difficultArtistName;
   [Header("Tutorial Tip")]
   [SerializeField] private CanvasGroup tipPanel;

   private bool isMoving = false;

   private string curSongName;
   private Difficulties curDifficuly;
   private SongItemSO.SongSets[] curSongSets;
   private int BPM;
   private AudioClip gameMusic;
   private PlayerName playerName;

   private int curSelectTrack = 0;

   private bool isDifficultyOpen = false;
   private bool isTipOpen = false;

   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      for(int i = 0; i < tracks.Length; i++) {
         if(i == curSelectTrack) {
            tracks[i].gameObject.GetComponent<RectTransform>().anchoredPosition3D = inPosition;
         } else {
            tracks[i].gameObject.GetComponent<RectTransform>().anchoredPosition3D = backPosition;
         }
      }
      gameInput.OnPrevTrackPressed += GameInput_OnPrevTrackPressed;
      gameInput.OnNextTrackPressed += GameInput_OnNextTrackPressed;
      gameInput.OnPrevSongPressed += GameInput_OnPrevSongPressed;
      gameInput.OnNextSongPressed += GameInput_OnNextSongPressed;
      gameInput.OnPrevDifficultyPressed += GameInput_OnPrevDifficultyPressed;
      gameInput.OnNextDifficultyPressed += GameInput_OnNextDifficultyPressed;
      gameInput.OnStartPressed += GameInput_OnStartPressed;
      gameInput.OnBackPressed += GameInput_OnBackPressed;
      if (SaveSystem.TryFirstTimePlaying()) {
         tipPanel.gameObject.SetActive(true);
         isTipOpen = true;
      }
   }

   private void GameInput_OnBackPressed(object sender, System.EventArgs e)
   {
      OnBackButton();
   }

   private void GameInput_OnStartPressed(object sender, System.EventArgs e)
   {
      if (isTipOpen) CloseTip();
      else if (isDifficultyOpen) GoToSong();
      else OnDifficultyOpen();
   }

   private void GameInput_OnNextDifficultyPressed(object sender, System.EventArgs e)
   {
      if (isDifficultyOpen) IncreaseDifficulty();
   }

   private void GameInput_OnPrevDifficultyPressed(object sender, System.EventArgs e)
   {
      if (isDifficultyOpen) DecreaseDifficulty();
   }

   private void GameInput_OnNextSongPressed(object sender, System.EventArgs e)
   {
      if (!isDifficultyOpen) NextSong();
   }

   private void GameInput_OnPrevSongPressed(object sender, System.EventArgs e)
   {
      if (!isDifficultyOpen) PrevSong();
   }

   private void GameInput_OnNextTrackPressed(object sender, System.EventArgs e)
   {
      if (!isDifficultyOpen) NextTrack();
   }

   private void GameInput_OnPrevTrackPressed(object sender, System.EventArgs e)
   {
      if(!isDifficultyOpen) PrevTrack();
   }

   private void OnEnable()
   {
      if (SaveSystem.TryLoadPrevSong(out PrevSongStruct prevSong)) {
         curSelectTrack = prevSong.trackIndex;
         trackSelector.SetPrevTrack(prevSong.trackIndex);
         tracks[prevSong.trackIndex].SetAsCurrentTrack(prevSong.songIndex);
      }
      else {;
         trackSelector.SetPrevTrack(0);
         tracks[0].SetAsCurrentTrack(0);
      }
   }

   public void SetSongName(string name, string displayName, string artistName, int BPM)
   {
      curSongName = name;
      if(displayName != null) {
         songName.text = displayName;
         this.artistName.text = artistName;
         bpmNum.text = "BPM: " + BPM;
      } else {
         songName.text = curSongName;
      }
      this.BPM = BPM;
      UpdateScore();
   }

   public void SetDifficulty(Difficulties difficulty)
   {
      curDifficuly = difficulty;
      UpdateScore();
   }

   public void SetSongSets(SongItemSO.SongSets[] songSets)
   {
      curSongSets = songSets;
   }

   public void SetGameMusic(AudioClip gameMusic)
   {
      this.gameMusic = gameMusic;
   }

   public void SetPlayerName(PlayerName playerName)
   {
      this.playerName = playerName;
   }

   public void GoToSong()
   {
      OnLeavingScene();
      var name = playerName.ToString();
      var diff = curDifficuly.ToString();
      SetSongLoader();
      ClipPlayer.Instance.PlayClip(sceneClip);
      Loader.Load2(name + "_" + diff);
   }

   private void SetSongLoader()
   {
      SongLoader.Instance.SetSongName(curSongName);
      foreach (var songSet in curSongSets) {
         if(songSet.difficulty == curDifficuly) {
            SongLoader.Instance.SetChart(songSet.chart);
            SongLoader.Instance.SetSpawner(songSet.enemySpawn);
         }
      }
      SongLoader.Instance.SetDisplayName(songName.text);
      SongLoader.Instance.SetBPM(BPM);
      SongLoader.Instance.SetGameMusic(gameMusic);
   }

   private void BackToLanding()
   {
      OnLeavingScene();
      SceneManager.LoadScene("Landing");
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
      tracks[curSelectTrack].gameObject.GetComponent<RectTransform>().anchoredPosition3D = backPosition;
      tracks[curSelectTrack].DeselectAsTrack();
      curSelectTrack++;
      curSelectTrack %= tracks.Length;
      tracks[curSelectTrack].SetAsCurrentTrack();
      trackSelector.SetCurrentTrack(curSelectTrack);
      isMoving = true;
      tracks[curSelectTrack].gameObject.GetComponent<RectTransform>().DOAnchorPos3D(inPosition, moveSpeed).SetEase(moveCurve).OnComplete(() =>
      {
         isMoving = false;
      });
      ClipPlayer.Instance.PlayClip(buttonClip);
   }

   public void PrevTrack() 
   {
      if (isMoving) return;
      tracks[curSelectTrack].gameObject.GetComponent<RectTransform>().anchoredPosition3D = backPosition;
      tracks[curSelectTrack].DeselectAsTrack();
      curSelectTrack--;
      if( curSelectTrack < 0 ) curSelectTrack = tracks.Length - 1;
      tracks[curSelectTrack].SetAsCurrentTrack();
      trackSelector.SetCurrentTrack(curSelectTrack);
      isMoving = true;
      tracks[curSelectTrack].gameObject.GetComponent<RectTransform>().DOAnchorPos3D(inPosition, moveSpeed).SetEase(moveCurve).OnComplete(() =>
      {
         isMoving = false;
      });
      ClipPlayer.Instance.PlayClip(buttonClip);
   }

   public void NextSong()
   {
      tracks[curSelectTrack].NextItem(buttonClip);
   }

   public void PrevSong()
   {
      tracks[curSelectTrack].PrevItem(buttonClip);
   }

   public void IncreaseDifficulty()
   {
      difficultySelector.Increase();
      ClipPlayer.Instance.PlayClip(buttonClip);
   }

   public void DecreaseDifficulty()
   {
      difficultySelector.Decrease();
      ClipPlayer.Instance.PlayClip(buttonClip);
   }

   public void GoToSettings()
   {
      OnLeavingScene();
      ClipPlayer.Instance.PlayClip(sceneClip);
      SceneManager.LoadScene("Settings");
   }

   private void UpdateScore()
   {
      if(SaveSystem.TryLoadHighScore(curSongName, curDifficuly, out ScoreStruct score)) {
         this.score.text = score.score.ToString();
         grade.text = score.letterGrade;
         combo.text = score.maxCombo;
         enemyKilled.text = score.enemyKilled;
         bossHP.text = score.bossHP;
         playerHP.text = score.playerHP;
         accuracy.text = score.accuracy;
      } else {
         this.score.text = "-";
         grade.text = "-";
         combo.text = "-";
         enemyKilled.text = "-";
         bossHP.text = "-";
         playerHP.text = "-";
         accuracy.text = "-";
      }
   }

   private void OnLeavingScene()
   {
      PrevSongStruct prevSong = new()
      {
         trackIndex = curSelectTrack,
         songIndex = tracks[curSelectTrack].GetCurrentSongIndex()
      };
      SaveSystem.SavePrevSong(prevSong);
   }

   public void OnBackButton()
   {
      if (isDifficultyOpen) {
         ClipPlayer.Instance.PlayClip(buttonClip);
         difficultyPanel.DOFade(0, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
         {
            difficultyPanel.gameObject.SetActive(false);
            isDifficultyOpen = false;
         });
         tracks[curSelectTrack].gameObject.GetComponent<RectTransform>().DOAnchorPos3D(inPosition, moveSpeed).From(backPosition).SetEase(moveCurve);
      } else if (isTipOpen) {
         CloseTip();
      }
      else {
         BackToLanding();
      }
   }

   public void OnDifficultyOpen()
   {
      isDifficultyOpen = true;
      difficultyPanel.gameObject.SetActive(true);
      difficultySelector.ResetDifficulty();
      difficultSongName.text = songName.text;
      difficultArtistName.text = artistName.text;
      ClipPlayer.Instance.PlayClip(buttonClip);
      difficultyPanel.alpha = 0;
      difficultyPanel.DOFade(1, 0.1f).SetEase(Ease.Linear);
      difficultyPanel.transform.DOLocalMoveY(20, 0.1f).From();
      tracks[curSelectTrack].gameObject.GetComponent<RectTransform>().DOAnchorPos3D(backPosition, moveSpeed).From(inPosition).SetEase(moveCurve);
   }

   public void SetSongIndexNumber(string index)
   {
      songIndexNumber.text = index;
   }

   public void CloseTip()
   {
      ClipPlayer.Instance.PlayClip(buttonClip);
      SaveSystem.SaveFirstTime();
      tipPanel.DOFade(0, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
      {
         tipPanel.gameObject.SetActive(false);
         isTipOpen = false;
      });
   }

   private void LegacyMove()
   {
      //if(isMoving) {
      //   elapsedTime += Time.deltaTime;
      //   float normalizedTime = elapsedTime / moveSpeed;
      //   float curveValue = moveCurve.Evaluate(normalizedTime);

      //   tracks[curSelectTrack].gameObject.transform.localPosition = Vector3.Lerp(backPosition, inPosition, curveValue);

      //   if (normalizedTime >= 1f) {
      //      elapsedTime = 0f;
      //      tracks[curSelectTrack].gameObject.transform.localPosition = inPosition;
      //      isMoving = false;
      //   }
      //}
   }
}
