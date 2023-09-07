using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class SquareSpecial2 : MonoBehaviour
{
   [SerializeField] private int damage;
   [SerializeField] private float interval;
   [SerializeField] private float lifetime;
   [SerializeField] private Collider2D attackCollider;
   [SerializeField] private ContactFilter2D contactFilter2D;
   [SerializeField] private float flyoutSpeed;
   [SerializeField] private float flyoutTime;

   [SerializeField] private SpriteRenderer top;
   [SerializeField] private SpriteRenderer bottom;
   [SerializeField] private SpriteRenderer left;
   [SerializeField] private SpriteRenderer right;
   [SerializeField] private AudioSyncerColor topSyncer;
   [SerializeField] private AudioSyncerColor bottomSyncer;
   [SerializeField] private AudioSyncerColor leftSyncer;
   [SerializeField] private AudioSyncerColor rightSyncer;

   private List<Collider2D> colliders = new List<Collider2D>();
   private float timer;

   private void Start()
   {
      timer = interval;
      var destination = transform.position + transform.right * flyoutSpeed * flyoutTime;
      if (!GameplayManager.Instance.UseEffect()) {
         top.gameObject.SetActive(false);
         bottom.gameObject.SetActive(false);
         left.gameObject.SetActive(false);
         right.gameObject.SetActive(false);
      }
      transform.DOMove(destination, flyoutTime).SetEase(Ease.InOutQuad).OnComplete(() =>
      {
         transform.DORotate(Vector3.zero, 0.3f);
         DOVirtual.DelayedCall(lifetime, () =>
         {
            topSyncer.enabled=false;
            bottomSyncer.enabled = false;
            leftSyncer.enabled = false;
            rightSyncer.enabled = false;
            top.DOKill();
            left.DOKill();
            right.DOKill();
            bottom.DOKill();
            top.DOFade(0, 0.5f);
            left.DOFade(0, 0.5f);
            right.DOFade(0, 0.5f);
            bottom.DOFade(0, 0.5f).OnComplete(() =>
            {
               Destroy(gameObject);
            });
         });
      });
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
