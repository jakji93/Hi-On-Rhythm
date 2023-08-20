using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
   public event EventHandler OnNormal1Pressed;
   public event EventHandler OnNormal2Pressed;
   public event EventHandler OnPausePressed;
   public event EventHandler OnPrevTrackPressed;
   public event EventHandler OnNextTrackPressed;
   public event EventHandler OnPrevSongPressed;
   public event EventHandler OnNextSongPressed;
   public event EventHandler OnPrevDifficultyPressed;
   public event EventHandler OnNextDifficultyPressed;
   public event EventHandler OnStartPressed;
   public event EventHandler OnBackPressed;
   private PlayerInputs playerInputs;
    [SerializeField] InputActionAsset playerAction;

   private void Awake()
   {
      playerInputs = new PlayerInputs();
      playerInputs.Player.Enable();
      playerInputs.UI.Enable();

      //playerInputs.Player.N1.performed += Noramal1_performed;
      //playerInputs.Player.N2.performed += N2_performed;
      playerAction.FindAction("N1").performed += Noramal1_performed;
      playerAction.FindAction("N2").performed += N2_performed;
      playerInputs.Player.Pause.performed += Pause_performed;

      playerInputs.UI.PrevTrack.performed += PrevTrack_performed;
      playerInputs.UI.NextTrack.performed += NextTrack_performed;
      playerInputs.UI.PrevSong.performed += PrevSong_performed;
      playerInputs.UI.NextSong.performed += NextSong_performed;
      playerInputs.UI.PrevDiff.performed += PrevDiff_performed;
      playerInputs.UI.NextDiff.performed += NextDiff_performed;
      playerInputs.UI.Confirm.performed += Confirm_performed;
      playerInputs.UI.Back.performed += Back_performed;
      playerInputs.UI.PrevSongScroll.performed += PrevSongScroll_performed;
      playerInputs.UI.NextSongScroll.performed += NextSongScroll_performed;
   }

   private void NextSongScroll_performed(InputAction.CallbackContext obj)
   {
      var num = obj.ReadValue<float>();
      if(num > 0) {
         OnNextSongPressed?.Invoke(this, EventArgs.Empty);
      }
   }

   private void PrevSongScroll_performed(InputAction.CallbackContext obj)
   {
      var num = obj.ReadValue<float>();
      if (num > 0) {
         OnPrevSongPressed?.Invoke(this, EventArgs.Empty);
      }
   }

   private void Back_performed(InputAction.CallbackContext obj)
   {
      OnBackPressed?.Invoke(this, EventArgs.Empty);
   }

   private void Confirm_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
   {
      OnStartPressed?.Invoke(this, EventArgs.Empty);
   }

   private void NextDiff_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
   {
      OnNextDifficultyPressed?.Invoke(this, EventArgs.Empty);
   }

   private void PrevDiff_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
   {
      OnPrevDifficultyPressed?.Invoke(this, EventArgs.Empty);
   }

   private void NextSong_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
   {
      OnNextSongPressed?.Invoke(this, EventArgs.Empty);
   }

   private void PrevSong_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
   {
      OnPrevSongPressed?.Invoke(this, EventArgs.Empty);
   }

   private void NextTrack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
   {
      OnNextTrackPressed?.Invoke(this, EventArgs.Empty);
   }

   private void PrevTrack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
   {
      OnPrevTrackPressed?.Invoke(this, EventArgs.Empty);
   }

   private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
   {
      OnPausePressed?.Invoke(this, EventArgs.Empty);
   }

   private void N2_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
   {
      OnNormal2Pressed?.Invoke(this, EventArgs.Empty);
   }

   public void Noramal1_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
   {
      OnNormal1Pressed?.Invoke(this, EventArgs.Empty);
   }


   private void OnDestroy()
   {
      playerInputs.Dispose();
   }
}
