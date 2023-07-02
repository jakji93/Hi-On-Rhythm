using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
   public static GameplayManager Instance { get; private set; }

   public event EventHandler OnStateChange;
   public event EventHandler OnGamePause;
   public event EventHandler OnGameUnpause;

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
   [SerializeField] private AudioClip clickClip;

   private bool musicPlaying = false;
   private bool gamePasued = false;
   private bool enemySpawned = false;

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
   }

   private void Start()
   {
      Cursor.visible = false;
      state = GameState.WaitingToStart;
      Debug.Log("Waiting to start");
      gameInput.OnPausePressed += GameInput_OnPausePressed;
      MusicManager.Instance.StartIntroTheme();
   }

   private void GameInput_OnPausePressed(object sender, EventArgs e)
   {
      if (state != GameState.Playing) return;
      gamePasued = !gamePasued;
      if (gamePasued) {
         ClipPlayer.Instance.PlayClip(clickClip);
         Time.timeScale = 0f;
         musicPlaying = false;
         MusicManager.Instance.PauseMusic();
         OnGamePause?.Invoke(this, EventArgs.Empty);
      } else {
         musicPlaying = true;
         Time.timeScale = 1f;
         MusicManager.Instance.StartMusic();
         OnGameUnpause?.Invoke(this, EventArgs.Empty);
         ClipPlayer.Instance.PlayClip(clickClip);
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

   public void ResumeGame()
   {
      gamePasued = false;
      musicPlaying = true;
      Time.timeScale = 1f;
      MusicManager.Instance.StartMusic();
      OnGameUnpause?.Invoke(this, EventArgs.Empty);
      ClipPlayer.Instance.PlayClip(clickClip);
   }

   public void ExitGame()
   {
      Time.timeScale = 1f;
      Loader.Load("Song Selection");
   }

   public void Restart()
   {
      Time.timeScale = 1f;
      var sceneName = songName.ToString() + '_' + difficulty.ToString();
      Loader.Load(sceneName);
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
}
