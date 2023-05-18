using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    private NoteType[] musicChart;
    private float waitTime =0f;
    public int timer = 0;


    // Start is called before the first frame update
    void Start()
    {
        musicChart = getMusic();

        

    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime < 3f)
        {
            waitTime += Time.deltaTime;

        }
        else
        {
            waitTime = 0f;
            timer++;
            print(timer.ToString());

          
            print(musicChart[timer].getNotePos().ToString());
            print(musicChart[timer].getNoteType().ToString());


        }

    }

    NoteType grabNote(NoteType note)
    {
        note = null;

        return (null);
    }

    NoteType[] getMusic(/*musicfiletype*/)
    {
        //this will eventuall read from a file

        int randMusicLength = UnityEngine.Random.Range(5, 10);
        NoteType[] chart = new NoteType[randMusicLength]; //random music length for now


        for(int i = 0; i< chart.Length; i++)
        {
            GameObject randNote = new GameObject("note"+ i.ToString() );
            randNote.AddComponent<NoteType>();
            

            int randType = UnityEngine.Random.Range(0,3);
            
            if (randType == 0)
            {
                randNote.GetComponent<NoteType>().setNoteType(NoteType.NoteTypes.Normal1);
                randNote.GetComponent<NoteType>().setNotePosition(i);
                chart[i] = randNote.GetComponent<NoteType>();
            }

            if (randType == 1)
            {
                randNote.GetComponent<NoteType>().setNoteType(NoteType.NoteTypes.Normal2);
                randNote.GetComponent<NoteType>().setNotePosition(i);
                chart[i] = randNote.GetComponent<NoteType>();
            }

            if (randType == 2)
            {
                randNote.GetComponent<NoteType>().setNoteType(NoteType.NoteTypes.Special);
                randNote.GetComponent<NoteType>().setNotePosition(i);
                chart[i] = randNote.GetComponent<NoteType>();
            }

            if (randType == 3)
            {
                randNote.GetComponent<NoteType>().setNoteType(NoteType.NoteTypes.Empty);
                randNote.GetComponent<NoteType>().setNotePosition(i);
                chart[i] = randNote.GetComponent<NoteType>();
            }
        }




        
        return (chart);
    }
}
