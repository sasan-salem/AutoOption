using AutoOption.Database.Interface;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoOption.Tests
{
    public class HandleOptionTests
    {

        [Test]
        public void GetOption_WhenCalled_ReturnFilledOption()
        {
            var DBQueriesMock = new Mock<IDBQueries>();
            DBQueriesMock.Setup(d => d.GetAll<OptionEntity>()).Returns(OptionEntities);

            var handleOption = new HandleOption(typeof(Option), null, DBQueriesMock.Object);

            var result = handleOption.GetOption<Option>();

            Assert.That(result.First, Is.EqualTo(1));
            Assert.That(result.Second, Is.EqualTo("s"));
            Assert.That(result.Third, Is.EqualTo(true));
        }

        [Test]
        public void Register_TableExist_DoEverythinsFine()
        {

        }

        // Private Function //

        private List<OptionEntity> OptionEntities =>
            new List<OptionEntity>()
            {
                new OptionEntity(){Key ="First", Display="d1",Value="1" },
                new OptionEntity(){Key ="Second", Display="d2",Value="s" },
                new OptionEntity(){Key ="Third", Display="d2",Value="true" }
            };

        class Option
        {
            public int First { get; set; }
            public string Second { get; set; }
            public bool Third { get; set; }
        }
    }
}
