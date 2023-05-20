using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
   public static GameplayManager Instance { get; private set; }

   public event EventHandler OnStateChange;

   [SerializeField] private float waitingToStart = 1f;
   [SerializeField] private float readyGo = 1f;
   [SerializeField] private float waitingToEnd = 1f;
   [SerializeField] private float musicDelay = 0f;
   [SerializeField] private float chartDelay = 0.1f;

   private bool musicPlaying = false;

   private GameState state;

   public enum GameState
   {
      WaitingToStart,
      ReadyGo,
      Playing,
      WaitingToEnd,
      Score,
   }

   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      state = GameState.WaitingToStart;
      Debug.Log("Waiting to start");
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
               Debug.Log("Playing");
            }
            break;
         case GameState.Playing:
            musicDelay -= Time.deltaTime;
            chartDelay -= Time.deltaTime;
            if(chartDelay < 0f) {
               ChartManager.Instance.StartPlaying();
            }
            if (!musicPlaying && musicDelay < 0f) {
               MusicManager.Instance.StartMusic();
               musicPlaying = true;
            }
            if(musicPlaying && !MusicManager.Instance.IsPlaying()) { 
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
               //TODO: ScoreManager.Instance.ShowFianlScore
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
      return state == GameState.Playing;
   }
}
