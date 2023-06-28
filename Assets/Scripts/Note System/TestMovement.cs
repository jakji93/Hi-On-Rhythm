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

   private float chartSpeed;
   private Vector3 baseLocation;

   private void Awake()
   {
      baseLocation = transform.localPosition;
      audio.time = playtime;
      var postionAtPlaytime = playtime * BPM / 60 * 480;
      baseLocation.x = baseLocation.x - postionAtPlaytime;
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
      Debug.Log(transform.localPosition.x);
   }

   private void FixedUpdate()
   {
      timeSofar += Time.fixedDeltaTime;
      transform.position -= new Vector3(chartSpeed * Time.fixedDeltaTime, 0, 0);
   }
}
