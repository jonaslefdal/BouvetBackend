using Xunit;
using System;
using System.Collections.Generic;

namespace BouvetBackend.Tests.Controllers
{
    public class TransportEconomicsTests
    {
        public enum Methode
        {
            Car,
            Bus,
            Cycling,
            Walking
        }

        private double GetMaxRewardableDistance(Methode mode)
        {
            return mode switch
            {
                Methode.Car     => 11.11,
                Methode.Bus     => 36.00,
                Methode.Cycling => 17.36,
                Methode.Walking => 17.36,
                _               => 30.0
            };
        }

        private double CalculateCo2(double distanceKm, Methode mode)
        {
            var emissionFactors = new Dictionary<Methode, double>()
            {
                { Methode.Car, 0.2 },
                { Methode.Bus, 0.1 },
                { Methode.Cycling, 0.0 },
                { Methode.Walking, 0.0 }
            };

            double defaultEmission = 0.2;
            double selectedEmission = emissionFactors.GetValueOrDefault(mode, defaultEmission);
            double cappedDistance = Math.Min(distanceKm, GetMaxRewardableDistance(mode));
            double savedCo2 = (defaultEmission - selectedEmission) * cappedDistance;
            return Math.Max(savedCo2, 0);
        }

        private double CalculateMoneySaved(double distanceKm, Methode mode)
        {
            var costPerKm = new Dictionary<Methode, double>()
            {
                { Methode.Car, 2.50 },  // Samkjørt bil
                { Methode.Bus, 1.19 },
                { Methode.Cycling, 0.0 },
                { Methode.Walking, 0.0 }
            };

            double defaultCost = 5.00;
            double selectedCost = costPerKm.GetValueOrDefault(mode, defaultCost);
            double cappedDistance = Math.Min(distanceKm, GetMaxRewardableDistance(mode));
            double savedMoney = (defaultCost - selectedCost) * cappedDistance;
            return Math.Max(savedMoney, 0);
        }

        // Test wether km and caluclation match for co2

        [Theory]
        [InlineData(10.0, Methode.Bus, 1.0)]     // (0.2 - 0.1) * 10
        [InlineData(10.0, Methode.Cycling, 2.0)] // (0.2 - 0.0) * 10
        [InlineData(50.0, Methode.Bus, 3.6)]     // capped at 36 km
        [InlineData(10.0, Methode.Car, 0.0)]     // ingen besparelse
        [InlineData(0.0, Methode.Walking, 0.0)]
        public void CalculateCo2_ReturnsExpected(double km, Methode mode, double expectedKg)
        {
            var result = CalculateCo2(km, mode);
            Assert.Equal(expectedKg, Math.Round(result, 2));
        }

        // Test wether km and caluclation match for money

        [Theory]
        [InlineData(10.0, Methode.Bus, 38.1)]        // (5.00 - 1.19) * 10
        [InlineData(10.0, Methode.Cycling, 50.0)]    // (5.00 - 0.00) * 10
        [InlineData(50.0, Methode.Cycling, 86.8)]    // capped: 17.36 * 5.00
        [InlineData(10.0, Methode.Car, 25.0)]        // (5.00 + 2.5) * 10 
        [InlineData(0.0, Methode.Walking, 0.0)]
        public void CalculateMoneySaved_ReturnsExpected(double km, Methode mode, double expectedKr)
        {
            var result = CalculateMoneySaved(km, mode);
            Assert.Equal(expectedKr, Math.Round(result, 1));
        }
    }
}
