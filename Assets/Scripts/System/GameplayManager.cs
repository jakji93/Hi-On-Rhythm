using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Febucci.UI;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
   public static GameplayManager Instance { get; private set; }

   public event EventHandler OnStateChange;
   public event EventHandler OnGamePause;
   public event EventHandler OnGameUnpause;
   public event EventHandler OnFirstBeat;

   [SerializeField] private SongNames songName;
   [SerializeField] private Difficulties difficulty;

   [SerializeField] private GameInput gameInput;

   [SerializeField] private float waitingToStart = 1f;
   [SerializeField] private float readyGo = 1f;
   [SerializeField] private float waitingToEnd = 1f;
   [SerializeField] private float musicDelay = 0f;
   [SerializeField] private float chartDelay = 0.5f;
   [SerializeField] private float spawnDelay = 0f;
   [SerializeField] private float playerDeadDelay = 1f;
   [SerializeField] TextAnimatorPlayer textAnimator;
   [SerializeField] private AudioClip pauseSFX;
   [SerializeField] private string selectionSceneName;
   [SerializeField] private MMF_Player openingPlayer;
   [SerializeField] private RectTransform healthUI;
   [SerializeField] private RectTransform feverUI;

   private bool musicPlaying = false;
   private bool gamePasued = false;
   private bool enemySpawned = false;
   private bool canPause = true;
   private bool useEffect = true;

   private GameState state;

   public enum GameState
   {
      WaitingToStart,
      ReadyGo,
      Playing,
      PlayerDead,
      WaitingToEnd,
      Score,
   }

   private void Awake()
   {
      Instance = this;
      if (PlayerPrefs.HasKey("SpecialEffects")) {
         useEffect = PlayerPrefs.GetInt("SpecialEffects") == 1;
      }
      else {
         useEffect = true;
      }
   }

   private void Start()
   {
      state = GameState.WaitingToStart;
      Debug.Log("Waiting to start");
      gameInput.OnPausePressed += GameInput_OnPausePressed;
      MusicManager.Instance.StartIntroTheme();
   }

   private void GameInput_OnPausePressed(object sender, EventArgs e)
   {
      if (state != GameState.Playing) return;
      if (!canPause) return;
      if (!gamePasued) {
         PauseGame();
      } else {
         ResumeGame();
      }
   }

   private void Update()
   {
      switch (state) {
         case GameState.WaitingToStart:
            waitingToStart -= Time.deltaTime;
            if(waitingToStart < 0f) {
               state = GameState.ReadyGo;
               textAnimator.ShowText("ready");
               OnStateChange?.Invoke(this, EventArgs.Empty);
               Debug.Log("Ready to Go");
               openingPlayer.PlayFeedbacks();
               healthUI.DOLocalMoveY(509f, 3f);
               feverUI.DOLocalMoveY(-501f, 3f);
            }
            break;
         case GameState.ReadyGo:
            readyGo -= Time.deltaTime;
            if (readyGo < 0f) {
               state = GameState.Playing;
               OnStateChange?.Invoke(this, EventArgs.Empty);
               textAnimator.StartDisappearingText();
               Debug.Log("Playing");
            }
            break;
         case GameState.Playing:
            musicDelay -= Time.deltaTime;
            chartDelay -= Time.deltaTime;
            spawnDelay -= Time.deltaTime;
            if(chartDelay < 0f) {
               ChartManager.Instance.StartPlaying();
               OnFirstBeat?.Invoke(this, EventArgs.Empty);
            }
            if (!musicPlaying && musicDelay < 0f && !gamePasued) {
               MusicManager.Instance.StartMusic();
               musicPlaying = true;
            }
            if(spawnDelay < 0f && !enemySpawned) {
               enemySpawned = true;
               EnemySpawner.Instance?.StartSpawn();
            }
            if(musicPlaying && !MusicManager.Instance.IsPlaying()) { 
               state = GameState.WaitingToEnd;
               OnStateChange?.Invoke(this, EventArgs.Empty);
               Debug.Log("waiting to end");
            }
            break;
         case GameState.PlayerDead:
            playerDeadDelay -= Time.deltaTime;
            if(playerDeadDelay < 0f) {
               state = GameState.Score;
               MusicManager.Instance.StopMusic();
               ScoreManager.Instance.ShowFailed();
               MusicManager.Instance.StartFailedTheme();
               OnStateChange?.Invoke(this, EventArgs.Empty);
               Debug.Log("Score");
            }
            break;
         case GameState.WaitingToEnd:
            waitingToEnd -= Time.deltaTime;
            if (waitingToEnd < 0f) {
               ChartManager.Instance.StopPlaying();
               state = GameState.Score;
               ScoreManager.Instance.ShowScore();
               MusicManager.Instance.StartVictoryTheme();
               OnStateChange?.Invoke(this, EventArgs.Empty);
               Debug.Log("Score");
            }
            break;
         case GameState.Score:
            break;
      }
   }

   public bool IsGamePlaying()
   {
      return state == GameState.Playing && !gamePasued;
   }

   public void PlayerDead()
   {
      state = GameState.PlayerDead;
      OnStateChange?.Invoke(this, EventArgs.Empty);
      EnemySpawner.Instance?.StopSpawn();
      ChartManager.Instance.StopPlaying(); 
      Debug.Log("Player Dead");
   }

   private IEnumerator ResumeCountdown()
   {
      canPause = false;
      textAnimator.ShowText("3");
      yield return new WaitForSecondsRealtime(1f);
      textAnimator.ShowText("2");
      yield return new WaitForSecondsRealtime(1f);
      textAnimator.ShowText("1");
      yield return new WaitForSecondsRealtime(1f);
      textAnimator.StartDisappearingText();
      yield return new WaitForSecondsRealtime(0.3f);
      gamePasued = false;
      musicPlaying = true;
      Time.timeScale = 1f;
      MusicManager.Instance.StartMusic();
      yield return new WaitForSecondsRealtime(0.3f);
      canPause = true;
   }

   private IEnumerator PauseDelay()
   {
      yield return new WaitForSecondsRealtime(0.3f);
      canPause = true;
   }

   public void ResumeGame()
   {
      StartCoroutine(ResumeCountdown());
      OnGameUnpause?.Invoke(this, EventArgs.Empty);
      ClipPlayer.Instance.PlayClip(pauseSFX);
   }

   private void PauseGame()
   {
      gamePasued = true;
      canPause = false;
      ClipPlayer.Instance.PlayClip(pauseSFX);
      Time.timeScale = 0f;
      musicPlaying = false;
      MusicManager.Instance.PauseMusic();
      OnGamePause?.Invoke(this, EventArgs.Empty);
      StartCoroutine(PauseDelay());
   }

   public void ExitGame()
   {
      Time.timeScale = 1f;
      Loader.Load2(selectionSceneName);
   }

   public void Restart()
   {
      Time.timeScale = 1f;
      var sceneName = songName.ToString() + '_' + difficulty.ToString();
      Loader.Load2(sceneName);
   }

   public SongNames GetSongName()
   {
      return songName;
   }

   public Difficulties GetDifficulty()
   {
      return difficulty;
   }

   public bool IsGameOver()
   {
      return state == GameState.Score;
   }

   private void OnDestroy()
   {
      DOTween.KillAll();
   }

   public bool UseEffect()
   {
      return useEffect;
   }
}
