using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Attack4 : MonoBehaviour
{
   [SerializeField] private GameObject projectile;
   [Min(1)]
   [SerializeField] private int numOfProjectiles = 1;
   [Min(0)]
   [SerializeField] private float arcAngle = 0;

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.green;
      var initialPos = transform.position;
      var endPoint1 = new Vector3(initialPos.x + 5, initialPos.y, initialPos.z);
      Gizmos.DrawLine(initialPos, endPoint1);
      float yPos = Mathf.Sin(Mathf.Deg2Rad * (arcAngle)) * 5;
      float xPos = Mathf.Cos(Mathf.Deg2Rad * (arcAngle)) * 5;
      var endPoint2 = new Vector3(initialPos.x + xPos, initialPos.y + yPos, 0);
      Gizmos.DrawLine(initialPos, endPoint2);
   }

   private void Start()
   {
      var angleInBtw = arcAngle / numOfProjectiles;
      var playerDir = (PlayerControl.Instance.transform.position - transform.position).normalized;
      var playAngle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;
      var initialAngle = playAngle + arcAngle / 2;
      for (int i = 0; i < numOfProjectiles; i++) {
         var quartAngle = Quaternion.Euler(0, 0, initialAngle - i * angleInBtw);
         Instantiate(projectile, transform.position, quartAngle);
      }
      Destroy(gameObject);
   }
}
