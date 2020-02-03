using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeMash.Repository;
using HrApp;
using HrApp.Entities;
using HrApp.Services;
using NUnit.Framework;

namespace LunchOrderServerTesting
{
    [TestFixture]
    class UsersManagerTests
    {
        [Test]
        [TestCase("ceo", "accountant", ExpectedResult = 1)]
        [TestCase("ceo", "administrator", ExpectedResult = 4)]
        [TestCase("CEO", "administrator", ExpectedResult = 3)]
        public int GetUsersByRoles(params string[] roles)
        {
            var userManager = new UsersManager();

            var users = userManager.GetUsersByRoles(roles.ToList());

            return users.Count;
        }

        [Test]
        public void WhenOrderIsDeclined()
        {
            var userManager = new UsersManager();
            var wishlistRepo = new CodeMashRepository<WishlistEntity>(Settings.Client);
            var declinedWishlistCount = wishlistRepo.Find().Items.Count(x => x.Status == "Declined");

            userManager.WhenSomeoneDeclinesOrder("5e3046c1bebcee00012f237b");

            var declinedWishlistCountAfterDeclinedOrder =
                wishlistRepo.Find().Items.Count(x => x.Status == "Declined");

            Assert.AreEqual(declinedWishlistCount + 1, declinedWishlistCountAfterDeclinedOrder);
        }

        [Test]
        [TestCase("5e2aa8f52fe37a00015a2b07", ExpectedResult = "ceo,accountant")]
        [TestCase("5e2aa8d12fe37a00015a2b06", ExpectedResult = "office-administrator,accountant")]
        [TestCase("5e2aa90a2fe37a00015a2b09", ExpectedResult = "accountant,office-administrator")]
        [TestCase("5e2aa8fc2fe37a00015a2b08", ExpectedResult = "cto")]
        public string GetRolesByOrderTypeTests(string orderType)
        {
            var usersManager = new UsersManager();

            var roles = usersManager.GetRolesByOrderType(orderType);

            var outputString = string.Join(",", roles);

            return outputString;
        }

        //use this test on a single wishlist, when there are 2 people left to approve
        [Test]
        [TestCase("5e2ab97da279910001ea231b", "25a5e422-6e07-42d4-9df4-18dae9a88426", false)]
        [TestCase("5e2ab97da279910001ea231b", "4db186cd-91b9-4538-97b5-40b0f7a139ab", true)]
        public void WhenAllAgreeOnOrder(string wishlistId, string userThatApproved, bool shouldOrderBeApproved)
        {
            var userManager = new UsersManager();
            var wishlistRepo = new CodeMashRepository<WishlistEntity>(Settings.Client);
            var approvedByCount = wishlistRepo.FindOneById(wishlistId).ApprovedBy.Count;

            userManager.WhenUserAgrees(wishlistId, userThatApproved);

            var newWishlist = wishlistRepo.FindOneById(wishlistId);

            Assert.AreEqual(approvedByCount + 1, newWishlist.ApprovedBy.Count);
            Assert.AreEqual(newWishlist.Status == "Approved", shouldOrderBeApproved);
        }
    }
}