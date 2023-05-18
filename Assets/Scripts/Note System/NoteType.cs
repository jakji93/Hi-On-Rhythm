using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteType : MonoBehaviour
{
    public enum NoteTypes
    {
        Normal1,
        Normal2,
        Special,
        Empty

    }

    [SerializeField] private NoteTypes type;
    [SerializeField] private int position;
    [SerializeField] public int speed;

    public NoteTypes getNoteType() { return type; }

    public void setNoteType(NoteTypes typeType) { this.type = typeType; }
    public void setNotePosition(int typePos) { this.position = typePos; }

    public void setNoteSpeed(int typeSpeed) { this.speed = typeSpeed; }





    public int getNotePos() { return position; }
}
    