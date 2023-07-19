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
   private PlayerInputs playerInputs;

   private void Awake()
   {
      playerInputs = new PlayerInputs();
      playerInputs.Player.Enable();
      playerInputs.UI.Enable();

      playerInputs.Player.N1.performed += Noramal1_performed;
      playerInputs.Player.N2.performed += N2_performed;
      playerInputs.Player.Pause.performed += Pause_performed;

      playerInputs.UI.PrevTrack.performed += PrevTrack_performed;
      playerInputs.UI.NextTrack.performed += NextTrack_performed;
      playerInputs.UI.PrevSong.performed += PrevSong_performed;
      playerInputs.UI.NextSong.performed += NextSong_performed;
      playerInputs.UI.PrevDiff.performed += PrevDiff_performed;
      playerInputs.UI.NextDiff.performed += NextDiff_performed;
      playerInputs.UI.Confirm.performed += Confirm_performed;
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

   private void Noramal1_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
   {
      OnNormal1Pressed?.Invoke(this, EventArgs.Empty);
   }

   private void OnDestroy()
   {
      playerInputs.Dispose();
   }
}
