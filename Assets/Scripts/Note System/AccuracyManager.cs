using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AccuracyManager : MonoBehaviour
{
   public static AccuracyManager Instance { get; private set; }

   [SerializeField] private CanvasGroup missText;

   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      NoteManager.Instance.OnNoteMissed += NoteManager_OnNoteMissed;
   }

   private void NoteManager_OnNoteMissed(object sender, System.EventArgs e)
   {
      missText.DOKill();
      missText.transform.DOKill();
      missText.alpha = 1;
      missText.transform.localPosition = Vector3.zero;
      missText.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f, 0, 0).OnComplete(() =>
      {
         missText.transform.DOLocalMoveY(-20f, 1f);
         missText.DOFade(0, 1f);
      });
   }
}
