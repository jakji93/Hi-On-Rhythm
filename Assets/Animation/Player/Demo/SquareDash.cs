using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class SquareDash : MonoBehaviour
{
   private SpriteRenderer spriteRenderer;
   [SerializeField] private float targetScale = 4f;

   private void Start()
   {
      spriteRenderer = GetComponent<SpriteRenderer>();
      DashAnime();
   }

   private async void DashAnime()
   {
      var tasks = new List<Task>();
      tasks.Add(transform.DOScale(targetScale, 0.5f).SetEase(Ease.OutQuint).AsyncWaitForCompletion());
      tasks.Add(spriteRenderer.DOFade(0, 0.5f).SetEase(Ease.OutQuint).AsyncWaitForCompletion());
      await Task.WhenAll(tasks);
      Destroy(gameObject);
   }
}
