using Xunit;
using System;

namespace BouvetBackend.Tests.Controllers
{
    public class TransportCalculationTests
    {
        public enum Methode
        {
            Car,
            Bus,
            Cycling,
            Walking
        }

        private int CalculatePoints(double distanceKm, Methode mode)
        {
            const double singleOccupantCO2 = 120.0;
            const double carpoolCO2 = 60.0;
            const double busCO2 = 70.0;
            const double cycleCO2 = 0.0;
            const double walkCO2 = 0.0;

            double modeCO2 = mode switch
            {
                Methode.Car => carpoolCO2,
                Methode.Bus => busCO2,
                Methode.Cycling => cycleCO2,
                Methode.Walking => walkCO2,
                _ => singleOccupantCO2
            };

            double exponent = 0.5;
            double adjustedDistance = Math.Pow(distanceKm, exponent); // rot(d)
            double baselineCO2 = singleOccupantCO2 * adjustedDistance;
            double actualCO2 = modeCO2 * adjustedDistance;
            double co2Saved = Math.Max(0, baselineCO2 - actualCO2);

            double points = (co2Saved / 50.0) * 50; // samme som co2Saved
            double final = points;

            double maxCap = mode switch
            {
                Methode.Bus => 300,
                Methode.Cycling => 500,
                Methode.Walking => 500,
                Methode.Car => 200,
                _ => 200
            };

            return (int)Math.Round(Math.Min(final, maxCap));
        }


        // test pre calucalted expected points for a method with some km
        [Theory]
        [InlineData(3.0, Methode.Car, 104)]
        [InlineData(20.0, Methode.Bus, 224)]
        [InlineData(4.0, Methode.Cycling, 240)]
        [InlineData(1.0, Methode.Walking, 120)]
        [InlineData(15.0, Methode.Car, 200)] // capped
        [InlineData(30.0, Methode.Cycling, 500)] // capped
        [InlineData(0.0, Methode.Walking, 0)]
        public void CalculatePoints_WorksCorrectly(double km, Methode mode, int expectedPoints)
        {
            var result = CalculatePoints(km, mode);
            Assert.Equal(expectedPoints, result);
        }

        // test wether points go up in value with distance
        [Fact]
        public void CalculatePoints_IncreasesWithDistance()
        {
            var low = CalculatePoints(1.0, Methode.Bus);
            var high = CalculatePoints(10.0, Methode.Bus);
            Assert.True(high > low);
        }

        // test if we can get 0 when we should
        [Fact]
        public void CalculatePoints_IsZeroWhenDistanceIsZero()
        {
            var result = CalculatePoints(0, Methode.Car);
            Assert.Equal(0, result);
        }

        // test to make sure points dont go over max cap
        [Fact]
        public void CalculatePoints_DoesNotExceedMaxCap()
        {
            var cyclingPoints = CalculatePoints(1000, Methode.Cycling);
            Assert.Equal(500, cyclingPoints); // cap

            var carPoints = CalculatePoints(1000, Methode.Car);
            Assert.Equal(200, carPoints); // cap
        }
    }
}
