using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
   private const float BPM_MULTIPLIER = 480f;
   private float timeSofar = 0f;
   [SerializeField] private float BPM = 120;
   [SerializeField] private GameInput playerInputs;
   [SerializeField] private AudioSource audio;
   [SerializeField] private float playtime;
   [SerializeField] private float basePostionX;
   [SerializeField] private bool isCharting;

   [SerializeField] private RectTransform rectTransform;

   private float chartSpeed;
   private Vector3 baseLocation;

   private void Awake()
   {
      baseLocation = transform.localPosition;
      audio.time = playtime;
      var postionAtPlaytime = playtime * BPM / 60 * 480;
      baseLocation.x = basePostionX - postionAtPlaytime;
      transform.localPosition = baseLocation;
   }

   private void Start()
   {
      chartSpeed = BPM / 60 * BPM_MULTIPLIER;
      Debug.Log(transform.localPosition.x);
      playerInputs.OnNormal1Pressed += PlayerInputs_OnNormal1Pressed;
   }

   private void PlayerInputs_OnNormal1Pressed(object sender, System.EventArgs e)
   {
      if(isCharting) Debug.Log(transform.localPosition.x);
   }

   private void FixedUpdate()
   {
      timeSofar += Time.fixedDeltaTime;
      var postionAtPlaytime = audio.time * BPM / 60 * 480;
      rectTransform.anchoredPosition3D = new Vector3(basePostionX - postionAtPlaytime, baseLocation.y, 0);
      //rectTransform.anchoredPosition3D -= new Vector3(chartSpeed * Time.fixedDeltaTime, 0, 0);
   }
}
