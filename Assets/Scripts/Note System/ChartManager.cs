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
   private Vector3 basePosition;
   private Transform thisTransform;
   private float offset = 0f;

   private void Awake()
   {
      Instance = this;
      chartSpeed = BPM / 60 * BPM_MULTIPLIER;
      thisTransform = transform;
      basePosition = thisTransform.localPosition;
   }

   private void Start()
   {
      if (PlayerPrefs.HasKey("ChartOffset")) {
         offset = PlayerPrefs.GetFloat("ChartOffset");
      } else {
         offset = 0f;
      }
      Debug.Log("Offset: " + offset);
      BPM = SongLoader.Instance.GetBPM();
      Instantiate(SongLoader.Instance.GetChart(), transform);
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
         //var postionAtPlaytime = MusicManager.Instance.GetGameMusicPlaytime() * BPM / 60 * BPM_MULTIPLIER;
         //thisTransform.localPosition = new Vector3(basePosition.x - postionAtPlaytime, basePosition.y, 0);
         //thisTransform.position -= new Vector3(chartSpeed * Time.fixedDeltaTime, 0, 0);
      }
   }

   private void Update()
   {
      if (isPlaying) {
         var postionAtPlaytime = (MusicManager.Instance.GetGameMusicPlaytime() + offset) * BPM / 60 * BPM_MULTIPLIER;
         thisTransform.localPosition = new Vector3(basePosition.x - postionAtPlaytime, basePosition.y, 0);
         //thisTransform.position -= new Vector3(chartSpeed * Time.deltaTime, 0, 0);
      }
   }
}
