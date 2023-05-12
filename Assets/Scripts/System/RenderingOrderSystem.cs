using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingOrderSystem : MonoBehaviour
{
   [SerializeField] private SpriteRenderer spriteRenderer;
   [SerializeField] private Transform parentGameObject;

   private void Update()
   {
      spriteRenderer.sortingOrder = (int)(parentGameObject.position.y * -100);
   }
}
