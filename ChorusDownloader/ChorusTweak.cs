using Common.Settings;
using Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace ChorusDownloader
{
    public class ChorusTweak : MonoBehaviour
    {
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

		private bool isSearching = false;
		dynamic chorusJson = null;

		// Game Manager
		private GameObject gameManagerObject;
		private GameManagerWrapper gameManager;

		#region Unity Methods

		private void Start()
		{
			SceneManager.activeSceneChanged += delegate (Scene _, Scene __)
			{
				sceneChanged = true;
			};

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

			if (delta >= 1)
			{
				delta = 0;
			}

			if (this.sceneChanged)
			{
				this.sceneChanged = false;
				if (sceneName == "Gameplay")
				{
					configWindowEnabled = false;

					gameManagerObject = GameObject.Find("Game Manager");
					gameManager = new GameManagerWrapper(gameManagerObject.GetComponent<GameManager>());

					Debug.Log("Setup Variables");
				}
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
				var outputRect = GUILayout.Window(187004001, new Rect(100, 100, 400.0f, 250.0f), OnWindow, new GUIContent("ChorusDownloader"), settingsWindowStyle);
			}
		}

		#endregion

		private void OnWindow(int id)
		{
			string search = "";

			search = GUILayout.TextArea(search, short.MaxValue, settingsTextFieldStyle);

			if (GUILayout.Button("Search Chorus", settingsButtonStyle) && search != "" && !isSearching)
			{
				isSearching = true;
				Task.Run(async () =>
				{
					try
					{
						var uri = HttpUtility.UrlEncode("http://chorus.fightthe.pw/api/search?query=" + search);
						HttpWebRequest request = HttpWebRequest.CreateHttp(uri);
						HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

						if (response.StatusCode != HttpStatusCode.OK)
						{
							chorusJson = null;
							return;
						}

						Debug.Log("Ok");

						using (StreamReader reader = new StreamReader(response.GetResponseStream()))
						{
							string json = await reader.ReadToEndAsync();

							chorusJson = JsonConvert.DeserializeObject(json);
							Debug.Log("Read data");
						}
					}
					catch (Exception e)
					{
						Debug.Log(e.Message);
						Debug.Log(e.StackTrace);
					}
					isSearching = false;
				});
			}

			GUILayout.Label(JsonConvert.SerializeObject(chorusJson));

			GUILayout.Space(25.0f);

			/*if(chorusJson != null)
			{
				dynamic songsArray = chorusJson.songs;

				foreach(dynamic song in songsArray)
				{
					GUILayout.Label(song.name);
				}
			}*/

			GUILayout.Space(25.0f);

			GUILayout.Label($"ChorusDownloader v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}");
			GUILayout.Label("by RileyTheFox");
			GUI.DragWindow(new Rect(0, 0, 10000, 100));
		}

	}
}
