using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
   [SerializeField] private GameObject[] specialAttacks;
   [SerializeField] private GameObject perfectAttack;
   [SerializeField] private GameObject greatAttack;
   [SerializeField] private GameObject goodAttack;

   [SerializeField] private Transform aimTransform;

   private int curSpecial = 0;

   private bool isFeverMode = false;

   private void Start()
   {
      FeverManager.Instance.OnFeverModeChanged += FaverMode_OnFeverModeChanged;
      NoteManager.Instance.OnNotePerfect += NoteMnager_OnNotePerfect;
      NoteManager.Instance.OnNoteGreat += NoteManager_OnNoteGreat;
      NoteManager.Instance.OnNoteGood += NoteManager_OnNoteGood;
   }

   private void NoteManager_OnNoteGood(object sender, System.EventArgs e)
   {
      if (isFeverMode) {
         var thisAttack = specialAttacks[curSpecial];
         Attack(thisAttack);
         curSpecial++;
         curSpecial %= specialAttacks.Length;
      }
      Attack(goodAttack);
   }

   private void NoteManager_OnNoteGreat(object sender, System.EventArgs e)
   {
      if (isFeverMode) {
         var thisAttack = specialAttacks[curSpecial];
         Attack(thisAttack);
         curSpecial++;
         curSpecial %= specialAttacks.Length;
      }
      Attack(greatAttack);
   }

   private void NoteMnager_OnNotePerfect(object sender, System.EventArgs e)
   {
      if (isFeverMode) {
         var thisAttack = specialAttacks[curSpecial];
         Attack(thisAttack);
         curSpecial++;
         curSpecial %= specialAttacks.Length;
      }
      Attack(perfectAttack);
   }

   private void FaverMode_OnFeverModeChanged(object sender, FeverManager.OnFeverModeEventArgs e)
   {
      isFeverMode = e.isFeverMode;
      curSpecial = 0;
   }

   private void Attack(GameObject attack)
   {
      Instantiate(attack, aimTransform.position, aimTransform.rotation, aimTransform.transform);
   }
}
