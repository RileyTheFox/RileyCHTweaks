using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
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
        #region Precision Mode Consts

        // This is half of what the total window size will be
        // because the front end and back end are both set to this value
        // E.g 0.07f = 70ms, Front end = 70ms, Back end = 70ms
        // 70ms + 70ms = 140ms = CH default window size.
        public const float PRECISION_WINDOW_SIZE = 0.05f;
		// These values are the CH default's divided by a number to make them smaller.
		public const float PRECISION_STRUM_LENIENCE_AMOUNT_NO_NOTES = 0.048f / 3f;
		public const float PRECISION_STRUM_LENIENCE_AMOUNT = 0.084f / 3f;
		public const float PRECISION_HOPO_LENIENCE_AMOUNT = 0.096f / 3f;

		public const bool PRECISION_ANTI_GHOSTING_ENABLED = true; // Currently unused but a feature may be implemented that toggles anti ghosting. Unsure yet.
		public const int PRECISION_ANTI_GHOSTING_LIMIT = 1;       // The amount of notes allowed to be ghosted per note that is hit.

		private int[] PlayerGhostCount = new int[4] {0, 0, 0, 0};
		public byte[] PlayerLastNoteMask = new byte[4] { 0, 0, 0, 0 };

        #endregion

        public static PrecisionMode Instance { get; private set; }
		public static ManualLogSource BepLog;
		private Harmony Harmony;

		public PrecisionMode()
		{
			Instance = this;
			BepLog = Logger;
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

		private VersionCheck versionCheck;

		private GameManagerWrapper gameManager;
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

		#region Methods

		// Method to make life easier adding to the ghost count (stops me from having to type out SetCount(player, GetCount() + number) all the time.
		public void AddGhostCountToPlayer(CHPlayerWrapper player, int count)
		{
			SetPlayerGhostCount(player, GetPlayerGhostCount(player) + count);
		}

		public void SetPlayerGhostCount(CHPlayerWrapper player, int count)
		{
			PlayerGhostCount[player.PlayerIndex] = count;
		}

		public int GetPlayerGhostCount(CHPlayerWrapper player)
		{
			return PlayerGhostCount[player.PlayerIndex];
		}

		public int CountFrets(byte noteMask)
		{
			int count = 0;

			// Open note - the game treats no frets held the same as the note mask being 64. It actually sets the buttons held to 64 representing an open note so they can be hit.
			if((noteMask & 64) != 0)
			{
				return 64;
			}
			int fretToTest = 1;
			while(fretToTest < 32)
			{
				// Bitwise & operator to test if the bit is set. If it is, add to the fret count.
				if((noteMask & fretToTest) != 0)
				{
					count++;
				}
				// Fret values are powers of 2 (1, 2, 4, 8, 16) so just multiply the current fret by 2 to get the next one.
				fretToTest *= 2;
			}
			return count;
		}

		public int CountFretsPressedThisFrame(byte noteMaskCurrentFrame, byte noteMaskLastFrame)
		{
			//print($"Current : {noteMaskCurrentFrame}. Last: {noteMaskLastFrame}");

			int count = 0;

			int fretToTest = 1;
			while(fretToTest < 32)
			{
				/*if(noteMaskCurrentFrame != 64)
				{
					if(fretToTest == 1 || fretToTest == 2)
					{
						print($"Entire mask: {noteMaskCurrentFrame}. Fret test: {fretToTest}. Operation result: {noteMaskCurrentFrame & fretToTest}");
						print($"Mask last frame: {noteMaskLastFrame}. Fret test: {fretToTest}. Operation result: {noteMaskLastFrame & fretToTest}");
					}
				}*/
				// Same code as above, but this time checking the last frame to see if it was previously not pressed, and now is.
				if ((noteMaskCurrentFrame & fretToTest) != 0 && (noteMaskLastFrame & fretToTest) == 0)
				{
					count++;
				}
				fretToTest *= 2;
			}
			//print(count);
			return count;
		}

        #endregion

        #region UnityMethods

		private void Awake()
		{
			versionCheck = gameObject.AddComponent<VersionCheck>();
			versionCheck.InitializeSettings(Assembly.GetExecutingAssembly(), Config);
		}

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
						PlayerLastNoteMask[i] = 0;
					}
					var gameManagerObject = GameObject.Find("Game Manager");
					gameManager = GameManagerWrapper.Wrap(gameManagerObject.GetComponent<GameManager>());

					/*basePlayer = gameManager.BasePlayers[0];

					Logger.LogInfo(basePlayer.Player.PlayerIndex);
					Logger.LogInfo((int)basePlayer.Player.PlayerProfile.NoteModifier);*/
				}
			}
		}

		#endregion

	}
}
