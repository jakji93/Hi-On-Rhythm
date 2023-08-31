using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class EnemySummonAnimation : MonoBehaviour
{
   [SerializeField] private Transform visual;
   [SerializeField] private SpriteRenderer spriteRenderer;

   private void Start()
   {
      Summon();
   }

   private async void Summon()
   {
      var tasks = new List<Task>();
      tasks.Add(visual.DOScale(new Vector3(4f, 4f, 4f), 0.4f).AsyncWaitForCompletion());
      tasks.Add(spriteRenderer.DOFade(0f, 0.4f).AsyncWaitForCompletion());
      await Task.WhenAll(tasks);
      Destroy(gameObject);
   }
}
