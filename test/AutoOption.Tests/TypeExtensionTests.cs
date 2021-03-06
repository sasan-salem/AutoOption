﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoOption.Tests
{
    [TestFixture]
    class TypeExtensionTests
    {
        private readonly Type type = typeof(Option);

        [Test]
        public void ExtractKeysFromType_WhenCalled_DoYourWorkWell()
        {
            var result = type.ExtractKeysFromType();

            Assert.That(result, Is.EqualTo(new List<OptionEntity>()
            {
                new OptionEntity(){ Key = "First", Display = "First Display", Type = "Int32" },
                new OptionEntity(){ Key = "Second", Display = "Second Display", Type = "String" },
                new OptionEntity(){ Key = "Third", Display = "Third", Type = "Boolean" }
            }));
        }

        [Test]
        [TestCase(0, "First Display")]
        [TestCase(2, "Third")]
        public void GetDisplay_WhenCalled_DoYourWorkWell(int prop, string expectedResult)
        {
            var result = type.GetProperties()[prop].GetDisplay();
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(0, "1", 1)]
        [TestCase(1, "s", "s")]
        [TestCase(2, "true", true)]
        public void Convert_WhenCalled_DoYourWorkWell(int number, string value, object expectedResult)
        {
            var result = type.GetProperties()[number].Convert(value);
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        class Option
        {
            [Display(Name = "First Display")]
            public int First { get; set; }

            [Display(Name = "Second Display")]
            public string Second { get; set; }

            public bool Third { get; set; }
        }
    }
}
