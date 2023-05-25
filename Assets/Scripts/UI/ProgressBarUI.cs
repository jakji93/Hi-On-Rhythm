using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
   [SerializeField] private GameObject hasProgressObject;
   [SerializeField] private Image barImage;
   [SerializeField] private bool isDepleting = false;
   [SerializeField] private bool hideAtStart = false;

   private IHasProgress hasProgress;

   private void Start()
   {
      hasProgress = hasProgressObject.GetComponent<IHasProgress>();
      if(hasProgress == null) {
         Debug.LogError("Object missing IHasProgress");
      }

      hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
      barImage.fillAmount = isDepleting ? 1f : 0f;
      if(hideAtStart) {
         Hide();
      }
   }

   private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
   {
      barImage.fillAmount = e.currentValue / e.maxValue;
      if(hideAtStart && (e.currentValue == 0 || e.currentValue == 1)) {
         Hide();
      } else {
         Active();
      }
   }

   private void Hide()
   {
      gameObject.SetActive(false);
   }

   private void Active()
   {
      gameObject.SetActive(true);
   }
}
