using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
   public static ScoreManager Instance { get; private set; }

   [SerializeField] private float noteHitMultiplier = 10f;
   [SerializeField] private float noteMissedMultiplier = 15f;
   [SerializeField] private float enemyKilledMultiplier = 20f;
   [SerializeField] private float bossHealthMultiplier = 20f;
   [SerializeField] private float playerHealthMultiplier = 10f;
   [SerializeField] private float highestComboMultiplier = 10f;
   [SerializeField] private bool isBossStage = false;
   [SerializeField] private GameObject scoreBoard;

   private int noteHitCounter = 0;
   private int noteMissedCounter = 0;
   private int enemyKilledCounter = 0;

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
      Hide();
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
      //calculate final score
      float finalScore = 0f;
      finalScore += noteHitCounter * noteHitMultiplier;
      finalScore -= noteMissedCounter * noteMissedMultiplier;
      if (isBossStage) {
         //finalScore += BossHp * BossHPMulti
      } else {
         finalScore += enemyKilledCounter * enemyKilledMultiplier;
      }
      // finalScore += playerHp * playerHpMulti
      // finalScore += highestCombo * comboMulti
      //get final score letter
      //show score screen
      Active();
   }

   private void Hide()
   {
      scoreBoard.SetActive(false);
   }

   private void Active()
   {
      scoreBoard.SetActive(true);
   }
}
