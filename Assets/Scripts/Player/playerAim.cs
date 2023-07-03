using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAim : MonoBehaviour
{
    private Vector2 mousePos;

    // Update is called once per frame
    void Update()
    {
      mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      transform.right = (mousePos - (Vector2)transform.position).normalized;

   }
}
