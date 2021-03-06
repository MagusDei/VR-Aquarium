//using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace UnityTests
{
    public class MovesenseInterfaceTests
    {

        [Test]
        public void CanLoadCSV()
        {
            var movesenseInterface = new MovesenseInterface();
            Assert.That(movesenseInterface.LoadCSV("AccelerationData.csv"), Is.EqualTo(true));
        }

        [Test]
        public void NextValueGivesCorrectFirstResult()
        {
            var movesenseInterface = new MovesenseInterface();
            movesenseInterface.LoadCSV("AccelerationData.csv");
            Assert.That(movesenseInterface.NextValue(), Is.EquivalentTo(new float[] { 999026f, 7.987527f, 2.148831f, 5.470188f}));
        }

        [Test]
        public void SubsequentNextValuesAreNotTheSame()
        {
            var movesenseInterface = new MovesenseInterface();
            movesenseInterface.LoadCSV("AccelerationData.csv");
            for (int i = 0; i < 10; i++)
            {
                Assert.That(movesenseInterface.NextValue(), Is.Not.EqualTo(movesenseInterface.NextValue()));
            }
        }

        [Test]
        public void NextValueReturnsNullIfNotCSVLoaded()
        {
            var movesenseInterface = new MovesenseInterface();
            Assert.That(movesenseInterface.NextValue(), Is.Null);
        }

        [Test]
        public void DifferentCSVsGiveDifferentResults()
        {
            var interface1 = new MovesenseInterface();
            var interface2 = new MovesenseInterface();
            interface1.LoadCSV("AccelerationData.csv");
            interface2.LoadCSV("TemperatureData.csv");
            Assert.That(interface2.NextValue(), Is.Not.EquivalentTo(interface1.NextValue()));
        }

        [Test]
        public void WrongFilenamesRaiseException()
        {
            var interface1 = new MovesenseInterface();
            Assert.That(() => interface1.LoadCSV("nosuch.csv"), Throws.ArgumentException);
        }

        [Test]
        public void NextValueLoopsThroughTheWholeFile()
        {
            var interface1 = new MovesenseInterface();
            interface1.LoadCSV("AccelerationData4Lines.csv");
            for (int i = 0; i < 10; i++)
            {
                var val = interface1.NextValue();
                Assert.NotNull(val);
            }

        }
    }
}