using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSelector : MonoBehaviour
{
   [SerializeField] private float radius = 200f;

   [SerializeField] private float rotationSpeed = 10f;
   [SerializeField] private AnimationCurve rotationCurve;
   [SerializeField] private SongItems[] songSelectors;
   [SerializeField] private SongItemSO[] songItems;

   private Quaternion targetRotation;
   private Quaternion initialRotation;
   private float elapsedTime;

   private int curItem = 0;
   private int curSong = 0;
   private int midIndex = 0;
   private int distance = 0;
   private float angle;
   private int numOfChild;
   private bool isRotating;

   private void Awake()
   {
      numOfChild = songSelectors.Length;
      angle = 360f / numOfChild;
      midIndex = Mathf.FloorToInt(numOfChild / 2f);
      distance = midIndex;
      LoadSongWheel();
   }

   private void Start()
   {
      DisplaySongWheel();
   }

   private void Update()
   {
      if (isRotating == true) {
         elapsedTime += Time.deltaTime;
         float normalizedTime = elapsedTime / rotationSpeed;
         float curveValue = rotationCurve.Evaluate(normalizedTime);
         transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, curveValue);

         if (normalizedTime >= 1f) {
            elapsedTime = 0f;
            transform.localRotation = targetRotation;
            initialRotation = transform.localRotation;
            isRotating = false;
         }
      }
   }


   private void OnDrawGizmos()
   {
      Gizmos.color = Color.green;
      Gizmos.DrawWireSphere(transform.position, radius);
   }

   public void NextItem()
   {
      if (isRotating) return;
      songSelectors[curItem].StopPulse();
      curItem++;
      curItem %= numOfChild;
      var newSongIndex = curSong + distance;
      newSongIndex %= songItems.Length;
      songSelectors[midIndex].LoadSO(songItems[newSongIndex]);
      midIndex++;
      midIndex %= numOfChild;
      curSong++;
      curSong %= songItems.Length;
      elapsedTime = 0f;
      targetRotation = Quaternion.Euler(0f, 0f, curItem * angle);
      isRotating = true;
      SetAsCurrentTrack();
   }

   public void PrevItem()
   {
      if (isRotating) return;
      songSelectors[curItem].StopPulse();
      curItem--;
      if (curItem < 0) curItem = numOfChild - 1;
      var newSongIndex = curSong - (numOfChild - distance);
      while (newSongIndex < 0) {
         newSongIndex += songItems.Length;
      }
      songSelectors[midIndex].LoadSO(songItems[newSongIndex]);
      midIndex--;
      if (midIndex < 0) midIndex = numOfChild - 1;
      curSong--;
      if (curSong < 0) curSong = songItems.Length - 1;
      elapsedTime = 0f;
      targetRotation = Quaternion.Euler(0f, 0f, curItem * angle);
      isRotating = true;
      SetAsCurrentTrack();
   }

   public void SetAsCurrentTrack()
   {
      LevelSelectManager.Instance.SetSongName(songSelectors[curItem].GetSongNames(), songSelectors[curItem].GetDisplayTitle());
      LevelSelectManager.Instance.PlayThisSong(songSelectors[curItem].GetAudioClip());
      songSelectors[curItem].StartPulse();
   }

   public void SetAsCurrentTrack(int songIndex)
   {
      curSong = songIndex;
      numOfChild = songSelectors.Length;
      midIndex = Mathf.FloorToInt(numOfChild / 2f);
      LoadSongWheel();
      SetAsCurrentTrack();
   }

   public int GetCurrentSongIndex()
   {
      return curSong;
   }

   public void DeselectAsTrack()
   {
      songSelectors[curItem].StopPulse();
   }

   private void LoadSongWheel()
   {
      if(songItems.Length == 0) return;
      for (int i = 0; i < midIndex; i++) {
         var songItemIndex = curSong + i;
         songItemIndex %= songItems.Length;
         songSelectors[i].LoadSO(songItems[songItemIndex]);
      }
      for(int i = 0;i < songSelectors.Length - midIndex; i++) {
         var songItemIndex = curSong - 1 - i;
         while(songItemIndex < 0) {
            songItemIndex += songItems.Length;
         }
         songSelectors[songSelectors.Length - 1 - i].LoadSO(songItems[songItemIndex]);
      }
   }

   private void DisplaySongWheel()
   {
      var resolutionScaler = 1f;
      if(Screen.fullScreen) {
         resolutionScaler = Screen.currentResolution.width / 1920;
      }
      for (int i = 0; i < songSelectors.Length; i++) {
         //0 at left
         float yPos = Mathf.Sin(Mathf.Deg2Rad * (i * angle)) * radius * resolutionScaler;
         float xPos = -Mathf.Cos(Mathf.Deg2Rad * (i * angle)) * radius * resolutionScaler;
         //0 at top
         //float xPos = Mathf.Sin(Mathf.Deg2Rad * (i * angle)) * radius;
         //float yPos = Mathf.Cos(Mathf.Deg2Rad * (i * angle)) * radius;
         songSelectors[i].transform.position = transform.position + new Vector3(xPos, yPos, 0f);
         songSelectors[i].transform.Rotate(new Vector3(0f, 0f, -i * angle));
      }
      initialRotation = transform.localRotation;
      isRotating = false;
   }
}
