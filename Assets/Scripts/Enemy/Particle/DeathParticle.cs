using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class DeathParticle : MonoBehaviour
{
   [SerializeField] private Transform visual;
   [SerializeField] private SpriteRenderer face;
   [SerializeField] private SpriteRenderer back;

   private void Start()
   {
      Destroy(gameObject, 0.5f);
   }

   private async void Fade()
   {
      var tasks = new List<Task>();
      tasks.Add(visual.DOLocalMoveY(6f, 0.5f).SetEase(Ease.Linear).AsyncWaitForCompletion());
      tasks.Add(face.DOFade(0f, 0.5f).SetEase(Ease.Linear).AsyncWaitForCompletion());
      tasks.Add(back.DOFade(0f, 0.5f).SetEase(Ease.Linear).AsyncWaitForCompletion());
      await Task.WhenAll(tasks);
      Destroy(gameObject);
   }
}
