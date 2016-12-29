using api.core.Provider;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.tests
{
    [TestFixture]
    public class Integration
    {
        [Test]
        public void DoFullIntegration()
        {
            var publishManager = new PublishManager();
        }
    }
}
