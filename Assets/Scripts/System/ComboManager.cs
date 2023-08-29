using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ComboManager : MonoBehaviour
{
   public static ComboManager Instance { get; private set; }

   [SerializeField] private int maxComberPerBar = 20;
   [SerializeField] private int maxMultiplier = 4;
   [SerializeField] private TextMeshProUGUI comboCounterText;
   [SerializeField] private CanvasGroup comboCounterCanvasGroup;
   [SerializeField] private TextMeshProUGUI multiplierText;

   private int curCombo = 0;
   private int curMultiplier = 1;
   private int maxCombo = 0;

   private int killCombo = 0;

   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      NoteManager.Instance.OnNormal1Hit += NoteManager_OnNormal1Hit;
      NoteManager.Instance.OnNormal2Hit += NoteManager_OnNormal2Hit;
      NoteManager.Instance.OnSpecialHit += NoteManager_OnSpecialHit;
      NoteManager.Instance.OnNoteMissed += NoteManager_OnNoteMissed;
      NoteManager.Instance.OnWrongNote += NoteManager_OnWrongNote;
      comboCounterCanvasGroup.alpha = 0;
   }

   private void NoteManager_OnWrongNote(object sender, System.EventArgs e)
   {
      ResetCombo();
   }

   private void NoteManager_OnNoNoteHits(object sender, System.EventArgs e)
   {
      ResetCombo();
   }

   private void NoteManager_OnNoteMissed(object sender, System.EventArgs e)
   {
      ResetCombo();
   }

   private void NoteManager_OnSpecialHit(object sender, System.EventArgs e)
   {
      UpdateCombo();
   }

   private void NoteManager_OnNormal2Hit(object sender, System.EventArgs e)
   {
      UpdateCombo();
   }

   private void NoteManager_OnNormal1Hit(object sender, System.EventArgs e)
   {
      UpdateCombo();
   }

   private void UpdateCombo()
   {
      curCombo++;
      if (curCombo > maxCombo) maxCombo = curCombo;
      if(curCombo == 5) {
         comboCounterCanvasGroup.DOFade(1, 0.2f).From(0.5f).SetEase(Ease.Linear);
      }
      if(curCombo > 5) {
         comboCounterText.transform.DOKill();
         comboCounterText.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f, 0, 0);
      }
      comboCounterText.text = curCombo.ToString();
   }

   private void ResetCombo()
   {
      curMultiplier = 1;
      curCombo = 0;
      comboCounterCanvasGroup.DOKill();
      comboCounterCanvasGroup.alpha = 0;
      comboCounterText.text = curCombo.ToString();
   }

   public int GetMultiplier()
   {
      return curMultiplier;
   }

   public int GetMaxCombo()
   {
      return maxCombo;
   }

   public void EnemyKilled()
   {
      killCombo++;
      if(killCombo >= maxComberPerBar) {
         killCombo = 0;
         curMultiplier++;
         curMultiplier = Mathf.Min(curMultiplier, maxMultiplier);
         multiplierText.text = curMultiplier.ToString() + "x";
      }
   }

   public void GotHit()
   {
      if (curMultiplier > 1) curMultiplier--;
      multiplierText.text = curMultiplier.ToString() + "x";
   }
}
