using BiendeoCHLib.Wrappers.Attributes;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BiendeoCHLib.Wrappers {
	[Wrapper("\u0311\u0316\u0315\u031B\u0310\u0314\u0316\u030E\u0311\u0315\u031B")]
	public struct PlayerProfileWrapper {
		public object PlayerProfile { get; private set; }

		public static PlayerProfileWrapper Wrap(object playerProfile) => new PlayerProfileWrapper {
			PlayerProfile = playerProfile
		};

		public override bool Equals(object obj) => PlayerProfile.Equals(obj);

		public override int GetHashCode() => PlayerProfile.GetHashCode();

		public bool IsNull() => PlayerProfile == null;

		#region Fields

		public string PlayerName {
			get => playerNameField(PlayerProfile);
			set => playerNameField(PlayerProfile) = value;
		}
		[WrapperField("\u031A\u0311\u0312\u030D\u0315\u0310\u0311\u0316\u030D\u0311\u0312")]
		private static readonly AccessTools.FieldRef<object, string> playerNameField;

		public ControllerType Instrument {
			get => (ControllerType)instrumentField(PlayerProfile);
			set => instrumentField(PlayerProfile) = value;
		}
		[WrapperField("\u030E\u031C\u0314\u031B\u030E\u031A\u0313\u030D\u0314\u0310\u031A")]
		private static readonly AccessTools.FieldRef<object, object> instrumentField;

		public Difficulty Difficulty {
			get => (Difficulty)difficultyField(PlayerProfile);
			set => difficultyField(PlayerProfile) = value;
		}
		[WrapperField("\u030E\u0310\u0312\u031C\u0314\u031A\u030E\u031A\u0312\u0318\u030E")]
		private static readonly AccessTools.FieldRef<object, object> difficultyField;

		public bool Bot
		{
			get => botField(PlayerProfile);
			set => botField(PlayerProfile) = value;
		}
		[WrapperField("\u030F\u031B\u0312\u031B\u0314\u0313\u031A\u0316\u031B\u0316\u0312")]
		private static readonly AccessTools.FieldRef<object, bool> botField;

		public NoteWrapper.Modifier NoteModifier
		{
			get => (NoteWrapper.Modifier)noteModifierField(PlayerProfile);
			set => noteModifierField(PlayerProfile) = value;
		}
		[WrapperField("\u030E\u0315\u0317\u030F\u0312\u0316\u0313\u0311\u030E\u030D\u0318")]
		private static readonly AccessTools.FieldRef<object, object> noteModifierField;

		public GameSettingWrapper NoteSpeed
		{
			get => GameSettingWrapper.Wrap(noteSpeedField(PlayerProfile));
			set => noteSpeedField(PlayerProfile) = value.GameSetting;
		}
		[WrapperField("\u0313\u030E\u0314\u0318\u0317\u031A\u0315\u031C\u0313\u0316\u0312")]
		private static readonly AccessTools.FieldRef<object, object> noteSpeedField;

		#endregion

		#region Methods

		public static string ModifierString(NoteWrapper.Modifier modifier) => (string)modifierStringMethod.Invoke(null, new object[] { modifier });
		[WrapperMethod("\u0312\u030D\u031A\u0315\u0315\u0314\u0318\u031A\u030D\u0314\u0318")]
		private static readonly FastInvokeHandler modifierStringMethod;

		public void AddModifier(NoteWrapper.Modifier modifier) => addModifierMethod.Invoke(PlayerProfile, new object[] { modifier });
		[WrapperMethod("\u0311\u030D\u031B\u030F\u0315\u031C\u031A\u0314\u0319\u031C\u0314")]
		private static readonly FastInvokeHandler addModifierMethod;

		public void AddModChartModifier(NoteWrapper.Modifier modifier) => addModChartModifierMethod.Invoke(PlayerProfile, new object[] { modifier });
		[WrapperMethod("\u030E\u0311\u030D\u031B\u0314\u0312\u031A\u0314\u0319\u031A\u0318")]
		private static readonly FastInvokeHandler addModChartModifierMethod;

		public bool HasModifier(NoteWrapper.Modifier modifier) => (bool)hasModifierMethod.Invoke(PlayerProfile, new object[] { modifier });
		[WrapperMethod("\u0313\u0319\u0312\u031C\u0316\u031A\u031B\u0311\u0312\u0319\u0312")]
		private static readonly FastInvokeHandler hasModifierMethod;

		#endregion

		#region Enumerations

		public enum ControllerType : byte {
			Guitar,
			GHLGuitar,
			Drums
		}

		#endregion
	}
}
