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
   [SerializeField] private SpriteRenderer top;
   [SerializeField] private SpriteRenderer left;
   [SerializeField] private SpriteRenderer bottom;
   [SerializeField] private SpriteRenderer right;

   private List<Collider2D> colliders = new List<Collider2D>();

   private void Awake()
   {
      transform.parent = PlayerControl.Instance.transform;
      transform.rotation = Quaternion.identity;
   }

   private void Start()
   {
      Fade();
      hitCollider.OverlapCollider(contactFilter, colliders);
      foreach (var collider in colliders) {
         if (collider.gameObject.TryGetComponent(out Health health)) {
            health.TakeDamage(damage);
         }
      }
   }

   private async void Fade()
   {
      var tasks = new List<Task>();
      tasks.Add(top.DOFade(0f, 0.3f).From().SetEase(Ease.Linear).AsyncWaitForCompletion());
      tasks.Add(left.DOFade(0f, 0.3f).From().SetEase(Ease.Linear).AsyncWaitForCompletion());
      tasks.Add(right.DOFade(0f, 0.3f).From().SetEase(Ease.Linear).AsyncWaitForCompletion());
      tasks.Add(bottom.DOFade(0f, 0.3f).From().SetEase(Ease.Linear).AsyncWaitForCompletion());
      tasks.Add(top.transform.DOScale(Vector3.zero, 0.3f).From().SetEase(Ease.Linear).AsyncWaitForCompletion());
      tasks.Add(left.transform.DOScale(Vector3.zero, 0.3f).From().SetEase(Ease.Linear).AsyncWaitForCompletion());
      tasks.Add(right.transform.DOScale(Vector3.zero, 0.3f).From().SetEase(Ease.Linear).AsyncWaitForCompletion());
      tasks.Add(bottom.transform.DOScale(Vector3.zero, 0.3f).From().SetEase(Ease.Linear).AsyncWaitForCompletion());
      tasks.Add(top.transform.DOLocalMove(Vector3.zero, 0.3f).From().SetEase(Ease.Linear).AsyncWaitForCompletion());
      tasks.Add(left.transform.DOLocalMove(Vector3.zero, 0.3f).From().SetEase(Ease.Linear).AsyncWaitForCompletion());
      tasks.Add(right.transform.DOLocalMove(Vector3.zero, 0.3f).From().SetEase(Ease.Linear).AsyncWaitForCompletion());
      tasks.Add(bottom.transform.DOLocalMove(Vector3.zero, 0.3f).From().SetEase(Ease.Linear).AsyncWaitForCompletion());
      await Task.WhenAll(tasks);
      Destroy(gameObject);
   }
}
