using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AccuracyManager : MonoBehaviour
{
   public static AccuracyManager Instance { get; private set; }

   [SerializeField] private CanvasGroup missText;
   [SerializeField] private CanvasGroup perfectText;
   [SerializeField] private CanvasGroup greatText;
   [SerializeField] private CanvasGroup goodText;

   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      NoteManager.Instance.OnNoteMissed += NoteManager_OnNoteMissed;
      NoteManager.Instance.OnNoteGood += NoteManager_OnNoteGood;
      NoteManager.Instance.OnNoteGreat += NoteManager_OnNoteGreat;
      NoteManager.Instance.OnNotePerfect += NoteManager_OnNotePerfect;
   }

   private void NoteManager_OnNotePerfect(object sender, System.EventArgs e)
   {
      TextAnimation(perfectText);
   }

   private void NoteManager_OnNoteGreat(object sender, System.EventArgs e)
   {
      TextAnimation(greatText);
   }

   private void NoteManager_OnNoteGood(object sender, System.EventArgs e)
   {
      TextAnimation(goodText);
   }

   private void NoteManager_OnNoteMissed(object sender, System.EventArgs e)
   {
      TextAnimation(missText);
   }

   private void TextAnimation(CanvasGroup textCanvas)
   {
      textCanvas.DOKill();
      textCanvas.transform.DOKill();
      textCanvas.alpha = 1;
      textCanvas.transform.localPosition = Vector3.zero;
      textCanvas.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f, 0, 0).OnComplete(() =>
      {
         textCanvas.transform.DOLocalMoveY(-20f, 1f);
         textCanvas.DOFade(0, 1f);
      });
   }
}
