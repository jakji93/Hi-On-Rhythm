using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotate : MonoBehaviour
{
   public float radius = 200f;

   public float targetAngle = 0f; // The target angle in degrees
   public float rotationSpeed = 10f; // The rotation speed in degrees per second
   public AnimationCurve rotationCurve;
   public SongSelector[] songSelectors;
   public AudioSource audioSource;

   private Quaternion targetRotation; // The target rotation quaternion
   private Quaternion initialRotation;
   private float elapsedTime;

   private int curItem = 0;
   private float angle;
   private int numOfChild;
   private bool isRotating;

   private void Start()
   {
      // Get all child game objects
      numOfChild = songSelectors.Length;
      // Calculate the angle between each object
      angle = 360f / numOfChild;

      // Place each object in a circle
      for (int i = 0; i < songSelectors.Length; i++) {
         float xPos = Mathf.Sin(Mathf.Deg2Rad * (i * angle)) * radius;
         float yPos = Mathf.Cos(Mathf.Deg2Rad * (i * angle)) * radius;
         songSelectors[i].transform.position = transform.position + new Vector3(xPos, yPos, 0f);
         songSelectors[i].transform.Rotate(new Vector3(0f, 0f, -i * angle));
      }
      initialRotation = transform.rotation;
      //targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
      targetRotation = transform.rotation;
      isRotating = false;
      PlayCurrentSong();
   }

   private void Update()
   {
      // Rotate towards the target rotation
      //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
      // Update the elapsed time
      elapsedTime += Time.deltaTime;

      // Calculate the normalized time based on the rotation speed
      float normalizedTime = elapsedTime / rotationSpeed;

      // Evaluate the animation curve to get the rotation interpolation factor
      float curveValue = rotationCurve.Evaluate(normalizedTime);

      // Perform the rotation interpolation
      transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, curveValue);

      // Check if the rotation is complete
      if (normalizedTime >= 1f) {
         // Reset the elapsed time and finish the rotation
         elapsedTime = 0f;
         transform.rotation = targetRotation;
         initialRotation = transform.rotation;
         isRotating = false;
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
      PlayCurrentSong();
   }

   public void PrevItem()
   {
      if (isRotating) return;
      curItem--;
      if(curItem < 0) curItem = numOfChild - 1;
      elapsedTime = 0f;
      targetRotation = Quaternion.Euler(0f, 0f, curItem * angle);
      isRotating = true;
      PlayCurrentSong();
   }

   public void PlayCurrentSong()
   {
      audioSource.Stop();
      audioSource.clip = songSelectors[curItem].GetAudioClip();
      audioSource.Play();
   }
}
