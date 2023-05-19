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
   }

   [SerializeField] private NoteTypes type;

   public NoteTypes getNoteType() { return type; }
}
