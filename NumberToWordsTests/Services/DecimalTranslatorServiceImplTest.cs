using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using NumberToWords.Services;
using Xunit;

namespace NumberToWordsTests.Services
{
    public class DecimalTranslatorServiceImplTest
    {

        [Fact]
        public void TestFractionalNumberOnly()
        {
            var localizer = new Mock<IStringLocalizer<DecimalTranslatorServiceImpl>>();
            localizer.Setup(l => l[It.IsAny<string>()])
                .Returns<string>(i => new LocalizedString(i, i));
            var subject = new DecimalTranslatorServiceImpl(localizer.Object);
            var actual = subject.Translate(BigCurrency.ValueOf(".12"));
            actual.Should().BeEquivalentTo("Twelve Cents");

            actual = subject.Translate(BigCurrency.ValueOf("0.12"));
            actual.Should().BeEquivalentTo("Twelve Cents");

            actual = subject.Translate(BigCurrency.ValueOf("0.02"));
            actual.Should().BeEquivalentTo("Two Cents");

            actual = subject.Translate(BigCurrency.ValueOf("0.2"));
            actual.Should().BeEquivalentTo("Twenty Cents");
        }

        [Fact]
        public void TestZeroDollars()
        {
            var localizer = new Mock<IStringLocalizer<DecimalTranslatorServiceImpl>>();
            localizer.Setup(l => l[It.IsAny<string>()])
                .Returns<string>(i => new LocalizedString(i, i));
            var subject = new DecimalTranslatorServiceImpl(localizer.Object);
            var actual = subject.Translate(BigCurrency.ValueOf("0"));
            actual.Should().BeEquivalentTo("Zero Dollars");
        }

        [Fact]
        public void TestSingleDigits()
        {
            var localizer = new Mock<IStringLocalizer<DecimalTranslatorServiceImpl>>();
            localizer.Setup(l => l[It.IsAny<string>()])
                .Returns<string>(i => new LocalizedString(i, i));
            var subject = new DecimalTranslatorServiceImpl(localizer.Object);
            var actual = subject.Translate(BigCurrency.ValueOf("1"));
            actual.Should().BeEquivalentTo("One Dollar");

            actual = subject.Translate(BigCurrency.ValueOf("2"));
            actual.Should().BeEquivalentTo("Two Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("3"));
            actual.Should().BeEquivalentTo("Three Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("4"));
            actual.Should().BeEquivalentTo("Four Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("5"));
            actual.Should().BeEquivalentTo("Five Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("6"));
            actual.Should().BeEquivalentTo("Six Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("7"));
            actual.Should().BeEquivalentTo("Seven Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("8"));
            actual.Should().BeEquivalentTo("Eight Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("9"));
            actual.Should().BeEquivalentTo("Nine Dollars");
        }

        [Fact]
        public void TestTeenNumbers()
        {
            var localizer = new Mock<IStringLocalizer<DecimalTranslatorServiceImpl>>();
            localizer.Setup(l => l[It.IsAny<string>()])
                .Returns<string>(i => new LocalizedString(i, i));
            var subject = new DecimalTranslatorServiceImpl(localizer.Object);
            var actual = subject.Translate(BigCurrency.ValueOf("11"));
            actual.Should().BeEquivalentTo("Eleven Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("12"));
            actual.Should().BeEquivalentTo("Twelve Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("13"));
            actual.Should().BeEquivalentTo("Thirteen Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("14"));
            actual.Should().BeEquivalentTo("Fourteen Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("15"));
            actual.Should().BeEquivalentTo("Fifteen Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("16"));
            actual.Should().BeEquivalentTo("Sixteen Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("17"));
            actual.Should().BeEquivalentTo("Seventeen Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("18"));
            actual.Should().BeEquivalentTo("Eighteen Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("19"));
            actual.Should().BeEquivalentTo("Nineteen Dollars");
        }

        [Fact]
        public void TestMultipleOfTenNumbers()
        {
            var localizer = new Mock<IStringLocalizer<DecimalTranslatorServiceImpl>>();
            localizer.Setup(l => l[It.IsAny<string>()])
                .Returns<string>(i => new LocalizedString(i, i));
            var subject = new DecimalTranslatorServiceImpl(localizer.Object);
            var actual = subject.Translate(BigCurrency.ValueOf("10"));
            actual.Should().BeEquivalentTo("Ten Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("20"));
            actual.Should().BeEquivalentTo("Twenty Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("30"));
            actual.Should().BeEquivalentTo("Thirty Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("40"));
            actual.Should().BeEquivalentTo("Forty Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("50"));
            actual.Should().BeEquivalentTo("Fifty Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("60"));
            actual.Should().BeEquivalentTo("Sixty Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("70"));
            actual.Should().BeEquivalentTo("Seventy Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("80"));
            actual.Should().BeEquivalentTo("Eighty Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("90"));
            actual.Should().BeEquivalentTo("Ninety Dollars");
        }

        [Fact]
        public void TestLargeNumbers()
        {
            var localizer = new Mock<IStringLocalizer<DecimalTranslatorServiceImpl>>();
            localizer.Setup(l => l[It.IsAny<string>()])
                .Returns<string>(i => new LocalizedString(i, i));
            var subject = new DecimalTranslatorServiceImpl(localizer.Object);
            var actual = subject.Translate(BigCurrency.ValueOf("100"));
            actual.Should().BeEquivalentTo("One Hundred Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("1000"));
            actual.Should().BeEquivalentTo("One Thousand Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("1000000"));
            actual.Should().BeEquivalentTo("One Million Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("1000000000"));
            actual.Should().BeEquivalentTo("One Billion Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("1000000000000"));
            actual.Should().BeEquivalentTo("One Trillion Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("1000000000000000"));
            actual.Should().BeEquivalentTo("One Quadrillion Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("1000000000000000000"));
            actual.Should().BeEquivalentTo("One Quintillion Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("1000000000000000000000"));
            actual.Should().BeEquivalentTo("One Sextillion Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("1000000000000000000000000"));
            actual.Should().BeEquivalentTo("One Septillion Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("1000000000000000000000000000"));
            actual.Should().BeEquivalentTo("One Octillion Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("1000000000000000000000000000000"));
            actual.Should().BeEquivalentTo("One Nonillion Dollars");

            actual = subject.Translate(BigCurrency.ValueOf("1000000000000000000000000000000000"));
            actual.Should().BeEquivalentTo("One Decillion Dollars");
        }

        [Fact]
        public void TestHundredAndOne()
        {
            var localizer = new Mock<IStringLocalizer<DecimalTranslatorServiceImpl>>();
            localizer.Setup(l => l[It.IsAny<string>()])
                .Returns<string>(i => new LocalizedString(i, i));
            var subject = new DecimalTranslatorServiceImpl(localizer.Object);
            var actual = subject.Translate(BigCurrency.ValueOf("101"));
            actual.Should().BeEquivalentTo("One Hundred And One Dollars");
        }

        [Fact]
        public void TestWithBothDollarsAndCents()
        {
            var localizer = new Mock<IStringLocalizer<DecimalTranslatorServiceImpl>>();
            localizer.Setup(l => l[It.IsAny<string>()])
                .Returns<string>(i => new LocalizedString(i, i));
            var subject = new DecimalTranslatorServiceImpl(localizer.Object);
            var actual = subject.Translate(BigCurrency.ValueOf("1001.27"));
            actual.Should().BeEquivalentTo("One Thousand One Dollars And Twenty-Seven Cents");
        }

        [Fact]
        public void TestMaxNumber()
        {
            var localizer = new Mock<IStringLocalizer<DecimalTranslatorServiceImpl>>();
            localizer.Setup(l => l[It.IsAny<string>()])
                .Returns<string>(i => new LocalizedString(i, i));
            var subject = new DecimalTranslatorServiceImpl(localizer.Object);
            var actual = subject.Translate(BigCurrency.ValueOf("999999999999999999999999999999999999.99"));
            actual.Should().BeEquivalentTo("Nine Hundred And Ninety-Nine Decillion " +
                "Nine Hundred And Ninety-Nine Nonillion " +
                "Nine Hundred And Ninety-Nine Octillion " +
                "Nine Hundred And Ninety-Nine Septillion " +
                "Nine Hundred And Ninety-Nine Sextillion " +
                "Nine Hundred And Ninety-Nine Quintillion " +
                "Nine Hundred And Ninety-Nine Quadrillion " +
                "Nine Hundred And Ninety-Nine Trillion " +
                "Nine Hundred And Ninety-Nine Billion " +
                "Nine Hundred And Ninety-Nine Million " +
                "Nine Hundred And Ninety-Nine Thousand " +
                "Nine Hundred And Ninety-Nine Dollars " +
                "And Ninety-Nine Cents");
        }

        [Fact]
        public void TestTooLargeNumber()
        {
            var localizer = new Mock<IStringLocalizer<DecimalTranslatorServiceImpl>>();
            localizer.Setup(l => l[It.IsAny<string>()])
                .Returns<string>(i => new LocalizedString(i, i));
            var subject = new DecimalTranslatorServiceImpl(localizer.Object);
            var actual = subject.Translate(BigCurrency.ValueOf("1999999999999999999999999999999999999.99"));
            actual.Should().BeEquivalentTo("Number is too large to handle");
        }
    }
}
