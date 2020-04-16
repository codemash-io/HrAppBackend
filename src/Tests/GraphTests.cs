using HrApp;
using Microsoft.Graph;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraphUser = Microsoft.Graph.User;

namespace Tests
{
    public class GraphTests
    {
        GraphRepository graphRepo;
        GraphUserRepository graphUserRepo;
        GraphEventsRepository graphEventRepo;
        GraphContactRepository graphContactRepo;

        RoomBookerService roomService;

        [SetUp]
        public void Setup()
        {
            graphRepo = new GraphRepository();
            graphEventRepo = new GraphEventsRepository();
            graphUserRepo = new GraphUserRepository();
            graphContactRepo = new GraphContactRepository();
            roomService = new RoomBookerService()
            {
                GraphEventRepository = graphEventRepo,
                GraphRepository = graphRepo,
                GraphUserRepository = graphUserRepo
            };
        }

        [Test]
        public async Task GetAccessToken()
        {
            var token = await graphRepo.GetAccessToken();
            Assert.IsNotNull(token);
        }

        [Test]
        public async Task GetCalenadrEventsByDate()
        {
            var meetingRoom = MeetingRooms.Hamburg.ToString();

            var from = new DateTime(2019, 09, 04, 07, 0, 0, DateTimeKind.Utc);
            var to = new DateTime(2019, 09, 04, 07, 15, 0, DateTimeKind.Utc);

            var result = await graphEventRepo.GetCalendarEventsByDate(meetingRoom, from, to);
            Assert.IsNotEmpty(result);
        }


        [Test]
        public async Task TestBookRoom()
        {
            var emp1 = "123";//"de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var emp2 = "123";// "789b4799-49fb-4d17-8413-a1a9e13d33a5";

            var employees = new List<string> { emp1, emp2 };

            var booking = new Booking(
                MeetingRooms.Hamburg.ToString(),
                emp1,
                new DateTime(2020, 01, 01, 21, 00, 00),
                new DateTime(2020, 01, 01, 21, 15, 00),
                employees,
                "Test_insert"
            );

           var isBookedSuccessfully = await roomService.BookRoom(booking);

            Assert.IsTrue(isBookedSuccessfully);
        }

        [Test]
        public async Task DeleteEventById()
        {
            string eventId = "AAMkADUwZGM3ZDBhLWEyOGQtNGI3My04NTE4LTYyZjg4Yzg3NDc5MwBGAAAAAADRRo8_4__FSbFENgFO65X1BwCVbXfSRkx5TowOq2ZibiTBAAAAAAENAACVbXfSRkx5TowOq2ZibiTBAAD0p8aVAAA=";
            string roomName = MeetingRooms.Hamburg.ToString();

            var isDeleted = await graphEventRepo.DeleteEventById(eventId, roomName);

            Assert.IsTrue(isDeleted);
        }
        [Test]
        public async Task EditEvent()
        {
            string eventId = "AAMkADUwZGM3ZDBhLWEyOGQtNGI3My04NTE4LTYyZjg4Yzg3NDc5MwBGAAAAAADRRo8_4__FSbFENgFO65X1BwCVbXfSRkx5TowOq2ZibiTBAAAAAAENAACVbXfSRkx5TowOq2ZibiTBAAD0p8aaAAA=";

            var emp1 = "123";//"de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var emp2 = "123";// "789b4799-49fb-4d17-8413-a1a9e13d33a5";

            var employees = new List<string> { emp1, emp2 };

            var booking = new Booking(
                MeetingRooms.Hamburg.ToString(),
                emp1,
                new DateTime(2020, 01, 01, 09, 30, 00),
                new DateTime(2020, 01, 01, 10, 00, 00),
                employees,
                "Test_insert"
            );

            await roomService.EditBooking(eventId, booking);
        }

        [Test]
        public async Task GetGraphUserById()
        {
            string userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";

            var user = await graphUserRepo.GetGraphUserById(userId);
            Assert.IsNotNull(user);
        }
        [Test]
        public async Task GetAllGraphUsers()
        {
            var users = await graphUserRepo.GetAllGraphUsers();
            Assert.IsNotEmpty(users);
        }

        [Test]
        public async Task CreateGraphUser()
        {
            string name = "Mantas3";
            string email = "mantasdaunoravicius@presentconnection.eu";
            string password = "Password123";
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                    throw new BusinessException("Your email is invalid");
            }
            catch
            {
                throw new BusinessException("Your email is invalid");
            }
            if (password.Length < 8)
                throw new BusinessException("Password must contain at least 8 characters");
            if (!password.Any(char.IsUpper))
                throw new BusinessException("Password must contain at least one upper char");
            if(!password.Any(char.IsDigit))
                throw new BusinessException("Password must contain at least one digit");

            var user = new GraphUser
            {
                AccountEnabled = true,
                DisplayName = name,
                UserPrincipalName = email,
                MailNickname = email.Split('@')[0],
                PasswordProfile = new PasswordProfile
                {
                    ForceChangePasswordNextSignIn = true,
                    Password = password
                }
            };

            var createdUser = await graphUserRepo.CreateGraphUser(user);

            Assert.IsNotNull(createdUser);
        }

        [Test]
        public async Task UpdateGraphUserAvatar()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";

            // Load file meta data with FileInfo
            string fileInfo = @"C:\Users\Mantas\Desktop\123\360x360.png";
            byte[] data = System.IO.File.ReadAllBytes(fileInfo);

            var createdUser = await graphUserRepo.EditGraphUserAvatar(userId, data);

            Assert.IsNotNull(createdUser);
        }
        [Test]
        public async Task GetUserPhoto()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";

            var size = "360x360";
            var bytes = await graphUserRepo.GetUserProfilePhoto(userId, size);
            Assert.IsNotEmpty(bytes);
        }

        [Test]
        public async Task EditGraphUser()
        {
            string name = "Mantas", surname = "Daunoravicius", 
                displayName = "Mantas Daunoravicius";
            string userId = "be8c2cf4-a4a4-49e5-b097-f6add3a935fa";

            var userDetails = new GraphUser
            {
                GivenName = name,
                Surname = surname,
                DisplayName = displayName
            };

            var users = await graphUserRepo.EditGraphUser(userId, userDetails);
            Assert.IsTrue(users);
        }

        [Test]
        public async Task GetUserReminderView()
        {
            var userId = "d6b46bc2-1eac-4b2f-9037-780c4464449e";
            //+2h because when event is created it saves time in UTC
            var from = new DateTime(2020, 01, 01, 12, 45, 0);
            var to = new DateTime(2020, 01, 01, 14, 30, 0);

            var reminders = await graphUserRepo.GetUserReminderView(userId, from, to);
            Assert.IsNotEmpty(reminders);
        }

        [Test]
        public async Task GetAllUserContacts()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            string select = null, expand = null;

            var contacts = await graphContactRepo.GetAllUserContacts(userId, expand, select);
            Assert.IsNotEmpty(contacts);
        }
        [Test]
        public async Task GetUserContactById()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var contactId = "AAMkADE2M2NhNTBhLTdlM2YtNDY1Mi1iZDIzLTU0MTU4ODY0ZjZjZQBGAAAAAACY-buA42x5RqvUdVGejluQBwCfssnsHwkhRrDM-OroFcp6AAAAAAEOAACfssnsHwkhRrDM-OroFcp6AAB-vVVxAAA=";
            string select = null, expand = null;

            var contact = await graphContactRepo.GetUserContactById(userId, contactId, expand, select);
            Assert.IsNotNull(contact);
        }

        [Test]
        public async Task CreateUserContact()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            string givenName = "mantas",
                surname = "daunoravicius",
                email = "mantasd@presentconnection.eu";

            givenName = givenName.First().ToString().ToUpper() + givenName.Substring(1);
            surname = surname.First().ToString().ToUpper() + surname.Substring(1);

            var contact = new Contact
            {
                GivenName = givenName,
                Surname = surname,
                EmailAddresses = new List<EmailAddress>
                {
                    new EmailAddress
                    {
                        Address = email,
                        Name = givenName + " " + surname + " (" + email + ")"
                    }
                },
                Initials = givenName.ToUpper()[0] + "."+surname.ToUpper()[0]
            };

            var createdContact = await graphContactRepo.CreateUserContact(userId, contact);
            Assert.IsNotNull(createdContact);
        }

        [Test]
        public async Task UpdateUserContact()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var contactId = "AAMkADE2M2NhNTBhLTdlM2YtNDY1Mi1iZDIzLTU0MTU4ODY0ZjZjZQBGAAAAAACY-buA42x5RqvUdVGejluQBwCfssnsHwkhRrDM-OroFcp6AAAAAAEOAACfssnsHwkhRrDM-OroFcp6AAB-vVVuAAA=";
            string displayName = "mantas d", surname = "d", name = "mantas",
                homePhones= "asdasd", mobilePhone = "asda" ,bussphones = "asd";
            string street = "some street", city = "kaunas", postcode = "123";
            var contact = new Contact
            {
                DisplayName = displayName,
                GivenName = name,
                Surname = surname,
                HomeAddress = new PhysicalAddress 
                {
                    Street = street,
                    City = city,
                    PostalCode = postcode
                },
                BusinessPhones = new List<string> { bussphones },
                HomePhones = new List<string> { homePhones },
                MobilePhone = mobilePhone

            };

            var createdContact = await graphContactRepo.UpdateUserContact(
                userId, contactId, contact);
            Assert.IsNotNull(createdContact);
        }

        [Test]
        public async Task DeleteUserContact()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var contactId = "AAMkADE2M2NhNTBhLTdlM2YtNDY1Mi1iZDIzLTU0MTU4ODY0ZjZjZQBGAAAAAACY-buA42x5RqvUdVGejluQBwCfssnsHwkhRrDM-OroFcp6AAAAAAEOAACfssnsHwkhRrDM-OroFcp6AAB-vVVvAAA=";

            var isDeletedSuccessfully = await graphContactRepo.DeleteUserContact(
                userId, contactId);

            Assert.IsTrue(isDeletedSuccessfully);
        }
    }
}