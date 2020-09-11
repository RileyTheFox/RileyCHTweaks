using System;
using System.Collections.Generic;
using System.Text;
using Common.Wrappers.Attributes;

namespace Common.Wrappers
{
    [Wrapper(typeof(ComboColor))]
    internal struct ComboColorWrapper {
        public readonly ComboColor comboColor;

        public ComboColorWrapper(ComboColor comboColor)
        {
            this.comboColor = comboColor;
        }

    }
}
