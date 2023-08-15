using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSyncer : MonoBehaviour
{
   [SerializeField] int band;
   [SerializeField] float startScale, scaleMultiplier;

   private void Update()
   {
      transform.localScale = new Vector3(transform.localScale.x, (AudioAnalyzer.Instance.GetBufferBandNormalizedData(band) * scaleMultiplier) + startScale, transform.localScale.z);
   }
}
