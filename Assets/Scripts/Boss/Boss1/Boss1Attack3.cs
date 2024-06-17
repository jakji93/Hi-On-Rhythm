using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Attack3 : MonoBehaviour
{
   [SerializeField] private GameObject projectile;
   [SerializeField] private float radius;
   [Min(1)]
   [SerializeField] int numOfProjectile = 1;
   [SerializeField] private float offset;

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.green;
      Gizmos.DrawWireSphere(transform.position, radius);
   }

   private void Start()
   {
      var degreeInBtw = (float)360 / numOfProjectile;
      for (int i = 0; i < numOfProjectile; i++) {
         float yPos = Mathf.Sin(Mathf.Deg2Rad * (i * degreeInBtw + offset)) * radius + transform.position.y;
         float xPos = Mathf.Cos(Mathf.Deg2Rad * (i * degreeInBtw + offset)) * radius + transform.position.x;
         var quartAngle = Quaternion.Euler(0, 0, i * degreeInBtw + offset + 180);
         Instantiate(projectile, new Vector3(xPos, yPos, 0), quartAngle);
      }
      Destroy(gameObject);
   }
}
