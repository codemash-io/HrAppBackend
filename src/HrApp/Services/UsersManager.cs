using System;
using System.Collections.Generic;
using System.Linq;
using CodeMash.Membership.Services;
using CodeMash.Repository;
using HrApp.Entities;
using Isidos.CodeMash.ServiceContracts;

namespace HrApp.Services
{
    public class UsersManager
    {
        public List<string> GetRolesByOrderType(string orderTypeString)
        {
            var orderRulesRepo = new CodeMashRepository<WishlistDecisionRule>(Settings.Client);

            var types = orderRulesRepo.Find();

            return types.Items.First(x => x.Type == orderTypeString).Roles;
        }

        public List<User> GetUsersByRoles(List<string> roles)
        {
            var usersDb = new CodeMashMembershipService(Settings.Client);
            var allUsers = usersDb.GetUsersList(new GetUsersRequest()).Result;

            var outputList = (from role in roles
                    from user in allUsers
                    where user.Roles.Any(x => x.Name == role)
                    select user)
                .ToList();

            return outputList;

            #region Function

            //this is how the function looked like before resharper decided
            //to convert my nested foreach'es into a single line of LINQ

            //var outputUsers = new List<User>();
            //var usersDb = new CodeMashMembershipService(Settings.Client);
            //var allUsers = usersDb.GetUsersList(new GetUsersRequest()).Result;

            //foreach (var role in roles)
            //{
            //    foreach (var user in allUsers)
            //    {
            //        if (user.Roles.Any(x => x.Name == role))
            //        {
            //            outputUsers.Add(user);
            //        }
            //    }
            //}

            //return outputUsers;

            #endregion
        }

        public void WhenUserAgrees(string wishlistId, string userThatAgreedId)
        {
            var wishlistRepo = new CodeMashRepository<WishlistEntity>(Settings.Client);
            var wishlist = wishlistRepo.FindOneById(wishlistId);

            wishlist.ApprovedBy.Add(userThatAgreedId);

            if (wishlist.ApprovedBy.Count == wishlist.ShouldBeApprovedBy.Count)
            {
                wishlist.Status = "Approved";
            }

            wishlistRepo.ReplaceOne(x => x.Id == wishlistId, wishlist);
        }

        public void WhenSomeoneDeclinesOrder(string wishlistRecordId)
        {
            var wishlistRepo = new CodeMashRepository<WishlistEntity>(Settings.Client);

            var wishlistEntity = wishlistRepo.FindOneById(wishlistRecordId);
            wishlistEntity.Status = "Declined";

            wishlistRepo.ReplaceOne(x => x.Id == wishlistRecordId, wishlistEntity);
        }
    }
}