using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using BiendeoCHLib.Patches.Attributes;
using BiendeoCHLib.Wrappers;
using UnityEngine;
using Rewired;
using System.Runtime.CompilerServices;

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
                PrecisionMode.BepLog.LogDebug($"Changed Visible Hit Window to {PrecisionMode.PRECISION_WINDOW_SIZE}");
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
            if (basePlayer.Player.PlayerProfile.HasModifier(NoteWrapper.Modifier.Precision))
            {
                basePlayer.SoloAct = PrecisionMode.PRECISION_WINDOW_SIZE;
                PrecisionMode.BepLog.LogDebug($"Changed Window Size to {PrecisionMode.PRECISION_WINDOW_SIZE * 1000}ms on each end. {PrecisionMode.PRECISION_WINDOW_SIZE * 1000 * 2}ms total.");
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

            if (basePlayer.Player.PlayerProfile.HasModifier(NoteWrapper.Modifier.Precision))
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

            PrecisionMode.Instance.SetPlayerGhostCount(basePlayer.Player, 0);
            if (hitNote.NoteMask != 64)
                PrecisionMode.Instance.PlayerLastNoteMask[basePlayer.Player.PlayerIndex] = hitNote.NoteMask;
        }
    }

    // REVERSE PATCH
    [HarmonyCHPatch(typeof(BasePlayerWrapper), nameof(BasePlayerWrapper.Update))]
    class BasePlayerReverseUpdatePatch
    {
        [HarmonyCHReversePatch]
        public static void Update(object __instance)
        {

        }
    }

    [HarmonyCHPatch(typeof(BaseGuitarPlayerWrapper), nameof(BaseGuitarPlayerWrapper.Update))]
    class BaseGuitarPlayerUpdatePatch
    {
        [HarmonyCHPrefix]
        static bool Prefix(object __instance)
        {
            BaseGuitarPlayerWrapper baseGuitarPlayer = BaseGuitarPlayerWrapper.Wrap((BaseGuitarPlayer)__instance);
            BasePlayerWrapper basePlayer = baseGuitarPlayer.CastToBasePlayer();

            // CastToBasePlayer() returns BasePlayer wrapper
            // .BasePlayer returns the obfuscated BasePlayer class (not the wrapper)
            // The reverse patch function call
            BasePlayerReverseUpdatePatch.Update(basePlayer.BasePlayer);
            
            if (baseGuitarPlayer.StrumLenienceTimer > 0f)
            {
                if (baseGuitarPlayer.HopoLenienceTimer > 0f)
                {
                    baseGuitarPlayer.HopoLenienceTimer = 0f;
                    baseGuitarPlayer.StrumLenienceTimer = 0f;
                }
                else
                {
                    baseGuitarPlayer.StrumLenienceTimer -= Time.deltaTime;
                    if (baseGuitarPlayer.StrumLenienceTimer <= 0f)
                    {
                        if (baseGuitarPlayer.WasHOPOStrummed)
                        {
                            baseGuitarPlayer.StrumLenienceTimer = 0f;
                        }
                        else
                        {
                            baseGuitarPlayer.OverStrum(true);
                        }
                        baseGuitarPlayer.WasHOPOStrummed = false;
                    }
                }
            }
            if (baseGuitarPlayer.HopoLenienceTimer > 0f)
            {
                baseGuitarPlayer.HopoLenienceTimer -= Time.deltaTime;
            }
            baseGuitarPlayer.CheckForHitNotes();
            baseGuitarPlayer.UpdateSustains();
            if(baseGuitarPlayer.WhammyTimer > 0f)
            {
                baseGuitarPlayer.WhammyTimer -= Time.deltaTime;
            }
            return false;
        }
    }

    [HarmonyCHPatch(typeof(BaseGuitarPlayerWrapper), nameof(BaseGuitarPlayerWrapper.CheckForHitNotes))]
    class BaseGuitarPlayerCheckForHitNotes
    {
        [HarmonyCHPrefix]
        static bool Prefix(object __instance)
        {
            BaseGuitarPlayerWrapper baseGuitarPlayer = BaseGuitarPlayerWrapper.Wrap((BaseGuitarPlayer)__instance);
            BasePlayerWrapper basePlayer = baseGuitarPlayer.CastToBasePlayer();

            if(basePlayer.HittableNotesThisFrame > 0)
            {
                int i = 0;
                baseGuitarPlayer.HopoLenienceTimer = 0f;
                while(i < basePlayer.HittableNotesThisFrame)
                {
                    NoteWrapper note = basePlayer.HittableNotes[i];
                    if (!note.WasMissed)
                    {
                        if (baseGuitarPlayer.ButtonsPressedTap == 0 &&
                            ((note.IsTap && (basePlayer.Combo == 0 || i == 0)) ||
                            (note.IsHOPO && i == 0 && (basePlayer.Combo > 0 || (GlobalVariablesWrapper.Instance.IsInPracticeMode && !basePlayer.FirstNoteMissed)))) && 
                            baseGuitarPlayer.WasNoteHit(note))
                        {
                            int ghostLimit = PrecisionMode.Instance.CountFrets(note.NoteMask) + PrecisionMode.PRECISION_ANTI_GHOSTING_LIMIT;
                            if (PrecisionMode.Instance.GetPlayerGhostCount(basePlayer.Player) < ghostLimit || basePlayer.Player.PlayerProfile.HasModifier(NoteWrapper.Modifier.Precision))
                            {
                                baseGuitarPlayer.HitNote(note);
                                return false;
                            }
                            else PrecisionMode.BepLog.LogDebug("Prevented note being hit due to ghosting. Ghost count ");
                        }
                        if(baseGuitarPlayer.StrumLenienceTimer > 0f && baseGuitarPlayer.HopoLenienceTimer <= 0f && (i == 0 || basePlayer.Combo == 0) && baseGuitarPlayer.WasNoteHit(note))
                        {
                            baseGuitarPlayer.HitNote(note);
                            return false;
                        }
                        i++;
                    }
                }
            }
            return false;
        }
    }

    [HarmonyCHPatch(typeof(BasePlayerWrapper), nameof(BasePlayerWrapper.MissNote))]
    class BasePlayerMissNote
    {
        [HarmonyCHPrefix]
        static void Prefix(object __instance)
        {
            BasePlayerWrapper basePlayer = BasePlayerWrapper.Wrap((BasePlayer)__instance);

            PrecisionMode.Instance.SetPlayerGhostCount(basePlayer.Player, 0);
            PrecisionMode.BepLog.LogDebug("Missed note. Resetting ghost count");
        }
    }
}