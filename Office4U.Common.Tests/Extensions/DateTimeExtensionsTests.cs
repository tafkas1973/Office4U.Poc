using NUnit.Framework;
using System;

namespace Office4U.Common.Extensions
{
    public class DateTimeExtensionsTests
    {
        [Test]
        public void CalculateAge_WithAGivenNewYearsDay_ReturnsCorrectAge()
        {
            //Arrange
            var testDate = new DateTime(1973, 01, 01);
            var expectedResult = DateTime.Now.Year - testDate.Year;

            //Act
            var result = DateTimeExtensions.CalculateAge(testDate);

            //Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CalculateAge_WithAGivenSylvestersDay_ReturnsCorrectAge()
        {
            //Arrange
            var testDate = new DateTime(1973, 12, 31);
            var expectedResult = DateTime.Now.Year - testDate.Year - 1;

            //Act
            var result = DateTimeExtensions.CalculateAge(testDate);

            //Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
