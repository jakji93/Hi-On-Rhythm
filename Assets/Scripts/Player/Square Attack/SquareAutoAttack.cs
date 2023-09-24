using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class SquareAutoAttack : MonoBehaviour
{
   [SerializeField] private Collider2D hitCollider;
   [SerializeField] private ContactFilter2D contactFilter;
   [SerializeField] private int damage;
   [SerializeField] private SpriteRenderer ring;

   private List<Collider2D> colliders = new List<Collider2D>();

   private void Awake()
   {
      transform.parent = PlayerControl.Instance.transform;
      transform.rotation = Quaternion.identity;
   }

   private void Start()
   {
      hitCollider.OverlapCollider(contactFilter, colliders);
      foreach (var collider in colliders) {
         if (collider.gameObject.TryGetComponent(out Health health)) {
            health.TakeDamage(damage);
         }
      }
      FadeRing();
   }

   private async void FadeRing()
   {
      var tasks = new List<Task>();
      tasks.Add(ring.DOFade(0f, 0.3f).SetEase(Ease.Linear).AsyncWaitForCompletion());
      tasks.Add(ring.transform.DOScale(Vector3.zero, 0.3f).From().SetEase(Ease.Linear).AsyncWaitForCompletion());
      await Task.WhenAll(tasks);
      Destroy(gameObject);
   }
}
