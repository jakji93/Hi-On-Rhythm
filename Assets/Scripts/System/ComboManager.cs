using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
   public static ComboManager Instance { get; private set; }

   [SerializeField] private int maxComberPerBar = 20;
   [SerializeField] private int maxMultiplier = 4;
   [SerializeField] private TextMeshProUGUI comboCounterText;
   [SerializeField] private TextMeshProUGUI multiplierText;

   private int curCombo = 0;
   private int curMultiplier = 1;
   private int maxCombo = 0;

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
   }

   private void NoteManager_OnNoteMissed(object sender, System.EventArgs e)
   {
      curMultiplier = 1;
      curCombo = 0;
      comboCounterText.text = curCombo.ToString();
      multiplierText.text = curMultiplier.ToString() + "x";
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
      int tempMulti = Mathf.FloorToInt(curCombo / maxComberPerBar) + 1;
      curMultiplier = tempMulti > maxMultiplier ? maxMultiplier : tempMulti;
      //TODO might chage to a progress bar based UI
      comboCounterText.text = curCombo.ToString();
      multiplierText.text = curMultiplier.ToString() + "x";
   }

   public int GetMultiplier()
   {
      return curMultiplier;
   }

   public int GetMaxCombo()
   {
      return maxCombo;
   }
}
