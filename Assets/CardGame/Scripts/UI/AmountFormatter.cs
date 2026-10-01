using System.Globalization;

namespace CardGame.UI
{
    public static class AmountFormatter
    {
        private const int Thousand = 1000;

        public static string Format(int amount) {
            if (amount < Thousand) return "x" + amount;

            float thousands = (float)amount / Thousand;
            return "x" + thousands.ToString("0.#", CultureInfo.InvariantCulture) + "K";
        }
    }
}
