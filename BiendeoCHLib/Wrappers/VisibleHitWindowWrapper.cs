using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiendeoCHLib.Wrappers.Attributes;
using HarmonyLib;
using UnityEngine;

namespace BiendeoCHLib.Wrappers
{
    [Wrapper(typeof(VisibleHitWindow))]
    public struct VisibleHitWindowWrapper
    {

        public VisibleHitWindow VisibleHitWindow { get; private set; }

        public static VisibleHitWindowWrapper Wrap(VisibleHitWindow visibleHitWindow) => new VisibleHitWindowWrapper
        {
            VisibleHitWindow = visibleHitWindow
        };

        public override bool Equals(object obj) => VisibleHitWindow.Equals(obj);

        public override int GetHashCode() => VisibleHitWindow.GetHashCode();

        public bool IsNull() => VisibleHitWindow == null;

        #region Fields

        public float Win
        {
            get => winField(VisibleHitWindow);
            set => winField(VisibleHitWindow) = value;
        }
        [WrapperField("\u0318\u0311\u0315\u0315\u030F\u0317\u031C\u0319\u0317\u0316\u0315")]
        private static readonly AccessTools.FieldRef<VisibleHitWindow, float> winField;

        public Transform ThisTransform
        {
            get => thisTransformField(VisibleHitWindow);
            set => thisTransformField(VisibleHitWindow) = value;
        }
        [WrapperField("\u031C\u0315\u0310\u0310\u031C\u031B\u030E\u0312\u030E\u031C\u0319")]
        private static readonly AccessTools.FieldRef<VisibleHitWindow, Transform> thisTransformField;

        public CHPlayerWrapper Player
        {
            get => CHPlayerWrapper.Wrap(playerField(VisibleHitWindow));
            set => playerField(VisibleHitWindow) = value.CHPlayer;
        }
        [WrapperField("\u0317\u0319\u0316\u030E\u031A\u030E\u031A\u031A\u0319\u0311\u0318")]
        private static readonly AccessTools.FieldRef<VisibleHitWindow, object> playerField;

        #endregion

        #region Methods

        public void Start() => startMethod.Invoke(VisibleHitWindow);
        [WrapperMethod("Start")]
        private static readonly FastInvokeHandler startMethod;

        public void UpdateLength() => updateLengthMethod.Invoke(VisibleHitWindow);
        [WrapperMethod("\u030D\u0310\u030F\u030D\u0319\u031A\u0313\u0318\u0313\u0314\u0310")]
        private static readonly FastInvokeHandler updateLengthMethod;

        #endregion

    }
}
