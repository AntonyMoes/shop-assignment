using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts {
    public static class Utils {
        public static string GetScenePath(this Transform transform) {
            var current = transform;
            var path = new List<string>();

            while (current != null) {
                path.Add(current.name);
                current = current.parent;
            }

            path.Reverse();
            return string.Join("/", path);
        }
    }
}