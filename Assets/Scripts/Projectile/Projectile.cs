using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
   [SerializeField] protected int damage;
   [SerializeField] protected float speed;
   [SerializeField] protected float lifeTime;
   [SerializeField] protected LayerMask targetMask;
   [SerializeField] protected Transform attackPoint;
   [SerializeField] protected float attackRadius;
   [SerializeField] protected GameObject destroyObject;

   protected void OnDrawGizmosSelected()
   {
      if(attackPoint != null) {
         Gizmos.color = Color.red;
         Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
      }
   }

   private void Start()
   {
      Invoke("DestroyProjectile", lifeTime);
   }

   virtual protected void Update()
   {
      Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
      Vector2 newPosition = currentPosition + (Vector2)transform.right * speed * Time.deltaTime;
      var hitInfos = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, targetMask);
      foreach(var hitInfo in hitInfos) {
         if (hitInfo.TryGetComponent(out Health health)) {
            health.TakeDamage(damage);
         }
         DestroyProjectile();
      }
      transform.position = newPosition;
   }

   protected void DestroyProjectile()
   {
      if (destroyObject != null) {
         Instantiate(destroyObject, transform.position, Quaternion.identity);
      }
      Destroy(gameObject, 2f);
   }
}
