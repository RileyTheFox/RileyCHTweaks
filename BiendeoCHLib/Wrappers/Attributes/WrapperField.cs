﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BiendeoCHLib.Wrappers.Attributes {
	public sealed class WrapperField : Attribute {
		public readonly string ObfuscatedName;

		public WrapperField(string obfuscatedName) {
			ObfuscatedName = obfuscatedName;
		}
	}
}
