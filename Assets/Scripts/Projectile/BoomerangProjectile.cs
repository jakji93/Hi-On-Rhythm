using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;

public class BoomerangProjectile : MonoBehaviour
{
   private enum State
   {
      Flyout,
      Sustain,
      Return
   }

   [SerializeField] private int damage;
   [SerializeField] private float flyoutSpeed;
   [SerializeField] private float returnSpeed;
   [SerializeField] private float flyoutTime;
   [SerializeField] private float sustainTime;
   [SerializeField] private float returnLifeTime;
   [SerializeField] private float attackSpeed;
   [SerializeField] protected LayerMask targetMask;
   [SerializeField] protected Transform attackPoint;
   [SerializeField] protected float attackRadius;
   [SerializeField] protected GameObject destroyObject;

   private Transform baseTransform;
   private float attackTimer = 0f;
   private float timer = 0f;
   private State state;

   private void Start()
   {
      attackTimer = attackSpeed;
      state = State.Flyout;
      var destination = transform.position + transform.right * flyoutSpeed * flyoutTime;
      transform.DOMove(destination, flyoutTime).SetEase(Ease.InOutQuad).OnComplete(() =>
      {
         state = State.Sustain;
      });
   }

   private void Update()
   {
      switch(state) {
         case State.Flyout:
            break;
         case State.Sustain:
            timer += Time.deltaTime;
            if(timer > sustainTime) {
               state = State.Return;
               var direction = transform.right * -1;
               if (baseTransform != null) direction = (baseTransform.position - transform.position).normalized;
               var destination = transform.position + direction * returnSpeed * returnLifeTime;
               transform.DOMove(destination, returnLifeTime).SetEase(Ease.Linear).OnComplete(() =>
               {
                  if (destroyObject != null) {
                     Instantiate(destroyObject, transform.position, Quaternion.identity);
                  }
                  Destroy(gameObject);
               });
            }
            break;
         case State.Return:
            break;
      }
      attackTimer += Time.deltaTime;
      if(attackTimer > attackSpeed) {
         attackTimer = 0f;
         var hitInfos = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, targetMask);
         foreach (var hitInfo in hitInfos) {
            if (hitInfo.TryGetComponent(out Health health)) {
               health.TakeDamage(damage * ComboManager.Instance.GetMultiplier());
            }
         }
      }
   }

   private void OnDrawGizmosSelected()
   {
      if (attackPoint != null) {
         Gizmos.color = Color.red;
         Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
      }
   }

   public void SetTransform(Transform targetTransform)
   {
      baseTransform = targetTransform;
   }
}
