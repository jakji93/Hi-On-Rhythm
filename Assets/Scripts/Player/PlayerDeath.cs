using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
   [SerializeField] private Health health;
   [SerializeField] private MMF_Player DeathZoomIn;
   [SerializeField] private MMF_Player DeathZoomOut;
   [SerializeField] private Transform body;
   [SerializeField] private GameObject deathParticle;

   private void Start()
   {
      health.OnDeath += Health_OnDeath;
   }

   private void Health_OnDeath(object sender, Health.OnTakeDamageEventArgs e)
   {
      DeathZoomIn.PlayFeedbacks();
      body.DOKill();
      body.DOShakePosition(2f, 0.3f, 100, 90f, false, false).OnComplete(() =>
      {
         gameObject.SetActive(false);
         DeathZoomIn.StopFeedbacks();
         DeathZoomOut.PlayFeedbacks();
         Instantiate(deathParticle, transform.position, Quaternion.identity);
      });
   }
}
