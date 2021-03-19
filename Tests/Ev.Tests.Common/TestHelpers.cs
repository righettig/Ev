using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ev.Tests.Common
{
    public static class TestHelpers
    {
        public static void ShouldThrowArgumentNullException(Action action)
        {
            Assert.ThrowsException<ArgumentNullException>(action);
        }

        public static void ShouldThrowArgumentException(Action action)
        {
            Assert.ThrowsException<ArgumentException>(action);
        }
    }
}