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
   private const string NOTE_HITS = "NOTE HITS: ";
   private const string NOTE_MISS = "NOTE MISSED: ";
   private const string PLAYER_HP = "PLAYER HP: ";
   public static ScoreManager Instance { get; private set; }

   [SerializeField] private float noteHitMultiplier = 10f;
   [SerializeField] private float noteMissedMultiplier = 15f;
   [SerializeField] private float enemyKilledMultiplier = 20f;
   [SerializeField] private float bossHealthMultiplier = 20f;
   [SerializeField] private float playerHealthMultiplier = 10f;
   [SerializeField] private float highestComboMultiplier = 10f;
   [SerializeField] private bool isBossStage = false;
   [SerializeField] private GameObject scoreBoard;
   [SerializeField] private GameObject failedBoard;

   [Header("Text Fields")]
   [SerializeField] private TextMeshProUGUI songNameText;
   [SerializeField] private TextMeshProUGUI difficultyText;
   [SerializeField] private TextMeshProUGUI maxComboText;
   [SerializeField] private TextMeshProUGUI EnemyBossText;
   [SerializeField] private TextMeshProUGUI noteHitText;
   [SerializeField] private TextMeshProUGUI noteMissText;
   [SerializeField] private TextMeshProUGUI playerHealthText;
   [SerializeField] private TextMeshProUGUI finalScoreText;
   [SerializeField] private TextMeshProUGUI letterGradeText;
   [SerializeField] private TextMeshProUGUI newBestText;

   [Header("Letter Grade Threshold")]
   [SerializeField] private float dGrade = 0.59f;
   [SerializeField] private float cGrade = 0.69f;
   [SerializeField] private float bGrade = 0.79f;
   [SerializeField] private float aGrade = 0.89f;
   [SerializeField] private float maxScore = 10f;

   private int noteHitCounter = 0;
   private int noteMissedCounter = 0;
   private int enemyKilledCounter = 0;

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
      HideScore();
      HideFailed();
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
      SaveScore(finalScore, letterGrade);
   }

   public void ShowFailed()
   {
      ActivateFailed();
   }

   private void HideScore()
   {
      scoreBoard.SetActive(false);
   }

   private void HideFailed()
   {
      failedBoard.SetActive(false);
   }

   private void ActivateScore()
   {
      scoreBoard.SetActive(true);
   }

   private void ActivateFailed()
   {
      failedBoard.SetActive(true);
   }

   private float GetFinalScore()
   {
      float finalScore = 0f;
      finalScore += noteHitCounter * noteHitMultiplier;
      finalScore -= noteMissedCounter * noteMissedMultiplier;
      if (isBossStage) {
         //finalScore += BossHp * BossHPMulti
      }
      else {
         finalScore += enemyKilledCounter * enemyKilledMultiplier;
      }
      finalScore += PlayerControl.Instance.GetPlayerHealth() * playerHealthMultiplier;
      finalScore += ComboManager.Instance.GetMaxCombo() * highestComboMultiplier;
      return finalScore;
   }

   private string GetLetterGrade(float finalScore)
   {
      string letterGrade = "D";
      //Get Boss HP as a % from somewhere
      float scoreToUse = isBossStage ? 0 : finalScore;
      if(scoreToUse / maxScore > dGrade) {
         letterGrade = "C";
         if(scoreToUse / maxScore > cGrade) {
            letterGrade = "B";
            if(scoreToUse /maxScore > bGrade) {
               letterGrade = "A";
               if(scoreToUse / maxScore > aGrade) {
                  letterGrade = "S";
               }
            }
         }
      }
      //if player dead, grade is D no matter the score
      if (PlayerControl.Instance.GetPlayerHealth() <= 0) letterGrade = "D";
      return letterGrade;
   }

   private void SetTextFields(float finalScore, string letterGrade)
   {
      maxComboText.text = MAX_COMBO + ComboManager.Instance.GetMaxCombo();
      if(isBossStage) {
         //get boss hp
         EnemyBossText.text = BOSS_HP + "0%";
      } else {
         EnemyBossText.text = ENEMY_KILLED + enemyKilledCounter;
      }
      noteHitText.text = NOTE_HITS + noteHitCounter;
      noteMissText.text = NOTE_MISS + noteMissedCounter;
      //get player hp, % or not?
      playerHealthText.text = PLAYER_HP + Mathf.FloorToInt((float)PlayerControl.Instance.GetPlayerHealth() / PlayerControl.Instance.GetPlayerMaxHealth() * 100) + "%";
      finalScoreText.text = finalScore.ToString();
      letterGradeText.text = letterGrade;
   }

   private void SaveScore(float finalScore, string letterGrade)
   {
      var score = new ScoreStruct();
      score.score = (int)finalScore;
      score.letterGrade = letterGrade;
      score.maxCombo = ComboManager.Instance.GetMaxCombo().ToString();
      score.playerHP = Mathf.FloorToInt((float)PlayerControl.Instance.GetPlayerHealth() / PlayerControl.Instance.GetPlayerMaxHealth() * 100) + "%";
      if(isBossStage) {
         score.enemyKilled = "n/a";
         //get boss hp
      } else {
         score.bossHP = "n/a";
         score.enemyKilled = enemyKilledCounter.ToString();
      }
      var songName = GameplayManager.Instance.GetSongNames();
      var difficulty = GameplayManager.Instance.GetDifficulties();
      if(SaveSystem.Instance.TrySaveHighScore(score, songName, difficulty)) {
         newBestText.gameObject.SetActive(true);
      } else {
         newBestText.gameObject.SetActive(false);
      }
   }
}
