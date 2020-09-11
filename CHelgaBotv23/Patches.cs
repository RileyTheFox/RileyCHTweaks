using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using BiendeoCHLib.Patches.Attributes;
using BiendeoCHLib.Wrappers;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace CHelgaBotv23
{
    [HarmonyCHPatch(typeof(GameManagerWrapper), nameof(GameManagerWrapper.Start))]
    class GameManagerStartPatch
    {
        [HarmonyCHPrefix]
        static void Prefix()
        {
            CHelgaChart.ClearCharts();
        }
    }

    [HarmonyCHPatch(typeof(ChartWrapper), nameof(ChartWrapper.GetNotesFromStandardChart))]
    class GetNotesFromStandardChartPatch
    {
        [HarmonyCHPostfix]
        static void Postfix(object __0, object __1, object __2, object __result)
        {
            SongObjectWrapper song = SongObjectWrapper.Wrap(__0);
            MoonChartWrapper moonChart = MoonChartWrapper.Wrap(__1);
            PlayerProfileWrapper playerProfile = PlayerProfileWrapper.Wrap(__2);

            foreach(CHelgaChart chelgaChart in CHelgaChart.Charts)
            {
                if (chelgaChart.MoonChart.Events == moonChart.Events)
                    return;
            }

            List<NoteWrapper> notes = new List<NoteWrapper>();//((List<object>)__result).Select(o => NoteWrapper.Wrap(o)).ToList();

            List<Activation> activations = new List<Activation>();

            foreach (ChartEventWrapper chartEvent in moonChart.Events)
            {
                if (chartEvent.EventName == "squeeze")
                {
                    Activation act = new Activation(ActivationType.SQUEEZE, chartEvent.CastToSongObject().Tick);

                    foreach (NoteWrapper note in notes)
                    {
                        if (note.TickPosition >= chartEvent.CastToSongObject().Tick)
                        {
                            act.Note = note;
                            break;
                        }
                    }
                }

                if (chartEvent.EventName == "activate_sp")
                {
                    activations.Add(new Activation(ActivationType.ACTIVATE_SP, chartEvent.CastToSongObject().Tick));
                }

                if (chartEvent.EventName == "activate_fuckinglate")
                {
                    activations.Add(new Activation(ActivationType.ACTIVATE_FUCKINGLATE, chartEvent.CastToSongObject().Tick));
                }

                if (chartEvent.EventName == "activate_nosqueeze")
                {
                    activations.Add(new Activation(ActivationType.ACTIVATE_NOSQUEEZE, chartEvent.CastToSongObject().Tick));
                }

                if (chartEvent.EventName == "increase_window")
                {
                    activations.Add(new Activation(ActivationType.INCREASE_WINDOW, chartEvent.CastToSongObject().Tick));
                }

                if (chartEvent.EventName == "decrease_window")
                {
                    activations.Add(new Activation(ActivationType.DECREASE_WINDOW, chartEvent.CastToSongObject().Tick));
                }

                if (chartEvent.EventName == "squeeze_nowindow")
                {
                    Activation act = new Activation(ActivationType.SQUEEZE_NOWINDOW, chartEvent.CastToSongObject().Tick);

                    foreach (NoteWrapper note in notes)
                    {
                        if (note.TickPosition >= chartEvent.CastToSongObject().Tick)
                        {
                            act.Note = note;
                            break;
                        }
                    }
                }

                /*if (chartEvent.EventName.Length > 6)
                {
                    if (chartEvent.EventName[6] == '_')
                    {
                        args = chartEvent.EventName.Substring(chartEvent.EventName.LastIndexOf('_') + 1);
                        chartEvent.EventName = chartEvent.EventName.Split('_')[0];

                        try
                        {
                            argsInt = Convert.ToInt32(args);
                        }
                        catch (Exception e)
                        {
                            chartEvent.EventName = "DFJKSDAKJH457843ERT";
                        }
                    }
                }

                if (chartEvent.EventName == "anchor")
                {
                    activations.Add(new AnchorEvent(argsInt, chartEvent.position));
                }*/

                if (chartEvent.EventName == "stop_whammy")
                {
                    activations.Add(new Activation(ActivationType.STOP_WHAMMY, chartEvent.CastToSongObject().Tick));
                }

                if (chartEvent.EventName == "start_whammy")
                {
                    activations.Add(new Activation(ActivationType.START_WHAMMY, chartEvent.CastToSongObject().Tick));
                }

                if (chartEvent.EventName == "tiny_window")
                {
                    activations.Add(new Activation(ActivationType.TINY_WINDOW, chartEvent.CastToSongObject().Tick));
                }
            }
            CHelgaChart.AddChart(moonChart, activations);
            Debug.Log("Added activations from chart");
        }
    }

    /*[HarmonyCHPatch(typeof(BaseGuitarPlayerWrapper), nameof(BaseGuitarPlayerWrapper.CheckForHitNotes))]
    class CheckForHitNotesPatch
    {
        [HarmonyCHPrefix]
        static bool Prefix(object __instance)
        {
            CHelgaBot.Instance.CheckForHitNotes();
            return false; // This return makes the underlying method get skipped and only this prefix will run
        }
    }*/

    [HarmonyCHPatch(typeof(BasePlayerWrapper), nameof(BasePlayerWrapper.Update))]
    class BasePlayerUpdatePatch
    {
        static bool ran = false;

        [HarmonyCHPrefix]
        static void Prefix(object __instance)
        {
            if(!ran)
            {
                ran = true;
                Debug.Log("this is in baseplayer!!!!");
            }
        }
    }

    [HarmonyCHPatch(typeof(BasePlayerWrapper), nameof(BasePlayerWrapper.Update))]
    class BasePlayerReverseUpdatePatch
    {
        [HarmonyCHReversePatch]
        public static void ReverseUpdate(object __instance)
        {
            BasePlayerWrapper basePlayer = BasePlayerWrapper.Wrap((BasePlayer)__instance);
            basePlayer.Health = basePlayer.Health;
            // this bit is running but actual base player update doesn't seem to be working
            Debug.Log("this is in baseplayer reverse");
        }
    }

    [HarmonyCHPatch(typeof(BaseGuitarPlayerWrapper), nameof(BaseGuitarPlayerWrapper.Update))]
    class BaseGuitarPlayerUpdatePatch
    {
        [HarmonyCHPrefix]
        static bool Prefix(object __instance)
        {
            /*// This is a wrapper to the derived class (it's obfuscated normally)
            BaseGuitarPlayerWrapper baseGuitarPlayer = BaseGuitarPlayerWrapper.Wrap((BaseGuitarPlayer)__instance);

            // CastToBasePlayer() returns the base class wrapper for BaseGuitarPlayer (BaseGuitarPlayer is derived from the class "BasePlayer")
            // Calling this update function causes it to lock up in an infinite loop, because it's actually calling BaseGuitarPlayer's update
            // (because it overrides the BasePlayer update), rather than BasePlayer's update
            baseGuitarPlayer.CastToBasePlayer().Update();

            return true;*/

            // These 2 log statements run
            BaseGuitarPlayerWrapper baseGuitarPlayer = BaseGuitarPlayerWrapper.Wrap((BaseGuitarPlayer)__instance);
            Debug.Log("before baseplayer update");
            BasePlayerReverseUpdatePatch.ReverseUpdate(baseGuitarPlayer.CastToBasePlayer().BasePlayer);
            Debug.Log("after baseplayer update");

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
            if (baseGuitarPlayer.WhammyTimer > 0f)
            {
                baseGuitarPlayer.WhammyTimer -= Time.deltaTime;
            }
            return true;
        }
    }
    /* MethodInfo method = typeof(BasePlayerWrapper).GetMethod("Update");
     DynamicMethod dm = new DynamicMethod("Update", null, new Type[] { typeof(BaseGuitarPlayer) }, typeof(BaseGuitarPlayer));
     ILGenerator gen = dm.GetILGenerator();
     gen.Emit(OpCodes.Ldarg_1);
     gen.Emit(OpCodes.Call, method);
     gen.Emit(OpCodes.Ret);

     var BaseFoo = (Action<BaseGuitarPlayer>)dm.CreateDelegate(typeof(Action<BaseGuitarPlayer>));
     BaseFoo(baseGuitarPlayer.BaseGuitarPlayer);



     /*if (baseGuitarPlayer.CastToBasePlayer().FCIndicator.activeInHierarchy)
         baseGuitarPlayer.CastToBasePlayer().FCIndicator.SetActive(false);*/

}
    //}
