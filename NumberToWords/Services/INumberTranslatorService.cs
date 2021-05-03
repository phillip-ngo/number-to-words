using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace NumberToWords.Services
{
    /// <summary>
    /// Services that can be used to perform "Number to Words" translations.
    /// </summary>
    public interface INumberTranslatorService
    {
        /// <summary>
        /// Returns the maximum possible value that this INumberTranslatorService will support.
        /// </summary>
        /// <returns>Maximum value that this INumberTranslatorService can translate.</returns>
        public BigCurrency GetMaxValue();

        /// <summary>
        /// Translates the specified BigCurrency into its respective words equivalent.
        /// </summary>
        /// <param name="number">The BigCurrency value to translate.</param>
        /// <returns>String representation of the provided number; Could also potentially return any applicable error messages.</returns>
        public String Translate(BigCurrency number);
    }

    /// <summary>
    /// Representation of a large currency value. Only supports positive values with two decimal places.
    /// </summary>
    public class BigCurrency : IComparable<BigCurrency>
    {
        /// <summary>
        /// The integer component (dollars) of this BigCurrency instance
        /// </summary>
        public BigInteger IntegerComponent { get; set; }

        /// <summary>
        /// The fractional component (cents) of this BigCurrency instance
        /// </summary>
        public int FractionalComponent { get; set; }

        private BigCurrency(string number)
        {
            // Split number to their integer and fractional components
            var splitNumber = number.Split('.');
            IntegerComponent = splitNumber[0].Length > 0 ? BigInteger.Parse(splitNumber[0]) : 0;
            FractionalComponent = splitNumber.Length > 1 ? Convert.ToInt32(splitNumber[1].PadRight(2, '0')) : 0;

            // Perform some basic validation
            if (IntegerComponent.CompareTo(BigInteger.Zero) < 0 || FractionalComponent > 99 || FractionalComponent < 0)
            {
                throw new FormatException("Invalid Number: " + number);
            }
        }

        /// <summary>
        /// Indicates whether the value of this BigCurrency is equal to 0
        /// </summary>
        /// <returns>true if the value of the BigCurrency object is 0; otherwise, false.</returns>
        public bool IsZero()
        {
            return IntegerComponent.IsZero && FractionalComponent == 0;
        }

        /// <summary>
        /// Checks with this BigCurrency instance has any cent value.
        /// </summary>
        /// <returns>true if this has a cent value; otherwise false.</returns>
        public bool HasCents()
        {
            return FractionalComponent > 0;
        }

        /// <summary>
        /// Checks with this BigCurrency instance has any dollar value.
        /// </summary>
        /// <returns>true if this has a dollar value; otherwise false.</returns>
        public bool HasDollars()
        {
            return !IntegerComponent.IsZero;
        }

        /// <summary>
        /// Compares this BigCurrency instance with another.
        /// </summary>
        /// <param name="other">The other BigCurrency instance to compare this one to</param>
        /// <returns>A negative integer, zero, or a positive integer as this BigCurrency is less than, 
        /// equal to, or greater than the specified BigCurrency respectively.</returns>
        public int CompareTo([AllowNull] BigCurrency other)
        {
            int i = this.IntegerComponent.CompareTo(other.IntegerComponent);
            if (i == 0)
            {
                // FractionalComponent can only be < 100, so subtraction method is fine here 
                // (i.e. no chance of integer overflow errors)
                i = this.FractionalComponent - other.FractionalComponent;
            }
            return i;
        }

        /// <summary>
        /// Produces a string representation of this BigCurrency instance.
        /// </summary>
        /// <returns>String value of BigCurrency</returns>
        public override string ToString()
        {
            return IntegerComponent.ToString() + "." + Convert.ToString(FractionalComponent).PadLeft(2, '0');
        }

        /// <summary>
        /// Creates an instance of BigCurrency for the given number. The provided number string must 
        /// be of the format: ############.##
        /// </summary>
        /// <param name="number">Number to be represented by the new BigCurrency</param>
        /// <returns>BigCurrency representation of the given number</returns>
        /// <exception cref="System.FormatException">Thrown when the provided number string is of 
        /// the incorrect format.</exception>
        static public BigCurrency ValueOf(string number)
        {
            return new BigCurrency(number);
        }
    }
}
