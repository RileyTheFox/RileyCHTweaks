using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using BiendeoCHLib.Patches.Attributes;
using BiendeoCHLib.Wrappers;
using UnityEngine;

namespace PrecisionMode
{
    [HarmonyCHPatch(typeof(VisibleHitWindowWrapper), nameof(VisibleHitWindowWrapper.Start))]
    class VisibleHitWindowStart
    {
        [HarmonyCHPostfix]
        static void Postfix(object __instance)
        {
            VisibleHitWindowWrapper visibleHitWindow = VisibleHitWindowWrapper.Wrap((VisibleHitWindow)__instance);

            // Remove this when the menu button works properly
            if (visibleHitWindow.Player.PlayerProfile.HasModifier(NoteWrapper.Modifier.Precision))
            {
                visibleHitWindow.Win = PrecisionMode.PRECISION_WINDOW_SIZE;
                Debug.Log($"Changed Visible Hit Window to {PrecisionMode.PRECISION_WINDOW_SIZE}");
                visibleHitWindow.UpdateLength();
            }
        }
    }

    [HarmonyCHPatch(typeof(BasePlayerWrapper), nameof(BasePlayerWrapper.Awake))]
    class BasePlayerAwake
    {
        [HarmonyCHPrefix]
        static void Prefix(object __instance)
        {
            BasePlayerWrapper basePlayer = BasePlayerWrapper.Wrap((BasePlayer)__instance);

            // Same as above, change when menu button is working.
            if(basePlayer.Player.PlayerProfile.HasModifier(NoteWrapper.Modifier.Precision))
            {
                basePlayer.SoloAct = PrecisionMode.PRECISION_WINDOW_SIZE;
                Debug.Log($"Changed Window Size to {PrecisionMode.PRECISION_WINDOW_SIZE * 1000}ms on each end. {PrecisionMode.PRECISION_WINDOW_SIZE * 1000 * 2}ms total.");
            }
        }
    }

    [HarmonyCHPatch(typeof(BasePlayerWrapper), nameof(BasePlayerWrapper.Start))]
    class BasePlayerStart
    {
        [HarmonyCHPrefix]
        static void Prefix(object __instance)
        {
            BasePlayerWrapper basePlayer = BasePlayerWrapper.Wrap((BasePlayer)__instance);
            BaseGuitarPlayerWrapper baseGuitarPlayer = basePlayer.CastToBaseGuitarPlayer();

            if(basePlayer.Player.PlayerProfile.HasModifier(NoteWrapper.Modifier.Precision))
            {
                baseGuitarPlayer.StrumLenienceAmount = PrecisionMode.PRECISION_STRUM_LENIENCE_AMOUNT;
                baseGuitarPlayer.StrumLenienceAmountNoNotes = PrecisionMode.PRECISION_STRUM_LENIENCE_AMOUNT_NO_NOTES;

                //Debug.Log($"Changed Strum Leniency to {baseGuitarPlayer.StrumLenienceAmount * 1000}ms");
                //Debug.Log($"Changed Strum Leniency No Notes to {baseGuitarPlayer.StrumLenienceAmountNoNotes * 1000}ms");
            }
        }
    }

    [HarmonyCHPatch(typeof(BaseGuitarPlayerWrapper), nameof(BaseGuitarPlayerWrapper.HitNote))]
    class BaseGuitarPlayerHitNote
    {
        [HarmonyCHPostfix]
        static void Postfix(object __instance, object __0)
        {
            BaseGuitarPlayerWrapper baseGuitarPlayer = BaseGuitarPlayerWrapper.Wrap((BaseGuitarPlayer)__instance);
            BasePlayerWrapper basePlayer = baseGuitarPlayer.CastToBasePlayer();
            NoteWrapper hitNote = NoteWrapper.Wrap(__0);

            if (!basePlayer.Player.PlayerProfile.HasModifier(NoteWrapper.Modifier.Precision))
                return;

            if (hitNote.IsHOPO || hitNote.IsTap)
            {
                baseGuitarPlayer.HopoLenienceTimer = PrecisionMode.PRECISION_HOPO_LENIENCE_AMOUNT;
                //Debug.Log($"Set Hopo Lenience Timer to {baseGuitarPlayer.HopoLenienceTimer * 1000}ms");
            }

            PrecisionMode.Instance.PlayerGhostCount[basePlayer.Player.PlayerIndex] = 0;
            if (hitNote.NoteMask != 64)
                PrecisionMode.Instance.PlayerLastNoteMask[basePlayer.Player.PlayerIndex] = hitNote.NoteMask;
        }
    }
}
