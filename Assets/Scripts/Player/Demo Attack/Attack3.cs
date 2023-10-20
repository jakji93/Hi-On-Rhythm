using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3 : MonoBehaviour
{
   [SerializeField] private GameObject bullet;

   // Start is called before the first frame update
   void Start()
   {
      Instantiate(bullet, transform.position, transform.rotation);
      Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 90f));
      Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 180f));
      Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 270f));
      Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 45f));
      Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 135f));
      Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 225f));
      Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 315f));
      Destroy(this.gameObject);
   }
}
