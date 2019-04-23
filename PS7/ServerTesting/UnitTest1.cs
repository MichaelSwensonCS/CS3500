using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceWars;

namespace ServerTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void oneStarOnCreation()
        {
            Server server = new Server();
            World w = server.World;
            Assert.AreEqual(1, w.Stars.Count);
        }

    }
}
