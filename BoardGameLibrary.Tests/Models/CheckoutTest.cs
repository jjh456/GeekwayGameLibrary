using BoardGameLibrary.Data.Models;
using NUnit.Framework;
using System;

namespace BoardGameLibrary.Tests.Models
{
    [TestFixture]
    public class CheckoutTest
    {
        [Test]
        public void UnresolvedCheckoutLengthIsNowMinusTimeOut()
        {
            var timeOut = new DateTime(2016, 4, 22);
            var model = new Checkout
            {
                TimeOut = timeOut
            };

            var expectedLength = DateTime.Now - timeOut;

            Assert.AreEqual(expectedLength, model.Length);
        }

        [Test]
        public void ResolvedCheckoutLengthIsTimeInMinusTimeOut()
        {
            var timeOut = new DateTime(2016, 4, 22);
            var timeIn = new DateTime(2016, 4, 24);
            var model = new Checkout
            {
                TimeOut = timeOut,
                TimeIn = timeIn
            };

            var expectedLength = timeIn - timeOut;

            Assert.AreEqual(expectedLength, model.Length);
        }
    }
}
