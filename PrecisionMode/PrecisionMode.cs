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
		// These values are the CH default's divided by a number to make them smaller.
		public const float PRECISION_STRUM_LENIENCE_AMOUNT_NO_NOTES = 0.048f / 2f;
		public const float PRECISION_STRUM_LENIENCE_AMOUNT = 0.084f / 2f;
		public const float PRECISION_HOPO_LENIENCE_AMOUNT = 0.096f / 2f;

		public const bool PRECISION_ANTI_GHOSTING_ENABLED = true;
		public const int PRECISION_ANTI_GHOSTING_LIMIT = 1;

		public int[] PlayerGhostCount = new int[4] {0, 0, 0, 0};
		public byte[] PlayerLastNoteMask = new byte[4] { 0, 0, 0, 0 };

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
			Instance = null;
		}

		#region Fields

		private bool sceneChanged = true;

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

		public int CountFrets(byte noteMask)
		{
			int count = 0;

			// Open note
			if((noteMask & 64) != 0)
			{
				return 64;
			}
			if((noteMask & 1) != 0)
			{
				count++;
			}
			if ((noteMask & 2) != 0)
			{
				count++;
			}
			if ((noteMask & 4) != 0)
			{
				count++;
			}
			if ((noteMask & 8) != 0)
			{
				count++;
			}
			if ((noteMask & 16) != 0)
			{
				count++;
			}
			return count;
		}

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
					for(int i = 0; i < 4; i++)
					{
						PlayerGhostCount[i] = 0;
					}
					var gameManagerObject = GameObject.Find("Game Manager");
					gameManager = GameManagerWrapper.Wrap(gameManagerObject.GetComponent<GameManager>());
					globalVariables = gameManager.GlobalVariables;

					basePlayer = gameManager.BasePlayers[0];

					Logger.LogInfo(basePlayer.Player.PlayerIndex);
					Logger.LogInfo(basePlayer.Notes.Count);
					Logger.LogInfo((int)basePlayer.Player.PlayerProfile.NoteModifier);
				}
			}
		}

		#endregion

	}
}
