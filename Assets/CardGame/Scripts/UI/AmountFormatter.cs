using System.Globalization;

namespace CardGame.UI
{
    public static class AmountFormatter
    {
        private const int Thousand = 1000;
        private const string AmountPrefix = "x";
        private const string ThousandsSuffix = "K";
        private const string ThousandsFormat = "0.#";
        private const string ExactFormat = "N0";

        public static string Format(int amount) {
            if (amount < Thousand) return AmountPrefix + amount;

            float thousands = (float)amount / Thousand;
            return AmountPrefix + thousands.ToString(ThousandsFormat, CultureInfo.InvariantCulture) + ThousandsSuffix;
        }

        public static string Exact(int amount) {
            return amount.ToString(ExactFormat, CultureInfo.InvariantCulture);
        }
    }
}
