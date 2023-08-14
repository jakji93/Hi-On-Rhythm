using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
   public enum NoteTypes
   {
      Normal1,
      Normal2,
      Special,
      Attack,
      Spawn,
   }

   [SerializeField] private NoteTypes type;

   public NoteTypes getNoteType() { return type; }

   private void Awake()
   {
      var postion = transform.localPosition;
      postion.z = postion.x / 1000;
      transform.localPosition = postion;
   }
}
