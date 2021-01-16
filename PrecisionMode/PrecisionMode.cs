using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BiendeoCHLib.Patches;
using BiendeoCHLib.Wrappers;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PrecisionMode
{
    [BepInPlugin("co.uk.rileythefox.precisionmode", "PrecisionMode", "1.0.0")]
    [BepInDependency("com.biendeo.biendeochlib")]
    public class PrecisionMode : BaseUnityPlugin
    {

		// This is half of what the total window size will be
		// because the front end and back end are both set to this value
		// E.g 0.07f = 70ms, Front end = 70ms, Back end = 70ms
		// 70ms + 70ms = 140ms = CH default window size.
		public const float PRECISION_WINDOW_SIZE = 0.05f;

		public static PrecisionMode Instance { get; private set; }
		private Harmony Harmony;

		public PrecisionMode()
		{
			Instance = this;
			Harmony = new Harmony("co.uk.rileythefox.precisionmode");
			PatchBase.InitializePatches(Harmony, Assembly.GetExecutingAssembly(), Logger);
		}

		~PrecisionMode()
		{
			Harmony.UnpatchAll();
		}

		#region Fields

		private bool sceneChanged = false;

		private GameManagerWrapper gameManager;
		private GlobalVariablesWrapper globalVariables;
		private BasePlayerWrapper basePlayer;

		private GUIStyle settingsWindowStyle;
		private GUIStyle settingsToggleStyle;
		private GUIStyle settingsButtonStyle;
		private GUIStyle settingsTextAreaStyle;
		private GUIStyle settingsTextFieldStyle;
		private GUIStyle settingsLabelStyle;
		private GUIStyle settingsBoxStyle;
		private GUIStyle settingsHorizontalSliderStyle;
		private GUIStyle settingsHorizontalSliderThumbStyle;

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

			if (this.sceneChanged)
			{
				sceneChanged = false;

				if (sceneName == "Gameplay")
				{
					var gameManagerObject = GameObject.Find("Game Manager");
					gameManager = GameManagerWrapper.Wrap(gameManagerObject.GetComponent<GameManager>());
					globalVariables = gameManager.GlobalVariables;

					basePlayer = gameManager.BasePlayers[0];

					// Remove this when menu button is implemented
					basePlayer.Player.PlayerProfile.AddModifier(NoteWrapper.Modifier.Precision);
					if(basePlayer.Player.PlayerProfile.HasModifier(NoteWrapper.Modifier.Precision))
					{
						Logger.LogInfo("Precision Mode is enabled");
					}
				}
			}
		}

		#endregion

	}
}
