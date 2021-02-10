using AutoOption.Database.Interface;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoOption.Tests
{
    [TestFixture]
    public class HandleOptionTests
    {
        private Mock<IDBQueries> dBQueriesMock;

        [SetUp]
        public void SetUp()
        {
            dBQueriesMock = new Mock<IDBQueries>();
        }

        [Test]
        public void GetOption_WhenCalled_ReturnFilledOption()
        {
            
            dBQueriesMock.Setup(d => d.GetAll<OptionEntity>()).Returns(OptionEntities);

            var handleOption = new HandleOption(typeof(Option), null, dBQueriesMock.Object);

            var result = handleOption.GetOption<Option>();

            Assert.That(result.First, Is.EqualTo(1));
            Assert.That(result.Second, Is.EqualTo("s"));
            Assert.That(result.Third, Is.EqualTo(true));
        }



        [Test]
        [TestCaseSource("_withNewProperties")]
        public void Register_TableExistAndOptionHasNewProperties_AddNewPeroperties(
            List<string> InTableKeys,
            List<OptionEntity> GoesToInsert)
        {
            dBQueriesMock.Setup(d => d.IsTableExist()).Returns(true);
            dBQueriesMock.Setup(d => d.GetOneColumn("[Key]")).Returns(InTableKeys);

            var handleOption = new HandleOption(typeof(Option), null, dBQueriesMock.Object);
            handleOption.Register();

            dBQueriesMock.Verify(d =>
                d.GroupDeleteByKeys(It.IsAny<List<string>>(), "[Key]"), Times.Never);

            dBQueriesMock.Verify(d => d.GroupAdd(GoesToInsert));
        }

        [Test]
        [TestCaseSource("_withFewerProperties")]
        public void Register_TableExistAndOptionHasFewerProperties_DeleteFewerPeroperties(
            List<string> InTableKeys,
            List<string> GoesToDelete)
        {

            dBQueriesMock.Setup(d => d.IsTableExist()).Returns(true);
            dBQueriesMock.Setup(d => d.GetOneColumn("[Key]")).Returns(InTableKeys);

            var handleOption = new HandleOption(typeof(Option), null, dBQueriesMock.Object);
            handleOption.Register();

            dBQueriesMock.Verify(d => d.GroupDeleteByKeys(GoesToDelete, "[Key]"));

            dBQueriesMock.Verify(d => d.GroupAdd(It.IsAny<List<OptionEntity>>()), Times.Never);
        }

        [Test]
        [TestCaseSource("_withNewAndFewerProperties")]
        public void Register_TableExistAndOptionHasNewAndFewerProperties_AddNewAndDeleteFewerPeroperties(
            List<string> InTableKeys,
            List<string> GoesToDelete,
            List<OptionEntity> GoesToInsert)
        {

            dBQueriesMock.Setup(d => d.IsTableExist()).Returns(true);
            dBQueriesMock.Setup(d => d.GetOneColumn("[Key]")).Returns(InTableKeys);

            var handleOption = new HandleOption(typeof(Option), null, dBQueriesMock.Object);
            handleOption.Register();

            dBQueriesMock.Verify(d => d.GroupDeleteByKeys(GoesToDelete, "[Key]"));

            dBQueriesMock.Verify(d => d.GroupAdd(GoesToInsert));
        }

        // Private //

        public static readonly object[] _withNewProperties =
        {
            new object[] {
                new List<string>() { "Third", "First" },
                new List<OptionEntity>()
                {
                    new OptionEntity(){ Key = "Second", Display = "Second Display", Type = "String" }
                }
            },
            new object[] {
                new List<string>() { "Third" },
                new List<OptionEntity>()
                {
                    new OptionEntity(){ Key = "First", Display = "First Display", Type = "Int32" },
                    new OptionEntity(){ Key = "Second", Display = "Second Display", Type = "String" }
                }
            }
        };

        public static readonly object[] _withFewerProperties =
        {
            new object[] {
                new List<string>() { "First", "Fourth", "Third", "Second" },
                new List<string>() { "Fourth" }
            }
        };

        public static readonly object[] _withNewAndFewerProperties =
        {
            new object[] {
                new List<string>() { "First", "Fourth", "Second" },
                new List<string>() { "Fourth" },
                new List<OptionEntity>()
                {
                    new OptionEntity(){ Key = "Third", Display = "Third Display", Type = "Boolean" }
                }
            }
        };

        private readonly List<OptionEntity> OptionEntities =
            new List<OptionEntity>()
            {
                new OptionEntity() { Key = "First", Display = "First Display" , Value= "1", Type = "Int32" },
                new OptionEntity() { Key = "Second", Display = "Second Display", Value="s", Type = "String" },
                new OptionEntity() { Key = "Third", Display = "Third Display", Value="true", Type = "Boolean" }
            };

        class Option
        {
            [Display(Name = "First Display")]
            public int First { get; set; }

            [Display(Name = "Second Display")]
            public string Second { get; set; }

            [Display(Name = "Third Display")]
            public bool Third { get; set; }
        }
    }
}
