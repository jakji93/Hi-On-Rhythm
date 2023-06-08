using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
   [Range(-1f, 1f)]
   [Tooltip("-1 foreground : 1 background")]
   [SerializeField] private float parallaxFactor; // The parallax factor controlling the layer's speed
   private Transform cameraTransform; // Reference to the main camera's transform
   private Vector3 previousCameraPosition; // Previous position of the camera

   private void Start()
   {
      // Get the reference to the main camera's transform
      cameraTransform = Camera.main.transform;

      // Set the initial previous camera position
      previousCameraPosition = cameraTransform.position;
   }

   private void LateUpdate()
   {
      // Calculate the distance the camera has moved in the x-axis
      var cameraDelta = cameraTransform.position - previousCameraPosition;

      // Calculate the parallax offset for the layer
      var parallaxOffset = cameraDelta * parallaxFactor;

      // Apply the parallax offset to the layer's position
      Vector3 newPosition = transform.position + parallaxOffset;
      transform.position = newPosition;

      // Update the previous camera position
      previousCameraPosition = cameraTransform.position;
   }
}
