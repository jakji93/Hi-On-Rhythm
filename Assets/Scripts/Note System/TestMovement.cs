using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
   private const float BPM_MULTIPLIER = 480f;
   private float timeSofar = 0f;
   [SerializeField] private float BPM = 120;
   [SerializeField] private GameInput playerInputs;
   private float chartSpeed;
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
