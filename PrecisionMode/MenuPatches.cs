using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BiendeoCHLib.Patches.Attributes;
using BiendeoCHLib.Wrappers;
using UnityEngine.UI;
using UnityEngine;

namespace PrecisionMode
{
    [HarmonyCHPatch(typeof(PlayerSelectionWrapper), nameof(PlayerSelectionWrapper.ClearMenuStrings))]
    class PlayerSelectionClearMenuStrings
    {
        [HarmonyCHPrefix]
        static bool Prefix(object __instance)
        {
            PlayerSelectionWrapper playerSelection = PlayerSelectionWrapper.Wrap((PlayerSelection)__instance);
            BaseMenuWrapper baseMenu = playerSelection.CastToBaseMenu();

            baseMenu.MenuStrings = new string[15];
            playerSelection.UsedMenuStrings = 0;

            return false;
        }
    }

    /*[HarmonyCHPatch(typeof(PlayerSelectionWrapper), nameof(PlayerSelectionWrapper.ShowModifiers))]
    class PlayerSelectionShowModifiers
    {
        [HarmonyCHPrefix]
        static bool Prefix(object __instance)
        {
            PlayerSelectionWrapper playerSelection = PlayerSelectionWrapper.Wrap((PlayerSelection)__instance);

            playerSelection.Header.text = LanguageManagerWrapper.Instance.GetString("Modifiers");
            playerSelection.State = PlayerSelectionWrapper.MenuState.ChoosingModifier;
            playerSelection.ClearMenuStrings();
            playerSelection.AddMenuString("Ready");
            playerSelection.AddMenuString("None");
            playerSelection.AddMenuString("Precision Mode");
            playerSelection.AddMenuString("All Strums");
            playerSelection.AddMenuString("All HOPO's");
            playerSelection.AddMenuString("All Taps");
            playerSelection.AddMenuString("All Opens");
            playerSelection.AddMenuString("HOPO's to Taps");
            playerSelection.AddMenuString("Mirror Mode");
            playerSelection.AddMenuString("Note Shuffle");
            playerSelection.AddMenuString("Lights Out");
            playerSelection.AddMenuString("ModChart Full");
            playerSelection.AddMenuString("ModChart Lite");
            playerSelection.AddMenuString("ModChart Prep");
            playerSelection.SetMenuProperties();
            playerSelection.CastToBaseMenu().JumpTo("Ready");
            playerSelection.ShowChosenModifiers();

            return false;
        }
    }*/

    /*[HarmonyCHPatch(typeof(PlayerSelectionWrapper), nameof(PlayerSelectionWrapper.ShowChosenModifiers))]
    class PlayerSelectionShowChosenModifiers
    {
        static bool Prefix(object __instance)
        {
            PlayerSelectionWrapper playerSelection = PlayerSelectionWrapper.Wrap((PlayerSelection)__instance);
            BaseMenuWrapper baseMenu = playerSelection.CastToBaseMenu();

            for (int i = 0; i < baseMenu.MaxMenuPosition; i++)
            {
                Image image = playerSelection.ModifiersChosen[i];
                string text = baseMenu.MenuStrings[baseMenu.TopOfMenu + i];

                switch (text)
                {
                    case "Ready":
                        image.enabled = false;
                        break;
                    case "Precision Mode":
                        image.enabled = baseMenu.ControllingPlayer.PlayerProfile.HasModifier(NoteWrapper.Modifier.Precision);
                        break;
                    case "All Strums":
                        image.enabled = baseMenu.ControllingPlayer.PlayerProfile.HasModifier(NoteWrapper.Modifier.AllStrums);
                        break;
                    case "All HOPO's":
                        image.enabled = baseMenu.ControllingPlayer.PlayerProfile.HasModifier(NoteWrapper.Modifier.AllHOPOs);
                        break;
                    case "All Opens":
                        image.enabled = baseMenu.ControllingPlayer.PlayerProfile.HasModifier(NoteWrapper.Modifier.AllOpens);
                        break;
                    case "All Taps":
                        image.enabled = baseMenu.ControllingPlayer.PlayerProfile.HasModifier(NoteWrapper.Modifier.AllTaps);
                        break;
                    case "Mirror Mode":
                        image.enabled = baseMenu.ControllingPlayer.PlayerProfile.HasModifier(NoteWrapper.Modifier.MirrorMode);
                        break;
                    case "Note Shuffle":
                        image.enabled = baseMenu.ControllingPlayer.PlayerProfile.HasModifier(NoteWrapper.Modifier.Shuffle);
                        break;
                    case "HOPO's to Taps":
                        image.enabled = baseMenu.ControllingPlayer.PlayerProfile.HasModifier(NoteWrapper.Modifier.HOPOsToTaps);
                        break;
                    case "Lights Out":
                        image.enabled = baseMenu.ControllingPlayer.PlayerProfile.HasModifier(NoteWrapper.Modifier.LightsOut);
                        break;
                    case "ModChart Full":
                        image.enabled = baseMenu.ControllingPlayer.PlayerProfile.HasModifier(NoteWrapper.Modifier.ModFull);
                        break;
                    case "ModChart Lite":
                        image.enabled = baseMenu.ControllingPlayer.PlayerProfile.HasModifier(NoteWrapper.Modifier.ModLite);
                        break;
                    case "ModChart Prep":
                        image.enabled = baseMenu.ControllingPlayer.PlayerProfile.HasModifier(NoteWrapper.Modifier.ModPrep);
                        break;
                    case "None":
                        image.enabled = baseMenu.ControllingPlayer.PlayerProfile.HasModifier(NoteWrapper.Modifier.None);
                        break;
                }
            }
            return false;
        }
    }

    [HarmonyCHPatch(typeof(PlayerSelectionWrapper), nameof(PlayerSelectionWrapper.ChooseModifier))]
    class PlayerSelectionChooseModifier
    {
        [HarmonyCHPrefix]
        static bool Prefix(object __instance)
        {
            PlayerSelectionWrapper playerSelection = PlayerSelectionWrapper.Wrap((PlayerSelection)__instance);
            PlayerProfileWrapper playerProfile = playerSelection.CastToBaseMenu().ControllingPlayer.PlayerProfile;

            string text = playerSelection.CastToBaseMenu().CurrentSelectionString;

            switch (text)
            {
                case "Ready":
                    playerSelection.ShowReady();
                    return false;
                case "None":
                    Debug.Log(playerProfile.NoteModifier);
                    playerProfile.NoteModifier = NoteWrapper.Modifier.None;
                    Debug.Log("No more modifiers enabled");
                    break;
                case "Precision Mode":
                    playerProfile.AddModifier(NoteWrapper.Modifier.Precision);
                    UnityEngine.Debug.Log("Precision enabled");
                    break;
                case "All Strums":
                    playerProfile.AddModifier(NoteWrapper.Modifier.AllStrums);
                    Debug.Log("Enabled all strums");
                    break;
                case "All HOPO's":
                    playerProfile.AddModifier(NoteWrapper.Modifier.AllHOPOs);
                    break;
                case "All Taps":
                    playerProfile.AddModifier(NoteWrapper.Modifier.AllTaps);
                    break;
                case "All Opens":
                    playerProfile.AddModifier(NoteWrapper.Modifier.AllOpens);
                    break;
                case "Mirror Mode":
                    playerProfile.AddModifier(NoteWrapper.Modifier.MirrorMode);
                    break;
                case "Note Shuffle":
                    playerProfile.AddModifier(NoteWrapper.Modifier.Shuffle);
                    break;
                case "HOPO's to Taps":
                    playerProfile.AddModifier(NoteWrapper.Modifier.HOPOsToTaps);
                    break;
                case "Lights Out":
                    playerProfile.AddModChartModifier(NoteWrapper.Modifier.LightsOut);
                    break;
                case "ModChart Full":
                    playerProfile.AddModChartModifier(NoteWrapper.Modifier.ModFull);
                    break;
                case "ModChart Lite":
                    playerProfile.AddModChartModifier(NoteWrapper.Modifier.ModLite);
                    break;
                case "ModChart Prep":
                    playerProfile.AddModChartModifier(NoteWrapper.Modifier.ModPrep);
                    break;
            }
            playerSelection.ShowChosenModifiers();
            Debug.Log("Showing chosen modifiers");

            return false;
        }
    }*/
}
