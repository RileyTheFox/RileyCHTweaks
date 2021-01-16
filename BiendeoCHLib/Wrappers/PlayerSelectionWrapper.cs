using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BiendeoCHLib.Wrappers;
using BiendeoCHLib.Wrappers.Attributes;
using HarmonyLib;
using TMPro;
using UnityEngine.UI;

namespace BiendeoCHLib.Wrappers
{
    [Wrapper(typeof(PlayerSelection))]
    public struct PlayerSelectionWrapper
    {
        public PlayerSelection PlayerSelection { get; private set; }

        public static PlayerSelectionWrapper Wrap(PlayerSelection playerSelection) => new PlayerSelectionWrapper
        {
            PlayerSelection = playerSelection
        };

        public override bool Equals(object obj) => PlayerSelection.Equals(obj);

        public override int GetHashCode() => PlayerSelection.GetHashCode();

        public bool IsNull() => PlayerSelection == null;

        #region Casts

        public BaseMenuWrapper CastToBaseMenu() => BaseMenuWrapper.Wrap(PlayerSelection);

        #endregion

        #region Fields

        public TextMeshProUGUI Header
        {
            get => headerField(PlayerSelection);
            set => headerField(PlayerSelection) = value;
        }
        [WrapperField("header")]
        private static readonly AccessTools.FieldRef<PlayerSelection, TextMeshProUGUI> headerField;

        public Image[] ModifiersChosen
        {
            get => modifiersChosenFields(PlayerSelection);
            set => modifiersChosenFields(PlayerSelection) = value;
        }
        [WrapperField("modifiersChosen")]
        private static readonly AccessTools.FieldRef<PlayerSelection, Image[]> modifiersChosenFields;

        public bool UnknownBool1
        {
            get => unknownBool1Field(PlayerSelection);
            set => unknownBool1Field(PlayerSelection) = value;
        }
        [WrapperField("\u0313\u0315\u0319\u030D\u031C\u0313\u0318\u031C\u031C\u0314\u0316")]
        private static readonly AccessTools.FieldRef<PlayerSelection, bool> unknownBool1Field;

        public int UsedMenuStrings
        {
            get => usedMenuStringsField(PlayerSelection);
            set => usedMenuStringsField(PlayerSelection) = value;
        }
        [WrapperField("\u0310\u0315\u0315\u0315\u0317\u0319\u031A\u030E\u031A\u0315\u0311")]
        private static readonly AccessTools.FieldRef<PlayerSelection, int> usedMenuStringsField;

        public MenuState State
        {
            get => (MenuState)stateField(PlayerSelection);
            set => stateField(PlayerSelection) = value;
        }
        [WrapperField("\u0312\u0312\u0313\u030F\u0318\u031B\u0310\u030E\u030F\u0317\u0315")]
        private static readonly AccessTools.FieldRef<PlayerSelection, object> stateField;

        #endregion

        #region Methods

        public void ShowReady() => showReadyMethod.Invoke(PlayerSelection, null);
        [WrapperMethod("\u030D\u031B\u0314\u030F\u0313\u031A\u0313\u0316\u0318\u0310\u031B")]
        private static FastInvokeHandler showReadyMethod;

        public void ShowModifiers() => showModifiersMethod.Invoke(PlayerSelection, null);
        [WrapperMethod("\u031A\u030D\u0315\u031B\u031C\u0316\u0312\u0312\u030D\u0310\u031A")]
        private static readonly FastInvokeHandler showModifiersMethod;

        public void ShowChosenModifiers() => showChosenModifiersMethod.Invoke(PlayerSelection, null);
        [WrapperMethod("\u030E\u031B\u0319\u0318\u0316\u0319\u0316\u031A\u0319\u0313\u031A")]
        private static readonly FastInvokeHandler showChosenModifiersMethod;

        public void ChooseModifier() => chooseModifierMethod.Invoke(PlayerSelection, null);
        [WrapperMethod("\u0313\u0313\u0314\u030F\u0313\u0312\u0318\u031A\u0311\u030D\u031A")]
        private static readonly FastInvokeHandler chooseModifierMethod;

        public void AddMenuString(string addString) => addMenuStringMethod.Invoke(PlayerSelection, new object[] { addString });
        [WrapperMethod("\u0314\u0312\u0313\u0312\u030D\u0310\u031C\u0312\u031C\u031C\u030F")]
        private static readonly FastInvokeHandler addMenuStringMethod;

        public void ClearMenuStrings() => clearMenuStringsMethod.Invoke(PlayerSelection, null);
        [WrapperMethod("\u0319\u030F\u0313\u0314\u031A\u0313\u0318\u0315\u031B\u030F\u031B")]
        private static readonly FastInvokeHandler clearMenuStringsMethod;

        public void SetMenuProperties() => setMenuPropertiesMethod.Invoke(PlayerSelection, null);
        [WrapperMethod("\u030E\u030E\u031A\u0318\u0316\u0314\u0310\u0319\u031A\u030D\u031A")]
        private static readonly FastInvokeHandler setMenuPropertiesMethod;

        #endregion

        #region Enumerations

        public enum MenuState
        {
            ChoosingInstrument,
            ChoosingDifficulty,
            ChoosingModifier,
            Ready
        }

        #endregion
    }
}
