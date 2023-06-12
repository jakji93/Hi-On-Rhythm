using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartManager : MonoBehaviour
{
   public static ChartManager Instance { get; private set; }
   private const float BPM_MULTIPLIER = 480f;

   [SerializeField] private float BPM = 120;

   [SerializeField] private bool isPlaying = false;
   private float chartSpeed;

   private void Awake()
   {
      Instance = this;
      chartSpeed = BPM / 60 * BPM_MULTIPLIER;
   }

   public void StartPlaying()
   {
      isPlaying = true;
   }

   public void StopPlaying() 
   { 
      isPlaying = false; 
   }

   private void Update()
   {
      if (isPlaying) {
         transform.position -= new Vector3(chartSpeed * Time.deltaTime, 0, 0);
      }
   }
}
