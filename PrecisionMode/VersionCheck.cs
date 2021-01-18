using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;
using BiendeoCHLib.Wrappers;
using UnityEngine;

namespace PrecisionMode
{
	public class VersionCheck : MonoBehaviour
	{
		private int windowId;
		private string assemblyName;
		public string AssemblyVersion;
		private Rect updateWindowRect;
		private static string latestVersion;

		private GUIStyle labelStyle;
		private GUIStyle buttonStyle;
		private GUIStyle windowStyle;

		public ConfigEntry<bool> SilenceUpdates;
		public bool HasVersionBeenChecked;
		public bool IsShowingUpdateWindow;

		public VersionCheck()
		{
			updateWindowRect = new Rect(Screen.width / 2 - 150.0f, Screen.height / 2 - 100.0f, 300.0f, 200.0f);
			HasVersionBeenChecked = false;
			IsShowingUpdateWindow = false;
			labelStyle = null;
			buttonStyle = null;
			windowStyle = null;
			latestVersion = null;
		}

		public void InitializeSettings(Assembly assembly, ConfigFile config)
		{
			AssemblyVersion = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
			assemblyName = new FileInfo(assembly.Location).Name;

			SilenceUpdates = config.Bind("VersionCheck", "SilenceUpdates", false, "If true, you will not be notified when a new version is available.");
		}

		public void Awake()
		{
			windowId = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		}

		public void Start()
		{
			if (!SilenceUpdates.Value && latestVersion == null)
			{
				string intendedVersion = GlobalVariablesWrapper.Instance.BuildVersion;
				try
				{
					using (var wc = new WebClient())
					{
						string versionsText = wc.DownloadString("https://raw.githubusercontent.com/RileyTheFox/RileyCHTweaks/master/versions.txt");
						var versions = versionsText.Split('\n').Select(l => l.Split('='));
						latestVersion = versions.Single(l => l[0] == intendedVersion)[1];

					}
				}
				catch (WebException)
				{
					// Any WebException could cause an error; since it's not really too vital for the tweak, it's simpler to just not prompt for an update.
				}
				catch (InvalidOperationException)
				{
					// This exception is thrown if the CH version isn't found in the versions list. Perhaps it should prompt the user that they're using an unsupported CH version?
				}
			}
			if (latestVersion != null)
			{
				IsShowingUpdateWindow = latestVersion != string.Join(".", AssemblyVersion.Split('.').Take(3));
				PrecisionMode.BepLog.LogDebug(latestVersion);
				PrecisionMode.BepLog.LogDebug(string.Join(".", AssemblyVersion.Split('.').Take(3)));
			}
			HasVersionBeenChecked = true;
		}

		public void OnGUI()
		{
			if (labelStyle == null || buttonStyle == null || windowStyle == null)
			{
				labelStyle = new GUIStyle(GUI.skin.label);
				buttonStyle = new GUIStyle(GUI.skin.button);
				windowStyle = new GUIStyle(GUI.skin.window);
				windowStyle.normal.background = MakeTex(2, 2, new Color(0.35f, 0.35f, 0.35f, 0.7f));
			}
			if (IsShowingUpdateWindow)
			{
				var r = GUILayout.Window(windowId, updateWindowRect, DrawWindow, new GUIContent($"Mod Updated Required!"), windowStyle);
				updateWindowRect.x = r.x;
				updateWindowRect.y = r.y;
			}
		}

		private void DrawWindow(int id)
		{
			var largeLabelStyle = new GUIStyle(labelStyle)
			{
				fontSize = 23,
				alignment = TextAnchor.MiddleCenter,
				normal = new GUIStyleState
				{
					textColor = Color.white,
				}
			};
			GUILayout.Label($"{assemblyName} has an update available!", largeLabelStyle);
			GUILayout.Label($"Please update to version {latestVersion}!", largeLabelStyle);
			if (GUILayout.Button("Open Update Page", buttonStyle))
			{
				Application.OpenURL($"https://github.com/RileyTheFox/RileyCHTweaks/releases/tag/v{latestVersion}");
			}
			if (GUILayout.Button("Close this Window", buttonStyle))
			{
				IsShowingUpdateWindow = false;
			}
			GUI.DragWindow();
		}

		private Texture2D MakeTex(int width, int height, Color col)
		{
			Color[] pix = new Color[width * height];
			for (int i = 0; i < pix.Length; ++i)
			{
				pix[i] = col;
			}
			Texture2D result = new Texture2D(width, height);
			result.SetPixels(pix);
			result.Apply();
			return result;
		}
	}
}
