using BepInEx;
using BiendeoCHLib;
using BiendeoCHLib.Patches;
using BiendeoCHLib.Patches.Attributes;
using BiendeoCHLib.Wrappers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GUI;


namespace CHelgaBotv23
{
	[BepInPlugin("co.uk.whereismyfurry.chelgabot", "CHelgaBot", "1.0")]
	[BepInDependency("com.biendeo.biendeochlib")]
	public class CHelgaBot : BaseUnityPlugin
    {
		public static CHelgaBot Instance { get; private set; }
		private Harmony Harmony;

		private bool sceneChanged;
		private float delta;
		private int lastCombo;
		private float lastWhammy = 0f;

		public CHelgaBot()
		{
			Instance = this;
			Harmony = new Harmony("co.uk.whereismyfurry.chelgabot");
			PatchBase.InitializePatches(Harmony, Assembly.GetExecutingAssembly(), Logger);
		}

		~CHelgaBot()
		{
			Harmony.UnpatchAll();
		}

		#region Variables

		public GameManagerWrapper gameManager;
		public GlobalVariablesWrapper globalVariables;
		public BasePlayerWrapper basePlayer;
		public BaseGuitarPlayerWrapper baseGuitarPlayer;

		#endregion

		#region UnityMethods

		private void Start()
		{
			SceneManager.activeSceneChanged += delegate (Scene _, Scene __)
			{
				sceneChanged = true;
			};
		}

		private void LateUpdate()
		{
			string sceneName = SceneManager.GetActiveScene().name;

			delta += Time.deltaTime;

			if (delta >= 0.2)
			{
				delta = 0;
			}

			if (this.sceneChanged)
			{
				sceneChanged = false;

				if (sceneName == "Gameplay")
				{
					var gameManagerObject = GameObject.Find("Game Manager");
					gameManager = GameManagerWrapper.Wrap(gameManagerObject.GetComponent<GameManager>());
					globalVariables = gameManager.GlobalVariables;

					basePlayer = gameManager.BasePlayers[0];

					basePlayer.SPBar.SPBar.gameObject.SetActive(true);
					basePlayer.ComboCounter.ComboColor.gameObject.SetActive(true);

					//Logger.LogInfo($"Bot Window: {basePlayer.windo}");
					Logger.LogInfo($"Early Act: {basePlayer.FrontHitWindow}");
					Logger.LogInfo($"Late Act: {basePlayer.BackHitWindow}");
					Logger.LogInfo($"Solo Act: {basePlayer.HitWindowLength}");
					Logger.LogInfo($"Video Offset: {globalVariables.OffsetsVideo.GetFloatSecond}");

					baseGuitarPlayer = BaseGuitarPlayerWrapper.Wrap(basePlayer.BasePlayer.GetComponent<BaseGuitarPlayer>());

					Logger.LogInfo($"Whammy Timer: {baseGuitarPlayer.WhammyTimer}");

					Logger.LogInfo($"Activation classes: {CHelgaChart.Charts.Count}");
					foreach(CHelgaChart chart in CHelgaChart.Charts)
					{
						Logger.LogInfo(chart.MoonChart.Notes.Length);
						foreach(CHelgaChart chart2 in CHelgaChart.Charts)
						{
							if ((chart.Activations == chart2.Activations) && chart != chart2)
								Logger.LogInfo("Duplicate charts");
						}
						/*Logger.LogInfo(chart.MoonChart.Song.Name);*/
						/*foreach(MoonChartWrapper moonChart in chart.MoonChart.Song.Charts)
						{
							if(moonChart.Notes == chart.MoonChart.Notes)
							{
								Logger.LogInfo("Duplicate charts");
							}
						}*/
					}
				}
			}
			else
			{
				if (sceneName == "Gameplay")
				{
					baseGuitarPlayer.WhammyTimer = 0f;
					baseGuitarPlayer.CurrentWhammy = 0f;

					/*if(basePlayer.Combo != lastCombo)
					{
						lastCombo = basePlayer.Combo;
						Debug.Log(baseGuitarPlayer.SustainNotes.Count);
					}

					bool anySustainsSP = false;
					foreach (NoteWrapper note in baseGuitarPlayer.SustainNotes)
					{
						if (note.IsStarPower && note.IsSustaining)
						{
							anySustainsSP = true;
							break;
						}
					}
					if (anySustainsSP && basePlayer.IsSPActive)
					{
						baseGuitarPlayer.WhammyTimer = 0.25f;
						if (delta == 0)
							baseGuitarPlayer.CurrentWhammy = 1f;
					}*/

					baseGuitarPlayer.WhammyTimer = 0.25f;
				}
			}
		}

        #endregion

        #region Patch Functions

		internal void CheckForHitNotes()
		{
			if (basePlayer.HittableNotesThisFrame > 0)
			{
				int i = 0;
				baseGuitarPlayer.HopoLenienceTimer = 0f;
				while (i < basePlayer.HittableNotesThisFrame)
				{
					NoteWrapper note = NoteWrapper.Wrap(basePlayer.HittableNotes[i]);
					basePlayer.MissNote(note);
				}
			}
		}

        #endregion
    }
}
