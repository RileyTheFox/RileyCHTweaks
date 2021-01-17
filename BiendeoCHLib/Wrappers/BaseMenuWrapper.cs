using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BiendeoCHLib.Wrappers;
using BiendeoCHLib.Wrappers.Attributes;
using HarmonyLib;

namespace BiendeoCHLib.Wrappers
{
    [Wrapper(typeof(BaseMenu))]
    public struct BaseMenuWrapper
    {

        public BaseMenu BaseMenu { get; private set; }

        public static BaseMenuWrapper Wrap(BaseMenu baseMenu) => new BaseMenuWrapper
        {
            BaseMenu = baseMenu
        };

        public override bool Equals(object obj) => BaseMenu.Equals(obj);

        public override int GetHashCode() => BaseMenu.GetHashCode();

        public bool IsNull() => BaseMenu == null;

        #region Fields

        public CHPlayerWrapper ControllingPlayer
        {
            get => CHPlayerWrapper.Wrap(controllingPlayerField(BaseMenu));
            set => controllingPlayerField(BaseMenu) = value.CHPlayer;
        }
        [WrapperField("\u0315\u0311\u0311\u0314\u0317\u031C\u030E\u031A\u031A\u0315\u031B")]
        private static readonly AccessTools.FieldRef<BaseMenu, object> controllingPlayerField;

        public int MenuPosition
        {
            get => menuPositionField(BaseMenu);
            set => menuPositionField(BaseMenu) = value;
        }
        [WrapperField("\u031C\u030E\u030F\u0314\u030D\u0312\u031B\u030E\u030E\u0314\u0312")]
        private static readonly AccessTools.FieldRef<BaseMenu, int> menuPositionField;

        public int MaxMenuPosition
        {
            get => maxMenuPositionField(BaseMenu);
            set => maxMenuPositionField(BaseMenu) = value;
        }
        [WrapperField("\u0314\u0310\u031B\u030D\u030E\u0317\u0314\u031B\u030F\u031A\u0312")]
        private static readonly AccessTools.FieldRef<BaseMenu, int> maxMenuPositionField;

        public int TopOfMenu
        {
            get => topOfMenuField(BaseMenu);
            set => topOfMenuField(BaseMenu) = value;
        }
        //[WrapperField("\u0311\u0311\u031C\u0314\u031A\u030F\u0319\u031A\u0317\u031C\u031A")]
        [WrapperField("\u0311\u0314\u0314\u031B\u0312\u0313\u031A\u0318\u0315\u030E\u030D")]
        private static readonly AccessTools.FieldRef<BaseMenu, int> topOfMenuField;

        public int MaxTopOfMenu
        {
            get => maxTopOfMenuField(BaseMenu);
            set => maxTopOfMenuField(BaseMenu) = value;
        }
        [WrapperField("\u0311\u0311\u031C\u0314\u031A\u030F\u0319\u031A\u0317\u031C\u031A")]
        private static readonly AccessTools.FieldRef<BaseMenu, int> maxTopOfMenuField;

        public string[] MenuStrings
        {
            get => menuStringsField(BaseMenu);
            set => menuStringsField(BaseMenu) = value;
        }
        [WrapperField("menuStrings")]
        private static readonly AccessTools.FieldRef<BaseMenu, string[]> menuStringsField;

        #endregion

        #region Properties

        public string CurrentSelectionString
        {
            get => (string)currentSelectionStringProperty.GetValue(BaseMenu);
            set => currentSelectionStringProperty.SetValue(BaseMenu, value);
        }
        [WrapperProperty("\u031B\u031B\u030D\u0316\u0310\u0316\u0315\u0312\u0319\u0317\u0311")]
        private static readonly PropertyInfo currentSelectionStringProperty;

        #endregion

        #region Methods

        public void SetMenuProperties() => setMenuPropertiesMethod.Invoke(BaseMenu, null);
        [WrapperMethod("\u030E\u030E\u031A\u0318\u0316\u0314\u0310\u0319\u031A\u030D\u031A")]
        private static readonly FastInvokeHandler setMenuPropertiesMethod;

        public void JumpTo(string selection) => jumpToMethod.Invoke(BaseMenu, new object[] { selection });
        [WrapperMethod("\u0319\u0311\u0319\u0317\u0317\u031B\u031A\u0313\u0311\u031A\u030E", new Type[] { typeof(string) })]
        private static readonly FastInvokeHandler jumpToMethod;

        #endregion
    }
}
