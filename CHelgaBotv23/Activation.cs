using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiendeoCHLib.Wrappers;
using JetBrains.Annotations;

namespace CHelgaBotv23
{
    public enum ActivationType
    {
        ACTIVATE_SP,
        SQUEEZE,
        ACTIVATE_NOSQUEEZE,
        ACTIVATE_FUCKINGLATE,
        SQUEEZE_NOWINDOW,
        INCREASE_WINDOW,
        DECREASE_WINDOW,
        TINY_WINDOW,
        STOP_WHAMMY,
        START_WHAMMY
    };

    public class Activation
    {
        public ActivationType Type { get; }
        public uint Tick { get; }

        public bool Used { get; set; } = false;

        public NoteWrapper Note { get; set; }

        public Activation(ActivationType type, uint tick)
        {
            Type = type;
            Tick = tick;
        }

    }
}
