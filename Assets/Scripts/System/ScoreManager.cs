using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
   private const string MAX_COMBO = "MAX COMBO: ";
   private const string ENEMY_KILLED = "ENEMY KILLED: ";
   private const string BOSS_HP = "BOSS HP: ";
   private const string NOTE_PERFECT = "PERFECT: ";
   private const string NOTE_GREAT = "GREAT: ";
   private const string NOTE_GOOD = "GOOD: ";
   private const string NOTE_MISS = "MISS: ";
   private const string PLAYER_HP = "PLAYER HP: ";
   public static ScoreManager Instance { get; private set; }

   [SerializeField] private float perfectHitMultiplier = 10f;
   [SerializeField] private float greatHitMultiplier = 10f;
   [SerializeField] private float goodHitMultiplier = 10f;
   [SerializeField] private float noteMissedMultiplier = 15f;
   [SerializeField] private float enemyKilledMultiplier = 20f;
   [SerializeField] private float bossHealthMultiplier = 20f;
   [SerializeField] private float playerHealthMultiplier = 10f;
   [SerializeField] private float highestComboMultiplier = 10f;
   [SerializeField] private bool isBossStage = false;
   [SerializeField] private Score scoreBoard;
   [SerializeField] private Failed failedBoard;

   [Header("Text Fields")]
   [SerializeField] private TextMeshProUGUI songNameText;
   [SerializeField] private TextMeshProUGUI difficultyText;
   [SerializeField] private TextMeshProUGUI maxComboText;
   [SerializeField] private TextMeshProUGUI EnemyBossText;
   [SerializeField] private TextMeshProUGUI EnemyKilledText;
   [SerializeField] private TextMeshProUGUI perfectText;
   [SerializeField] private TextMeshProUGUI greatText;
   [SerializeField] private TextMeshProUGUI goodText;
   [SerializeField] private TextMeshProUGUI noteMissText;
   [SerializeField] private TextMeshProUGUI playerHealthText;
   [SerializeField] private TextMeshProUGUI finalScoreText;
   [SerializeField] private TextMeshProUGUI letterGradeText;
   [SerializeField] private TextMeshProUGUI newBestText;
   [SerializeField] private TextMeshProUGUI failedText;

   [Header("Letter Grade Threshold")]
   [SerializeField] private float dGrade = 0.59f;
   [SerializeField] private float cGrade = 0.69f;
   [SerializeField] private float bGrade = 0.79f;
   [SerializeField] private float aGrade = 0.89f;

   [Header("Save Game")]
   [SerializeField] private bool saveScore = false;

   private int noteHitCounter = 0;
   private int noteMissedCounter = 0;
   private int enemyKilledCounter = 0;
   private int perfectHitCounter = 0;
   private int greatHitCounter = 0;
   private int goodHitCounter = 0;
   private string songName;

   //TODO: Add Restart and Return key bindings
   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      NoteManager.Instance.OnNoteMissed += NoteManager_OnNoteMissed;
      NoteManager.Instance.OnNormal1Hit += NoteManager_OnNormal1Hit;
      NoteManager.Instance.OnNormal2Hit += NoteManager_OnNormal2Hit;
      NoteManager.Instance.OnSpecialHit += NoteManger_OnSpecialHit;
      NoteManager.Instance.OnWrongNote += NoteManager_OnWrongNote;
      NoteManager.Instance.OnNotePerfect += NoteManager_OnNotePerfect;
      NoteManager.Instance.OnNoteGreat += NoteManager_OnNoteGreat;
      NoteManager.Instance.OnNoteGood += NoteManager_OnNoteGood;
      songName = SongLoader.Instance.GetDisplaySongName();
      songNameText.text = songName;
      failedText.text = songName;
      HideScore();
      HideFailed();
   }

   private void NoteManager_OnNoteGood(object sender, EventArgs e)
   {
      goodHitCounter++;
   }

   private void NoteManager_OnNoteGreat(object sender, EventArgs e)
   {
      greatHitCounter++;
   }

   private void NoteManager_OnNotePerfect(object sender, EventArgs e)
   {
      perfectHitCounter++;
   }

   private void NoteManager_OnWrongNote(object sender, System.EventArgs e)
   {
      noteMissedCounter++;
   }

   private void NoteManger_OnSpecialHit(object sender, System.EventArgs e)
   {
      noteHitCounter++;
   }

   private void NoteManager_OnNormal2Hit(object sender, System.EventArgs e)
   {
      noteHitCounter++;
   }

   private void NoteManager_OnNormal1Hit(object sender, System.EventArgs e)
   {
      noteHitCounter++;
   }

   private void NoteManager_OnNoteMissed(object sender, System.EventArgs e)
   {
      noteMissedCounter++;
   }

   public void EnemyKilled()
   {
      enemyKilledCounter++;
   }

   public void ShowScore()
   {
      float finalScore = Mathf.Max(0, GetFinalScore());
      string letterGrade = GetLetterGrade(finalScore);
      SetTextFields(finalScore, letterGrade);
      ActivateScore();
      if (saveScore) SaveScore(finalScore, letterGrade);
   }

   public void ShowFailed()
   {
      ActivateFailed();
   }

   private void HideScore()
   {
      scoreBoard.gameObject.SetActive(false);
   }

   private void HideFailed()
   {
      failedBoard.gameObject.SetActive(false);
   }

   private void ActivateScore()
   {
      scoreBoard.gameObject.SetActive(true);
      scoreBoard.StartAnimation();
   }

   private void ActivateFailed()
   {
      failedBoard.gameObject.SetActive(true);
      failedBoard.StartAnimation();
   }

   private float GetFinalScore()
   {
      float finalScore = 0f;
      finalScore += perfectHitCounter * perfectHitMultiplier;
      finalScore += greatHitCounter * greatHitMultiplier;
      finalScore += goodHitCounter * goodHitMultiplier;
      finalScore -= noteMissedCounter * noteMissedMultiplier;
      if (isBossStage) {
         var maxHealth = BossController.Instance.GetBossMaxHealth();
         var curHealth = BossController.Instance.GetBossHealth();
         finalScore += (maxHealth - curHealth) * bossHealthMultiplier;
      }
      finalScore += enemyKilledCounter * enemyKilledMultiplier;
      finalScore += PlayerControl.Instance.GetPlayerHealth() * playerHealthMultiplier;
      finalScore += ComboManager.Instance.GetMaxCombo() * highestComboMultiplier;
      return finalScore;
   }

   private string GetLetterGrade(float finalScore)
   {
      string letterGrade = "D";
      //float scoreToUse = finalScore;
      float scoreToUse = NoteManager.Instance.GetAccuracyRating();
      //var max = GetMaxScore();
      var max = 1f;
      if (scoreToUse / max > dGrade) {
         letterGrade = "C";
         if(scoreToUse / max > cGrade) {
            letterGrade = "B";
            if(scoreToUse / max > bGrade) {
               letterGrade = "A";
               if(scoreToUse / max > aGrade) {
                  letterGrade = "S";
               }
            }
         }
      }
      return letterGrade;
   }

   private float GetMaxScore()
   {
      float max = 0;
      max += (noteHitCounter + noteMissedCounter) * perfectHitMultiplier;
      max += EnemySpawner.Instance.GetTotalSpawned() * enemyKilledMultiplier;
      max += PlayerControl.Instance.GetPlayerMaxHealth() * playerHealthMultiplier;
      max += (noteHitCounter + noteMissedCounter) * highestComboMultiplier;
      if(isBossStage) {
         max += BossController.Instance.GetBossMaxHealth() * bossHealthMultiplier;
      }
      Debug.Log("Max Score: " + max);
      return max;
   }

   private void SetTextFields(float finalScore, string letterGrade)
   {
      maxComboText.text = MAX_COMBO + ComboManager.Instance.GetMaxCombo();
      if(isBossStage) {
         //get boss hp
         var maxHealth = BossController.Instance.GetBossMaxHealth();
         var curHealth = BossController.Instance.GetBossHealth();
         EnemyBossText.text = BOSS_HP + Mathf.FloorToInt((float)curHealth / maxHealth * 100) + "%";
      } else {
         EnemyBossText.gameObject.SetActive(false);
      }
      EnemyKilledText.text = ENEMY_KILLED + enemyKilledCounter;
      perfectText.text = NOTE_PERFECT + perfectHitCounter;
      greatText.text = NOTE_GREAT + greatHitCounter;
      goodText.text = NOTE_GOOD + goodHitCounter;
      noteMissText.text = NOTE_MISS + noteMissedCounter;
      playerHealthText.text = PLAYER_HP + PlayerControl.Instance.GetPlayerHealth();
      finalScoreText.text = finalScore.ToString();
      letterGradeText.text = letterGrade;
   }

   private void SaveScore(float finalScore, string letterGrade)
   {
      var score = new ScoreStruct();
      score.score = (int)finalScore;
      score.letterGrade = letterGrade;
      score.maxCombo = ComboManager.Instance.GetMaxCombo().ToString();
      score.playerHP = PlayerControl.Instance.GetPlayerHealth().ToString();
      if(isBossStage) {
         var maxHealth = BossController.Instance.GetBossMaxHealth();
         var curHealth = BossController.Instance.GetBossHealth();
         score.bossHP = Mathf.FloorToInt((float)curHealth / maxHealth * 100) + "%";
      } else {
         score.bossHP = "n/a";
      }
      score.enemyKilled = enemyKilledCounter.ToString();
      float scoreToUse = NoteManager.Instance.GetAccuracyRating();
      var accuracy = Mathf.Round(scoreToUse * 100 * 100) / 100 + "%";
      score.accuracy = accuracy;
      var songName = GameplayManager.Instance.GetSongName();
      var difficulty = GameplayManager.Instance.GetDifficulty();
      if(SaveSystem.TrySaveHighScore(score, songName, difficulty)) {
         newBestText.gameObject.SetActive(true);
      } else {
         newBestText.gameObject.SetActive(false);
      }
   }
}
