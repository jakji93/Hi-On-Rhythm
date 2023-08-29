using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSpecial1 : MonoBehaviour
{
   [SerializeField] private int damage;
   [SerializeField] private float interval;
   [SerializeField] private float lifetime;
   [SerializeField] private Collider2D attackCollider;
   [SerializeField] private ContactFilter2D contactFilter2D;

   private List<Collider2D> colliders = new List<Collider2D>();
   private float timer;

   private void Start()
   {
      transform.parent = PlayerControl.Instance.transform;
      transform.rotation = Quaternion.identity;
      timer = interval;
      Destroy(gameObject, lifetime);
   }

   private void Update()
   {
      timer += Time.deltaTime;
      if (timer > interval) {
         attackCollider.OverlapCollider(contactFilter2D, colliders);
         foreach (var collider in colliders) {
            if (collider.gameObject.TryGetComponent(out Health health)) {
               health.TakeDamage(damage);
            }
         }
         timer = 0;
      }
   }
}
