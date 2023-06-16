using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSelector : MonoBehaviour
{
   [SerializeField] private float radius = 200f;

   [SerializeField] private float rotationSpeed = 10f;
   [SerializeField] private AnimationCurve rotationCurve;
   [SerializeField] private SongItems[] songSelectors;

   private Quaternion targetRotation;
   private Quaternion initialRotation;
   private float elapsedTime;

   private int curItem = 0;
   private float angle;
   private int numOfChild;
   private bool isRotating;

   private void Start()
   {
      numOfChild = songSelectors.Length;
      angle = 360f / numOfChild;

      for (int i = 0; i < songSelectors.Length; i++) {
         //0 at left
         float yPos = Mathf.Sin(Mathf.Deg2Rad * (i * angle)) * radius;
         float xPos = -Mathf.Cos(Mathf.Deg2Rad * (i * angle)) * radius;
         //0 at top
         //float xPos = Mathf.Sin(Mathf.Deg2Rad * (i * angle)) * radius;
         //float yPos = Mathf.Cos(Mathf.Deg2Rad * (i * angle)) * radius;
         songSelectors[i].transform.position = transform.position + new Vector3(xPos, yPos, 0f);
         songSelectors[i].transform.Rotate(new Vector3(0f, 0f, -i * angle));
      }
      initialRotation = transform.rotation;
      targetRotation = transform.rotation;
      isRotating = false;
      //warm up
      for (int i = 0; i < songSelectors.Length; i++) {
         songSelectors[i].GetAudioClip().LoadAudioData();
      }
   }

   private void Update()
   {
      if (isRotating == true) {
         elapsedTime += Time.deltaTime;
         float normalizedTime = elapsedTime / rotationSpeed;
         float curveValue = rotationCurve.Evaluate(normalizedTime);
         transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, curveValue);

         if (normalizedTime >= 1f) {
            elapsedTime = 0f;
            transform.rotation = targetRotation;
            initialRotation = transform.rotation;
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
      curItem++;
      curItem %= numOfChild;
      elapsedTime = 0f;
      targetRotation = Quaternion.Euler(0f, 0f, curItem * angle);
      isRotating = true;
      SetAsCurrentTrack();
   }

   public void PrevItem()
   {
      if (isRotating) return;
      curItem--;
      if (curItem < 0) curItem = numOfChild - 1;
      elapsedTime = 0f;
      targetRotation = Quaternion.Euler(0f, 0f, curItem * angle);
      isRotating = true;
      SetAsCurrentTrack();
   }

   public void SetAsCurrentTrack()
   {
      LevelSelectManager.Instance.SetSongName(songSelectors[curItem].GetSongNames());
      LevelSelectManager.Instance.PlayThisSong(songSelectors[curItem].GetAudioClip());
   }

   public void SetAsCurrentTrack(int songIndex)
   {
      curItem = songIndex;
      transform.rotation = Quaternion.Euler(0f, 0f, curItem * angle);
      SetAsCurrentTrack();
   }

   public int GetCurrentSongIndex()
   {
      return curItem;
   }
}