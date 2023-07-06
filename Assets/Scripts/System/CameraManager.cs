using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
   public static CameraManager Instance { get; private set; }
   [SerializeField] private CinemachineVirtualCamera virtualCamera;
   [SerializeField] private float globalShakeForce = 1f;

   private CinemachineBasicMultiChannelPerlin perlin;
   float timer = 0f;
   private Vector3 basedLocation;
   float timeTotal = 0f;
   float startIntensity = 0;

   private void Awake()
   {
      Instance = this;
      //perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
      basedLocation = transform.position;
   }

   private void Start()
   {
      StopShake();
   }

   public void ShakeCamera(float intensity, float time)
   {
      startIntensity = intensity;
      timer = time;
      timeTotal = time;
   }

   private void StopShake()
   {
      //perlin.m_AmplitudeGain = 0;
      timer = 0;
      transform.position = basedLocation;
   }

   private void Update()
   {
      if (timer > 0f) {
         timer -= Time.deltaTime;
         //perlin.m_AmplitudeGain = Mathf.Lerp(startIntensity, 0f, 1 - (timer / timeTotal));
         if (timer <= 0f) {
            StopShake() ;
         }
      }
   }
}
