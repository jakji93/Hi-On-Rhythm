using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
   public static GameplayManager Instance { get; private set; }

   public event EventHandler OnStateChange;
   public event EventHandler OnGamePause;
   public event EventHandler OnGameUnpause;

   [SerializeField] private GameInput gameInput;

   [SerializeField] private float waitingToStart = 1f;
   [SerializeField] private float readyGo = 1f;
   [SerializeField] private float waitingToEnd = 1f;
   [SerializeField] private float musicDelay = 0f;
   [SerializeField] private float chartDelay = 0.5f;
   [SerializeField] private float spawnDelay = 0f;
   [SerializeField] private float playerDeadDelay = 1f;
   [SerializeField] TextAnimatorPlayer textAnimator;

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
      
   }

   private void GameInput_OnPausePressed(object sender, EventArgs e)
   {
      if(state != GameState.Playing) return;
      gamePasued = !gamePasued;
      if (gamePasued) {
         Time.timeScale = 0f;
         musicPlaying = false;
         MusicManager.Instance.PauseMusic();
         OnGamePause?.Invoke(this, EventArgs.Empty);
      } else {
         musicPlaying = true;
         Time.timeScale = 1f;
         MusicManager.Instance.StartMusic();
         OnGameUnpause?.Invoke(this, EventArgs.Empty);
      }
   }

   private void Update()
   {
      switch (state) {
         case GameState.WaitingToStart:
            waitingToStart -= Time.deltaTime;
            if(waitingToStart < 0f) {
               state = GameState.ReadyGo;
               //Invoke Ready/Go! animation
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
               EnemySpawner.Instance.StartSpawn();
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
               state = GameState.WaitingToEnd;
               OnStateChange?.Invoke(this, EventArgs.Empty);
               Debug.Log("waiting to end");
            }
            break;
         case GameState.WaitingToEnd:
            waitingToEnd -= Time.deltaTime;
            if (waitingToEnd < 0f) {
               ChartManager.Instance.StopPlaying();
               state = GameState.Score;
               ScoreManager.Instance.ShowScore();
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
      EnemySpawner.Instance.StopSpawn();
      ChartManager.Instance.StopPlaying();
      MusicManager.Instance.StopMusic();
      Debug.Log("Player Dead");
   }

   public void ResumeGame()
   {
      gamePasued = false;
      musicPlaying = true;
      Time.timeScale = 1f;
      MusicManager.Instance.StartMusic();
      OnGameUnpause?.Invoke(this, EventArgs.Empty);
   }
}
