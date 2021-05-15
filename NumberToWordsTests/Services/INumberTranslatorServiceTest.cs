using FluentAssertions;
using NumberToWords.Services;
using System;
using System.Numerics;
using Xunit;
using static FluentAssertions.FluentActions;

namespace NumberToWordsTests.Services
{
    public class INumberTranslatorServiceTest
    {
        [Fact]
        public void TestInvalidNumber()
        {
            Invoking(() => BigCurrency.ValueOf("abc")).Should().Throw<FormatException>();
            Invoking(() => BigCurrency.ValueOf("0.2000")).Should().Throw<FormatException>();
            Invoking(() => BigCurrency.ValueOf("-10")).Should().Throw<FormatException>();
            Invoking(() => BigCurrency.ValueOf("10.-10")).Should().Throw<FormatException>();
            Invoking(() => BigCurrency.ValueOf("10.aa")).Should().Throw<FormatException>();
        }

        [Fact]
        public void TestWithNoCents()
        {
            var actual = BigCurrency.ValueOf("1");
            actual.IntegerComponent.Should().BeEquivalentTo(BigInteger.One);
            actual.FractionalComponent.Should().Be(0);

            actual = BigCurrency.ValueOf("10");
            actual.IntegerComponent.Should().BeEquivalentTo(new BigInteger(10));
            actual.FractionalComponent.Should().Be(0);
        }

        [Fact]
        public void TestWithNoDollars()
        {
            var actual = BigCurrency.ValueOf("0.1");
            actual.IntegerComponent.Should().BeEquivalentTo(BigInteger.Zero);
            actual.FractionalComponent.Should().Be(10);

            actual = BigCurrency.ValueOf("0.01");
            actual.IntegerComponent.Should().BeEquivalentTo(BigInteger.Zero);
            actual.FractionalComponent.Should().Be(1);
        }

        [Fact]
        public void TestWithDollarsAndCents()
        {
            var actual = BigCurrency.ValueOf("100.01");
            actual.IntegerComponent.Should().BeEquivalentTo(new BigInteger(100));
            actual.FractionalComponent.Should().Be(1);
        }

        [Fact]
        public void TestZero()
        {
            var actual = BigCurrency.ValueOf("0");
            actual.IntegerComponent.Should().BeEquivalentTo(BigInteger.Zero);
            actual.FractionalComponent.Should().Be(0);

            actual = BigCurrency.ValueOf("0.0");
            actual.IntegerComponent.Should().BeEquivalentTo(BigInteger.Zero);
            actual.FractionalComponent.Should().Be(0);

            actual = BigCurrency.ValueOf("0.00");
            actual.IntegerComponent.Should().BeEquivalentTo(BigInteger.Zero);
            actual.FractionalComponent.Should().Be(0);

            actual = BigCurrency.ValueOf("000.0");
            actual.IntegerComponent.Should().BeEquivalentTo(BigInteger.Zero);
            actual.FractionalComponent.Should().Be(0);
        }

        [Fact]
        public void TestReallyBigValue()
        {
            var actual = BigCurrency.ValueOf("9999999999999999999999999999999999999999999999999999999.99");
            actual.IntegerComponent.Should().BeEquivalentTo(BigInteger.Parse("9999999999999999999999999999999999999999999999999999999"));
            actual.FractionalComponent.Should().Be(99);
        }

        [Fact]
        public void TestCompareTo()
        {
            var subject = BigCurrency.ValueOf("100.01");
            subject.CompareTo(BigCurrency.ValueOf("100.01")).Should().Be(0);
            subject.CompareTo(BigCurrency.ValueOf("100")).Should().BeGreaterThan(0);
            subject.CompareTo(BigCurrency.ValueOf("100.02")).Should().BeLessThan(0);
            subject.CompareTo(BigCurrency.ValueOf("99.01")).Should().BeGreaterThan(0);
            subject.CompareTo(BigCurrency.ValueOf("101.01")).Should().BeLessThan(0);
        }

        [Fact]
        public void TestToString()
        {
            var subject = BigCurrency.ValueOf("100.01");
            subject.ToString().Should().Be("100.01");

            subject = BigCurrency.ValueOf("0100.1");
            subject.ToString().Should().Be("100.10");
        }
    }
}
