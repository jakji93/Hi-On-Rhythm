using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
   public event EventHandler OnNormal1Pressed;
   public event EventHandler OnNormal2Pressed;
   private PlayerInputs playerInputs;

   private void Awake()
   {
      playerInputs = new PlayerInputs();
      playerInputs.Player.Enable();

      playerInputs.Player.N1.performed += Noramal1_performed;
      playerInputs.Player.N2.performed += N2_performed;
   }

   private void N2_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
   {
      OnNormal2Pressed?.Invoke(this, EventArgs.Empty);
   }

   private void Noramal1_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
   {
      OnNormal1Pressed?.Invoke(this, EventArgs.Empty); 
   }
}
