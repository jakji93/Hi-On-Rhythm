using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
   [SerializeField] private GameObject normalAttack1;
   [SerializeField] private GameObject normalAttack2;
   [SerializeField] private GameObject[] specialAttacks;

   [SerializeField] private Transform aimTransform;

   private int curSpecial = 0;

   private void Start()
   {
      NoteManager.Instance.OnNormal1Hit += NoteManager_OnNormal1Hit;
      NoteManager.Instance.OnNormal2Hit += NoteManager_OnNormal2Hit;
      NoteManager.Instance.OnSpecialHit += NoteManager_OnSpecialHit;
   }

   private void NoteManager_OnSpecialHit(object sender, System.EventArgs e)
   {
      var thisAttack = specialAttacks[curSpecial];
      Attack(thisAttack);
      curSpecial++;
      curSpecial %= specialAttacks.Length;
   }

   private void NoteManager_OnNormal2Hit(object sender, System.EventArgs e)
   {
      Attack(normalAttack2);
   }

   private void NoteManager_OnNormal1Hit(object sender, System.EventArgs e)
   {
      Attack(normalAttack1);
   }

   private void Attack(GameObject attack)
   {
      //Quaternion rotato = aimTransform.rotation;
      //Vector3 rot = rotato.eulerAngles;
      //rotato = Quaternion.Euler(rot);

      //Instantiate(attack, aimTransform.position, rotato, gameObject.transform);
      Instantiate(attack, aimTransform.position, aimTransform.rotation, aimTransform.transform);
   }
}
