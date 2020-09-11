using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Common.Wrappers.Attributes;

namespace Common.Wrappers
{
    [Wrapper(typeof(BaseGuitarPlayer))]
    internal struct BaseGuitarPlayerWrapper {
        public readonly BaseGuitarPlayer baseGuitarPlayer;

        #region Fields

        public float WhammyTimer
        {
            get => (float) whammyTimerField.GetValue(baseGuitarPlayer);
            set => whammyTimerField.SetValue(baseGuitarPlayer, value);
        }
        [WrapperField("\u0318\u030D\u0311\u0316\u0319\u0316\u0319\u030D\u0316\u0313\u0311")]
        private static readonly FieldInfo whammyTimerField;

        public float CurrentWhammy
        {
            get => (float)currentWhammyField.GetValue(baseGuitarPlayer);
            set => currentWhammyField.SetValue(baseGuitarPlayer, value);
        }
        [WrapperField("\u030D\u031A\u031A\u031B\u0311\u030F\u031B\u0319\u030F\u0319\u030D")]
        private static readonly FieldInfo currentWhammyField;

        public List<NoteWrapper> SustainNotes => ((ICollection)sustainNotesField.GetValue(baseGuitarPlayer))?.Cast<object>().Select(o => new NoteWrapper(o)).ToList();
        [WrapperField("\u031A\u0316\u0315\u0318\u0319\u0315\u0316\u0313\u0315\u0315\u0312")]
        private static readonly FieldInfo sustainNotesField;

        #endregion

        #region Methods

        public void CheckForHitNotes() => checkForHitNotesMethod.Invoke(baseGuitarPlayer, null);
        [WrapperMethod("\u0316\u0314\u030E\u0318\u0314\u0312\u030D\u0315\u030D\u0311\u030F")]
        private static readonly MethodInfo checkForHitNotesMethod;

        #endregion

        public BaseGuitarPlayerWrapper(BaseGuitarPlayer baseGuitarPlayer)
        {
            this.baseGuitarPlayer = baseGuitarPlayer;
        }
    }
}
