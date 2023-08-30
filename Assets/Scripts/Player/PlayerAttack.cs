using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
   [SerializeField] private GameObject[] normalAttack1;
   [SerializeField] private GameObject[] specialAttacks;
   [SerializeField] private GameObject perfectAttack;
   [SerializeField] private GameObject greatAttack;
   [SerializeField] private GameObject goodAttack;

   [SerializeField] private GameObject autoAttack;
   [SerializeField] private int bpm;

   [SerializeField] private Transform aimTransform;

   private int curSpecial = 0;
   private float attackSpeed;
   private float timer = 0f;
   private bool canAuto = false;

   private bool isFeverMode = false;

   private void Start()
   {
      //NoteManager.Instance.OnNormal1Hit += NoteManager_OnNormal1Hit;
      GameplayManager.Instance.OnFirstBeat += GameManager_OnFirstBeat;
      GameplayManager.Instance.OnStateChange += GameManager_OnStateChange;
      FeverManager.Instance.OnFeverModeChanged += FaverMode_OnFeverModeChanged;
      NoteManager.Instance.OnNotePerfect += NoteMnager_OnNotePerfect;
      NoteManager.Instance.OnNoteGreat += NoteManager_OnNoteGreat;
      NoteManager.Instance.OnNoteGood += NoteManager_OnNoteGood;
      attackSpeed = 60f / bpm;
      timer = attackSpeed;
   }

   private void NoteManager_OnNoteGood(object sender, System.EventArgs e)
   {
      if (isFeverMode) {
         var thisAttack = specialAttacks[curSpecial];
         Attack(thisAttack);
         curSpecial++;
         curSpecial %= specialAttacks.Length;
      } else {
         Attack(goodAttack);
      }
   }

   private void NoteManager_OnNoteGreat(object sender, System.EventArgs e)
   {
      if (isFeverMode) {
         var thisAttack = specialAttacks[curSpecial];
         Attack(thisAttack);
         curSpecial++;
         curSpecial %= specialAttacks.Length;
      }
      else {
         Attack(greatAttack);
      }
   }

   private void NoteMnager_OnNotePerfect(object sender, System.EventArgs e)
   {
      if (isFeverMode) {
         var thisAttack = specialAttacks[curSpecial];
         Attack(thisAttack);
         curSpecial++;
         curSpecial %= specialAttacks.Length;
      }
      else {
         Attack(perfectAttack);
      }
   }

   private void FaverMode_OnFeverModeChanged(object sender, FeverManager.OnFeverModeEventArgs e)
   {
      isFeverMode = e.isFeverMode;
      curSpecial = 0;
   }

   private void GameManager_OnStateChange(object sender, System.EventArgs e)
   {
      if(!GameplayManager.Instance.IsGamePlaying()) {
         canAuto = false;
      }
   }

   private void GameManager_OnFirstBeat(object sender, System.EventArgs e)
   {
      //canAuto = true;
   }

   private void NoteManager_OnNormal1Hit(object sender, System.EventArgs e)
   {
      if (isFeverMode) {
         var thisAttack = specialAttacks[curSpecial];
         Attack(thisAttack);
         curSpecial++;
         curSpecial %= specialAttacks.Length;
      } else {
         var attackIndex = ComboManager.Instance.GetMultiplier() - 1;
         attackIndex = Mathf.Clamp(attackIndex, 0, normalAttack1.Length - 1);
         var thisAttack = normalAttack1[attackIndex];
         Attack(thisAttack);
      }
   }

   private void Attack(GameObject attack)
   {
      Instantiate(attack, aimTransform.position, aimTransform.rotation, aimTransform.transform);
   }

   private void Update()
   {
      if(canAuto) {
         timer += Time.deltaTime;
         if(timer > attackSpeed) {
            Instantiate(autoAttack, aimTransform.position, aimTransform.rotation, aimTransform.transform);
            timer = 0;
         }
      }
   }
}
