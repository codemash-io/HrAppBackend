using HrApp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    public class AggregateTests
    {

        IAggregateRepository aggregateRepo;
        [SetUp]
        public void Setup()
        {
            aggregateRepo = new AggregateRepository();
        }

        [Test]
        public async Task Process_Noob_Form_test()
        {
            await aggregateRepo.LunchMenuReport("5e6a1b980187c000015b0767");
        }



    }
}