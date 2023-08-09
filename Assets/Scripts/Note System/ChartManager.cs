using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartManager : MonoBehaviour
{
   public static ChartManager Instance { get; private set; }
   private const float BPM_MULTIPLIER = 480f;

   [SerializeField] private float BPM = 120;

   [SerializeField] private bool isPlaying = false;

   [SerializeField] private RectTransform rectTransform;

   private float chartSpeed;
   private Vector3 basePosition;

   private void Awake()
   {
      Instance = this;
      chartSpeed = BPM / 60 * BPM_MULTIPLIER;
      basePosition = rectTransform.anchoredPosition3D;
   }

   public void StartPlaying()
   {
      isPlaying = true;
   }

   public void StopPlaying() 
   { 
      isPlaying = false; 
   }

   private void FixedUpdate()
   {
      if (isPlaying) {
         var postionAtPlaytime = MusicManager.Instance.GetGameMusicPlaytime() * BPM / 60 * 480;
         rectTransform.anchoredPosition3D = new Vector3(basePosition.x - postionAtPlaytime, basePosition.y, 0);
         //rectTransform.anchoredPosition3D -= new Vector3(chartSpeed * Time.fixedDeltaTime, 0, 0);
      }
   }
}
