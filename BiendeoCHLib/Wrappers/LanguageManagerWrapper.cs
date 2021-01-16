using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiendeoCHLib.Wrappers.Attributes;
using HarmonyLib;

namespace BiendeoCHLib.Wrappers
{
    [Wrapper("\u031B\u031C\u030F\u0316\u0319\u0317\u031B\u0319\u0319\u0318\u0319")]
    public struct LanguageManagerWrapper
    {

		public object LanguageManager { get; private set; }

		public static LanguageManagerWrapper Wrap(object languageManager) => new LanguageManagerWrapper
		{
			LanguageManager = languageManager
		};

		public override bool Equals(object obj) => LanguageManager.Equals(obj);

		public override int GetHashCode() => LanguageManager.GetHashCode();

		public bool IsNull() => LanguageManager == null;

		#region Fields

		public static LanguageManagerWrapper Instance
		{
			get => Wrap(instanceField(null));
			set => instanceField(null) = value;
		}
		[WrapperField("\u0312\u0313\u0310\u0315\u030E\u0319\u030D\u0318\u0313\u030E\u031A")]
		private static readonly AccessTools.FieldRef<object, object> instanceField;

		#endregion

		#region Methods

		public string GetString(string key) => (string)getStringMethod.Invoke(LanguageManager, new object[] { key });
		[WrapperMethod("\u0318\u0318\u0314\u0311\u030E\u0318\u0319\u0311\u030D\u0317\u030E")]
		private static readonly FastInvokeHandler getStringMethod;

        #endregion

    }
}
