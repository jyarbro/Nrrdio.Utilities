﻿using System.Linq;

namespace Nrrdio.Utilities.Text {
    public static class PascalToSnakeExtension {
        public static string PascalToSnake(this string value) => string.Concat(value.Select((c, i) => i > 0 && char.IsUpper(c) ? $"_{c}" : $"{c}")).ToLower();
    }
}
