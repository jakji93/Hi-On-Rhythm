using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
   public static CameraManager Instance { get; private set; }
   [SerializeField] private float globalShakeForce = 1f;

   private void Awake()
   {
      Instance = this;
   }

   public void ShakeCamera(CinemachineImpulseSource source)
   {
      source.GenerateImpulseWithForce(globalShakeForce);
   }
}
