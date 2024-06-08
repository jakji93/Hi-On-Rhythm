using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static BossMovementOnBeat;

public class BossAttackOnBeat : MonoBehaviour
{
   [Serializable]
   public class BossAttacks
   {
      public GameObject attackObject;
      public bool attachToHost = true;
      public bool spawnAtPlayer = false;
      public bool spawnAtPosition = false;
      public Transform position;
   }

   [Serializable]
   public class BossAttackPattern
   {
      public BossAttacks[] attacks;
      public float musicTime;
   }

   [Serializable]
   public class SpecificAttackPattern
   {
      public BossAttacks attack;
      public int waveNumber;
   }

   [SerializeField] private BossAttackPattern[] attackPatterns;
   [SerializeField] private SpecificAttackPattern[] specificAttackPatterns;

   private int waveCounter = 0;
   private int globalWaveCounter = 0;
   private BossAttackPattern curPattern;

   private void Start()
   {
      NoteManager.Instance.OnAttackBeat += Instance_OnAttackBeat;
   }

   private void Instance_OnAttackBeat(object sender, System.EventArgs e)
   {
      UpdateCurSet();
      Attack();
   }

   private void Attack()
   {
      if (curPattern == null) return;
      var curWaveNum = waveCounter % curPattern.attacks.Length;
      var curAttack = curPattern.attacks[curWaveNum];

      foreach (var pattern in specificAttackPatterns) {
         if (globalWaveCounter + 1 == pattern.waveNumber) {
            curAttack = pattern.attack;
         }
      }
      if(curAttack.attachToHost) {
         Instantiate(curAttack.attackObject, transform.position, Quaternion.identity, transform);
      } else if(curAttack.spawnAtPlayer) {
         Instantiate(curAttack.attackObject, PlayerControl.Instance.transform.position, Quaternion.identity);
      } else if(curAttack.spawnAtPosition) {
         Instantiate(curAttack.attackObject, curAttack.position.position, Quaternion.identity);
      }
      waveCounter++;
      globalWaveCounter++;
   }

   private void UpdateCurSet()
   {
      if (curPattern == null) return;
      var playtime = MusicManager.Instance.GetGameMusicPlaytime();
      if (playtime > curPattern.musicTime) {
         foreach (var set in attackPatterns) {
            if (playtime < set.musicTime) {
               curPattern = set;
               waveCounter = 0;
               return;
            }
         }
         curPattern = attackPatterns[attackPatterns.Length - 1];
      }
   }
}
