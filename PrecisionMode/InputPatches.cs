using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiendeoCHLib.Wrappers;
using BiendeoCHLib.Patches.Attributes;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace PrecisionMode
{
    [HarmonyCHPatch(typeof(BaseGuitarPlayerWrapper), nameof(BaseGuitarPlayerWrapper.UpdateInput))]
    class BaseGuitarPlayerUpdateInput
    {
        [HarmonyCHPrefix]
        static void UpdateInput(object __instance)
        {
            BaseGuitarPlayerWrapper baseGuitarPlayerWrapper = BaseGuitarPlayerWrapper.Wrap((BaseGuitarPlayer)__instance);
            BasePlayerWrapper basePlayer = baseGuitarPlayerWrapper.CastToBasePlayer();

            if (basePlayer.IsPlaying)
                PrecisionMode.Instance.AddGhostCountToPlayer(basePlayer.Player, PrecisionMode.Instance.CountFretsPressedThisFrame(basePlayer.ButtonsPressed, basePlayer.ButtonsPressedLastFrame));
        }
    }

    /*[HarmonyCHPatch(typeof(GuitarPlayerWrapper), nameof(GuitarPlayerWrapper.UpdateInput))]
    class GuitarPlayerUpdateInput
    {
        [HarmonyCHPrefix]
        static bool Prefix(object __instance)
        {
            GuitarPlayerWrapper guitarPlayer = GuitarPlayerWrapper.Wrap((GuitarPlayer)__instance);
            BaseGuitarPlayerWrapper baseGuitarPlayer = guitarPlayer.CastToBaseGuitarPlayer();
            BasePlayerWrapper basePlayer = baseGuitarPlayer.CastToBasePlayer();

            if (!basePlayer.IsPlaying)
                return false;

            int buttonsPressedInt = basePlayer.ButtonsPressed;
            if (baseGuitarPlayer.StrummedThisFrame)
            {
                //Debug.Log("Strummed last frame");
            }
            baseGuitarPlayer.StrummedThisFrame = false;

            if (basePlayer.Player.PlayerProfile.Bot)
                return false;

            if (Input.GetKeyDown(KeyCode.F))
            {
                basePlayer.StarPower_Gain(0.25f);
            }

            // Down Green
            if (basePlayer.Player.Player.GetButtonDown(0))
            {
                buttonsPressedInt |= 1;
                PrecisionMode.Instance.PlayerGhostCount[basePlayer.Player.PlayerIndex] += 1;
            }
            // Up Green
            if (basePlayer.Player.Player.GetButtonUp(0))
            {
                buttonsPressedInt &= ~1;
            }

            // Down Red
            if (basePlayer.Player.Player.GetButtonDown(1))
            {
                buttonsPressedInt |= 2;
                PrecisionMode.Instance.PlayerGhostCount[basePlayer.Player.PlayerIndex] += 1;
            }
            // Up Red
            if (basePlayer.Player.Player.GetButtonUp(1))
            {
                buttonsPressedInt &= ~2;
            }

            // Down Yellow
            if (basePlayer.Player.Player.GetButtonDown(2))
            {
                buttonsPressedInt |= 4;
                PrecisionMode.Instance.PlayerGhostCount[basePlayer.Player.PlayerIndex] += 1;
            }
            // Up Yellow
            if (basePlayer.Player.Player.GetButtonUp(2))
            {
                buttonsPressedInt &= ~4;
            }

            // Down Blue
            if (basePlayer.Player.Player.GetButtonDown(3))
            {
                buttonsPressedInt |= 8;
                PrecisionMode.Instance.PlayerGhostCount[basePlayer.Player.PlayerIndex] += 1;
            }
            // Up Blue
            if (basePlayer.Player.Player.GetButtonUp(3))
            {
                buttonsPressedInt &= ~8;
            }

            // Down Orange
            if (basePlayer.Player.Player.GetButtonDown(4))
            {
                buttonsPressedInt |= 16;
                PrecisionMode.Instance.PlayerGhostCount[basePlayer.Player.PlayerIndex] += 1;
            }
            // Up Orange
            if (basePlayer.Player.Player.GetButtonUp(4))
            {
                buttonsPressedInt &= ~16;
            }

            // Down Some Button
            if (basePlayer.Player.Player.GetButtonDown(8))
            {
                buttonsPressedInt |= 64;
                PrecisionMode.Instance.PlayerGhostCount[basePlayer.Player.PlayerIndex] += 1;
            }
            // Up
            if (basePlayer.Player.Player.GetButtonUp(8))
            {
                buttonsPressedInt &= ~64;
            }

            basePlayer.ButtonsPressed = Convert.ToByte(buttonsPressedInt);
            BaseGuitarPlayerUpdateInput.UpdateInput(baseGuitarPlayer, basePlayer);

            return false;
        }
    }

    class BaseGuitarPlayerUpdateInput
    {
        public static void UpdateInput(BaseGuitarPlayerWrapper baseGuitarPlayer, BasePlayerWrapper basePlayer)
        {

            if (!basePlayer.IsSPActive && basePlayer.SPAmount >= 0.5f
                && (basePlayer.Player.Player.GetButtonDown(6) || (GameSettingWrapper.IsSettingEnabled(basePlayer.Player.PlayerProfile.Tilt) && basePlayer.Player.Player.GetAxis(7) >= 1f)))
            {
                basePlayer.DeployStarPower();
            }
            if (GameSettingWrapper.IsSettingEnabled(basePlayer.Player.PlayerProfile.DualshockMode))
            {
                baseGuitarPlayer.StrummedThisFrame = (basePlayer.ButtonsPressed > basePlayer.ButtonsPressedLastFrame);
            }
            else
            {
                if (basePlayer.Player.Player.GetButtonDown(5) || basePlayer.Player.Player.GetButtonDown(14))
                {
                    baseGuitarPlayer.StrummedThisFrame = true;
                }
                if (basePlayer.ButtonsPressed == 0)
                {
                    basePlayer.ButtonsPressed |= 64;
                }
            }
            baseGuitarPlayer.CurrentWhammy = basePlayer.Player.Player.GetAxis(17);
            if (basePlayer.Player.Player.GetButton(30))
            {
                baseGuitarPlayer.CurrentWhammy = 1f;
            }
            if (baseGuitarPlayer.CurrentWhammy != baseGuitarPlayer.LastWhammy)
            {
                baseGuitarPlayer.WhammyTimer = 0.25f;
            }
            if (basePlayer.Player.Player.GetButton(30))
            {
                baseGuitarPlayer.WhammyTimer = 0.75f;
            }
            baseGuitarPlayer.LastWhammy = baseGuitarPlayer.CurrentWhammy;
            if (basePlayer.ButtonsPressed != baseGuitarPlayer.ButtonsPressedTap)
            {
                baseGuitarPlayer.ButtonsPressedTap = 0;
            }
            basePlayer.ButtonsPressedLastFrame = basePlayer.ButtonsPressed;

            if (GameSettingWrapper.IsSettingEnabled(basePlayer.Player.PlayerProfile.DualshockMode) && basePlayer.ButtonsPressed == 0)
            {
                basePlayer.ButtonsPressed |= 64;
            }
            if (baseGuitarPlayer.StrummedThisFrame)
            {
                if (baseGuitarPlayer.HopoLenienceTimer > 0f)
                {
                    baseGuitarPlayer.StrumLenienceTimer = 0f;
                    return;
                }
                if (baseGuitarPlayer.StrumLenienceTimer > 0f)
                {
                    basePlayer.OverStrum(true);
                }
                if (basePlayer.HittableNotesThisFrame > 0)
                {
                    baseGuitarPlayer.StrumLenienceTimer = baseGuitarPlayer.StrumLenienceAmount;
                }
                else
                {
                    baseGuitarPlayer.StrumLenienceAmount = baseGuitarPlayer.StrumLenienceAmountNoNotes;
                }
                baseGuitarPlayer.WasHOPOStrummed = false;
            }
        }
    }*/
}
