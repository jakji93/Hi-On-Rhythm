using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyChargeAttack : EnemyAttack
{
   [SerializeField] private LayerMask playerMask;
   [SerializeField] private int damage;
   [SerializeField] private float chargeSpeed;
   [SerializeField] private float chargeDuration;
   [SerializeField] private Transform visual;
   [SerializeField] private Transform main;
   [SerializeField] private Transform dashParticle;

   private Transform player;

   private bool isCharging = false;
   private Vector3 chargeDirection;

   public override void Attack()
   {
      if (isCharging) return;
      if (player == null) {
         player = PlayerControl.Instance.transform;
      }
      visual.DOShakePosition(0.5f, 0.3f, 100, 90f, false, false).OnComplete(() =>
      {
         chargeDirection = (player.position - main.position).normalized;
         var destination = transform.position + chargeDirection * chargeSpeed * chargeDuration;
         isCharging = true;
         //if (GameplayManager.Instance.UseEffect()) Instantiate(dashParticle, main.position, Quaternion.identity);
         Instantiate(dashParticle, main.position, Quaternion.identity);
         main.DOMove(destination, chargeDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
         {
            isCharging = false;
            EndAttacking();
         });
      });
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (!isCharging) return;
      if (((1 << collision.gameObject.layer) & playerMask) != 0) {
         if (collision.TryGetComponent<Health>(out Health health)) {
            health.TakeDamage(damage);
         }
      }
   }

   private void OnDestroy()
   {
      visual.DOKill();
      main.DOKill();
   }
}
