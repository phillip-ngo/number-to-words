using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NumberToWords.Services
{
    /// <summary>
    /// An INumberTranslatorService implementation for the standard decimal numbering system.
    /// </summary>
    public class DecimalTranslatorServiceImpl : INumberTranslatorService
    {
        /// <summary>
        /// The maximum value that this INumberTranslatorService implementation can translate.
        /// </summary>
        public static readonly BigCurrency MAX_VALUE;

        private static readonly string[] ONES = new string[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
        private static readonly string[] TEENS = new string[] { "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        private static readonly string[] TENS = new string[] { "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        private static readonly List<String> LARGE_NUMBERS = new List<String>(12);
        private static readonly BigInteger ONE_THOUSAND = new BigInteger(1000);

        static DecimalTranslatorServiceImpl()
        {
            LARGE_NUMBERS.Add("Hundred");
            LARGE_NUMBERS.Add("Thousand");
            LARGE_NUMBERS.Add("Million");
            LARGE_NUMBERS.Add("Billion");
            LARGE_NUMBERS.Add("Trillion");
            LARGE_NUMBERS.Add("Quadrillion");
            LARGE_NUMBERS.Add("Quintillion");
            LARGE_NUMBERS.Add("Sextillion");
            LARGE_NUMBERS.Add("Septillion");
            LARGE_NUMBERS.Add("Octillion");
            LARGE_NUMBERS.Add("Nonillion");
            LARGE_NUMBERS.Add("Decillion");

            // NB: The maximum value allowed can be extended further by adding to the list of LARGE_NUMBERS above 
            // (be sure to remember to add to the i18n resources as well if applicable).
            MAX_VALUE = BigCurrency.ValueOf(new String('9', LARGE_NUMBERS.Count * 3) + ".99");
        }

        private readonly IStringLocalizer<DecimalTranslatorServiceImpl> _localizer;

        /// <summary>
        /// Creates a new instance of DecimalTranslatorServiceImpl.
        /// </summary>
        /// <param name="localizer">IStringLocalizer to use for localisation when applicable.</param>
        public DecimalTranslatorServiceImpl(IStringLocalizer<DecimalTranslatorServiceImpl> localizer)
        {
            _localizer = localizer;
        }

        public BigCurrency GetMaxValue()
        {
            return MAX_VALUE;
        }

        public String Translate(BigCurrency number)
        {
            if (number.CompareTo(MAX_VALUE) > 0)
            {
                return _localizer["Number is too large to handle"];
            }

            var result = new StringBuilder();
            if (number.IsZero())
            {
                result.Append(_localizer["Zero"] + " " + _localizer["Dollars"]);
            }
            else
            {
                // Process fractional component
                if (number.HasCents())
                {
                    var fractionalComponent = number.FractionalComponent;
                    result.Append(ToWords(fractionalComponent));
                    result.Append(" " + _localizer[fractionalComponent > 1 ? "Cents" : "Cent"]);
                }

                // Process integer component
                if (number.HasDollars())
                {
                    if (number.HasCents())
                    {
                        result.Insert(0, " " + _localizer["And"] + " ");
                    }

                    var integerComponent = number.IntegerComponent;
                    result.Insert(0, _localizer[integerComponent > 1 ? "Dollars" : "Dollar"]);

                    // Process and truncate the last three digits of the integer component (i.e. first 0 to 999)
                    integerComponent = BigInteger.DivRem(integerComponent, ONE_THOUSAND, out BigInteger remaninder);
                    if (remaninder > 0)
                    {
                        result.Insert(0, ToWords((int)remaninder) + " ");
                    }

                    // Process the remaining digits (i.e. 1000+); three digits at a time until none is left
                    var i = 1;
                    while (integerComponent > 0)
                    {
                        integerComponent = BigInteger.DivRem(integerComponent, ONE_THOUSAND, out remaninder);
                        if (remaninder > 0)
                        {
                            // Join each translated three digit token with the appropriate values from LARGE_NUMBERS.
                            result.Insert(0, " " + _localizer[LARGE_NUMBERS[i]] + " ");
                            result.Insert(0, ToWords((int)remaninder));
                        }
                        i++;
                    }
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Performs the translation for a three digit number to "words". Provided number must be less than 999.
        /// </summary>
        /// <param name="num">The number to be translated.</param>
        /// <returns>Translated "words" representation for the provided number.</returns>
        private string ToWords(int num)
        {
            var ones = num % 10;
            var tens = ((num - ones) % 100) / 10;
            var hundreds = (num - tens * 10 - ones) / 100;

            var result = new StringBuilder();

            if (hundreds > 0)
            {
                result.Append(_localizer[ONES[hundreds - 1]] + " " + _localizer[LARGE_NUMBERS[0]]);
                if (tens > 0 || ones > 0)
                {
                    result.Append(" " + _localizer["And"] + " ");
                }
            }

            if (tens == 1 && ones > 0)
            {
                result.Append(_localizer[TEENS[ones - 1]]);
            }
            else if (ones > 0)
            {
                if (tens > 0)
                {
                    result.Append(_localizer[TENS[tens - 1]] + "-");
                }
                result.Append(_localizer[ONES[ones - 1]]);
            }
            else if (tens > 0)
            {
                result.Append(_localizer[TENS[tens - 1]]);
            }
            return result.ToString();
        }
    }
}
