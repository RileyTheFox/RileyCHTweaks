using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GUI;
using Common.Wrappers;
using Common.Settings;

namespace PlugHero
{
	public class PlugHeroTweak : MonoBehaviour
	{
		public PlugClient chPlug;
		private bool sceneChanged;

		private bool configWindowEnabled;

		private GUIStyle settingsWindowStyle;
		private GUIStyle settingsToggleStyle;
		private GUIStyle settingsButtonStyle;
		private GUIStyle settingsTextAreaStyle;
		private GUIStyle settingsTextFieldStyle;
		private GUIStyle settingsLabelStyle;
		private GUIStyle settingsBoxStyle;
		private GUIStyle settingsHorizontalSliderStyle;
		private GUIStyle settingsHorizontalSliderThumbStyle;

		private GUIConfigurationStyles styles;

		private float delta;

		#region GameplayVariables

		// Plug Variables
		int currentVibration = 5; // Starting: 5%
		const int startingVibration = 5;

		// Game Manager
		private GameObject gameManagerObject;
		private GameManagerWrapper gameManager;

		// Player Variables
		private BasePlayerWrapper basePlayer;
		private int comboPreviousFrame = 0;
		private int highestStreak = 0;

		private bool streakMessageSent = true;

		// Song Variables
		int songPercentageComplete;

		#endregion

		#region Unity Methods

		private void Start()
		{
			SceneManager.activeSceneChanged += delegate (Scene _, Scene __)
			{
				sceneChanged = true;
			};

			chPlug = new PlugClient();

			var largeLabelStyle = new GUIStyle
			{
				fontSize = 20,
				alignment = TextAnchor.UpperLeft,
				fontStyle = FontStyle.Bold,
				normal = new GUIStyleState
				{
					textColor = Color.white,
				}
			};
			var smallLabelStyle = new GUIStyle
			{
				fontSize = 14,
				alignment = TextAnchor.UpperLeft,
				normal = new GUIStyleState
				{
					textColor = Color.white,
				}
			};

			styles = new Common.Settings.GUIConfigurationStyles
			{
				LargeLabel = largeLabelStyle,
				SmallLabel = smallLabelStyle,
				Window = settingsWindowStyle,
				Toggle = settingsToggleStyle,
				Button = settingsButtonStyle,
				TextArea = settingsTextAreaStyle,
				TextField = settingsTextFieldStyle,
				Label = settingsLabelStyle,
				Box = settingsBoxStyle,
				HorizontalSlider = settingsHorizontalSliderStyle,
				HorizontalSliderThumb = settingsHorizontalSliderThumbStyle
			};
		}

		private void LateUpdate()
		{
			delta += Time.deltaTime;
			string sceneName = SceneManager.GetActiveScene().name;

			if(delta >= 1)
			{
				delta = 0;
			}

			if (this.sceneChanged)
			{
				this.sceneChanged = false;
				if(sceneName == "Gameplay")
				{
					configWindowEnabled = false;

					gameManagerObject = GameObject.Find("Game Manager");
					gameManager = new GameManagerWrapper(gameManagerObject.GetComponent<GameManager>());

					basePlayer = gameManager.BasePlayers[0];

					songPercentageComplete = 0;
					currentVibration = startingVibration;
					highestStreak = 0;
					comboPreviousFrame = 0;

					Debug.Log("Setup Variables");
				}
			}
			if(sceneName == "Gameplay")
			{
				if (HasVibrationPercentageIncreased())
					chPlug.SendPlugMessage(PlugMessageType.SONG_PERCENTAGE, Convert.ToByte(currentVibration));
				if (HasPlayerMissed())
					chPlug.SendPlugMessage(PlugMessageType.NOTE_MISSED);
				if (HasBeatenPreviousNoteStreak())
					chPlug.SendPlugMessage(PlugMessageType.HIGHEST_STREAK, highestStreak);

				comboPreviousFrame = basePlayer.Combo;
			}

			// Open configuration menu
			if (Input.GetKeyDown(KeyCode.P) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
			{
				configWindowEnabled = !configWindowEnabled;
			}
		}

		void OnGUI()
		{
			if (settingsWindowStyle is null)
			{
				settingsWindowStyle = new GUIStyle(GUI.skin.window);
				settingsToggleStyle = new GUIStyle(GUI.skin.toggle);
				settingsButtonStyle = new GUIStyle(GUI.skin.button);
				settingsTextAreaStyle = new GUIStyle(GUI.skin.textArea);
				settingsTextFieldStyle = new GUIStyle(GUI.skin.textField);
				settingsLabelStyle = new GUIStyle(GUI.skin.label);
				settingsBoxStyle = new GUIStyle(GUI.skin.box);
				settingsHorizontalSliderStyle = new GUIStyle(GUI.skin.horizontalSlider);
				settingsHorizontalSliderThumbStyle = new GUIStyle(GUI.skin.horizontalSliderThumb);
			}
			if (configWindowEnabled)
			{
				var outputRect = GUILayout.Window(187004001, new Rect(100, 100, 400.0f, 250.0f), OnWindow, new GUIContent("PlugHero Settings"), settingsWindowStyle);
			}
		}

        #endregion

        #region PlugConditions

		public bool HasVibrationPercentageIncreased()
		{
			bool increase = false;

			int newPercentage = (int)Math.Truncate(gameManager.SongTime / gameManager.SongLength * 100);

			// vibration = (15 * songPercentage) / 18 + 5

			currentVibration = (15 * songPercentageComplete) / 18 + 5;

			if(newPercentage != songPercentageComplete)
			{
				songPercentageComplete = newPercentage;
				increase = true;
			}

			return increase;
		}

		public bool HasBeatenPreviousNoteStreak()
		{
			if(basePlayer.Combo > highestStreak && !streakMessageSent)
			{
				highestStreak = basePlayer.Combo;
				streakMessageSent = true;

				return true;
			}
			return false;
		}

		public bool HasPlayerMissed()
		{
			if (basePlayer.Combo == 0 && comboPreviousFrame != 0)
			{
				if (comboPreviousFrame > highestStreak)
					highestStreak = comboPreviousFrame;

				streakMessageSent = false;
				return true;
			}

			return false;
		}

        #endregion

        private void OnWindow(int id)
		{
			if (!chPlug.Socket.IsAlive)
			{
				GUILayout.Label("Not Connected");

				if (GUILayout.Button("Connect to Server", settingsButtonStyle))
				{
					chPlug.Socket.ConnectAsync();
				}
			}

			else
			{
				GUILayout.Label("Connected");

				if (GUILayout.Button("Disconnect from Server", settingsButtonStyle))
				{
					chPlug.Socket.CloseAsync();
				}
			}

			GUILayout.Space(25.0f);

			GUILayout.Label($"PlugHero v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}");
			GUILayout.Label("by RileyTheFox");
			GUI.DragWindow();
		}

	}
}
