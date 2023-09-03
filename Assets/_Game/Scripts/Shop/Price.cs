using UnityEngine;

namespace _Game.Scripts.Shop {
    public static class Price {
        public static int ApplyModifier(int price, float modifier) {
            return Mathf.CeilToInt(price * modifier);
        }
    }
}