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
            if (true)//visibleHitWindow.Player.PlayerProfile.HasModifier(NoteWrapper.Modifier.Precision))
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
            if(true)
            {
                basePlayer.SoloAct = PrecisionMode.PRECISION_WINDOW_SIZE;
                Debug.Log($"Changed Window Size to {PrecisionMode.PRECISION_WINDOW_SIZE * 1000}ms on each end. {PrecisionMode.PRECISION_WINDOW_SIZE * 1000 * 2}ms total.");
            }
        }
    }
}
