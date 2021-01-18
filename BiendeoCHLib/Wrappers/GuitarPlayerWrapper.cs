using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiendeoCHLib.Wrappers.Attributes;
using HarmonyLib;

namespace BiendeoCHLib.Wrappers
{
    [Wrapper(typeof(GuitarPlayer))]
    public struct GuitarPlayerWrapper
    {

		public GuitarPlayer GuitarPlayer { get; private set; }

		public static GuitarPlayerWrapper Wrap(GuitarPlayer guitarPlayer) => new GuitarPlayerWrapper
		{
			GuitarPlayer = guitarPlayer
		};

		public override bool Equals(object obj) => GuitarPlayer.Equals(obj);

		public override int GetHashCode() => GuitarPlayer.GetHashCode();

		public bool IsNull() => GuitarPlayer == null;

		#region Casts

		public BaseGuitarPlayerWrapper CastToBaseGuitarPlayer() => BaseGuitarPlayerWrapper.Wrap(GuitarPlayer as BaseGuitarPlayer);

		#endregion

		#region Fields

		#endregion

		#region Methods

		public void UpdateInput() => updateInputMethod(GuitarPlayer);
		[WrapperMethod("\u030F\u0313\u031B\u0317\u0319\u0314\u0318\u030D\u0317\u030F\u030D")]
		private static readonly FastInvokeHandler updateInputMethod;

        #endregion
    }
}
