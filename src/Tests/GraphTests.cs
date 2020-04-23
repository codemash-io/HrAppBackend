using HrApp;
using Microsoft.Graph;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        IGraphFileRepository graphFileRepo;

        RoomBookerService roomService;

        [SetUp]
        public void Setup()
        {
            graphRepo = new GraphRepository();
            graphEventRepo = new GraphEventsRepository();
            graphUserRepo = new GraphUserRepository();
            graphContactRepo = new GraphContactRepository();
            graphFileRepo = new GraphFileRepository();
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
            string userId = "6d7a8195-b675-44d4-95f5-e214151d414c";

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

        [Test]
        public async Task GetAllDrives()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var type = GraphResourceTypes.users.ToString();

            var drives = await graphFileRepo.ListAllDrives(type, userId);
            Assert.IsNotEmpty(drives);
        }//IGraphFileRepository graphFileRepo = new GraphFileRepository();
        [Test]
        public async Task GetDrive()
        {
            var type = GraphResourceTypes.users.ToString();
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";

            var drive = await graphFileRepo.GetDrive(type, userId);
            Assert.IsNotNull(drive);
        }
        [Test]
        public async Task GetSpecialFolder()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var type = GraphResourceTypes.users.ToString();

            var driveItem = await graphFileRepo.GetSpecialFolder(type, "documents", userId);
            Assert.IsNotNull(driveItem);
        }
        [Test]
        public async Task SharedWithMe()
        {
            var type = GraphResourceTypes.users.ToString();
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";

            var sharedItems = await graphFileRepo.SharedWithMe(type, userId);
            Assert.IsNotEmpty(sharedItems);
        }
        [Test]
        public async Task ListChildren()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var itemId = "01XDBAENA3RBWGJKHGVZGJDRN6VPYK64HP";
            var path = "New Folder 1";
            var type = GraphResourceTypes.users.ToString();
            //root
            var children = await graphFileRepo.ListChildren(type, userId);
            Assert.IsNotEmpty(children);
            //with itemId
            var children2 = await graphFileRepo.ListChildren(type, userId, itemId);
            Assert.IsNotEmpty(children2);
            //with path
            var children3 = await graphFileRepo.ListChildren(type, userId, null, path);
            Assert.IsNotEmpty(children3);
        }

        [Test]
        public async Task GetItem()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var itemId = "01XDBAENA3RBWGJKHGVZGJDRN6VPYK64HP";
            var path = "New Folder 1";
            var type = GraphResourceTypes.users.ToString();

            //root - default
            var items = await graphFileRepo.GetItem(type, userId);
            Assert.IsNotNull(items);
            // with itemId
            var items2 = await graphFileRepo.GetItem(type, userId,itemId);
            Assert.IsNotNull(items2);
            // with path
            var items3 = await graphFileRepo.GetItem(type, userId, null, path);
            Assert.IsNotNull(items3);

        }

        [Test]
        public async Task GetThumbnails()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var itemId = "01XDBAENBJGMICFGOPYNF2P7NRFM3WQF4T";
            var type = GraphResourceTypes.users.ToString();

            //root - default
            var thumbs = await graphFileRepo.GetThumbnails(type, itemId, userId);
            Assert.IsNotEmpty(thumbs);
        }

        [Test]
        public async Task GetSingleThumbnail()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var itemId = "01XDBAENBJGMICFGOPYNF2P7NRFM3WQF4T";
            var type = GraphResourceTypes.users.ToString();
            var thumbId = "0";

            //root - default
            var thumb = await graphFileRepo.GetSingleThumbnail(type, itemId, thumbId, userId);
            Assert.IsNotNull(thumb);
        }
        [Test]
        public async Task GetSingleThumbnailContent()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var itemId = "01XDBAENBJGMICFGOPYNF2P7NRFM3WQF4T";
            var type = GraphResourceTypes.users.ToString();
            var thumbId = "0";
            var size = "medium";

            //root - default
            var thumb = await graphFileRepo.GetSingleThumbnailContent(type, itemId, thumbId, size, userId);
            Assert.IsNotEmpty(thumb);
        }

        [Test]
        public async Task CreateFolder()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var itemId = "01XDBAENA3RBWGJKHGVZGJDRN6VPYK64HP";
            var type = GraphResourceTypes.users.ToString();
            var item = new DriveItem { Name = "naujas.jpg", File = new Microsoft.Graph.File { } };

            //root - default
            var newFile = await graphFileRepo.CreateFolder(type, itemId, item, userId);
            Assert.IsNotNull(newFile);
        }
        [Test]
        public async Task UpdateItem()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var itemId = "01XDBAENDGIVNLRDI3FJAJWSGIX6W2L6O7";
            var type = GraphResourceTypes.users.ToString();
            var update = new DriveItem { Name = "myImg3.png" };

            //root - default
            var newFile = await graphFileRepo.UpdateItem(type, itemId, update, userId);
            Assert.IsNotNull(newFile);
        }
        [Test]
        public async Task DeleteItem()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var itemId = "01XDBAENDGIVNLRDI3FJAJWSGIX6W2L6O7";
            var type = GraphResourceTypes.users.ToString();

            //root - default
            var newFile = await graphFileRepo.DeleteItem(type, itemId, userId);
            Assert.IsTrue(newFile);
        }
        [Test]
        public async Task MoveItem()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var itemId = "01XDBAENDKLZBJRW6S4BGISBC5LSKATZI6";
            var moveTo = "01XDBAENAGEI7YF5TK2BCIVFPHN2LPGUJ4";
            var type = GraphResourceTypes.users.ToString();

            //root - default
            var movedItem = await graphFileRepo.MoveItem(type, itemId, moveTo, userId);
            Assert.IsNotNull(movedItem);
        }
        [Test]
        public async Task CopyItem()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var driveId = "b!ReYerZXwIUaQE56F9g7elp_trKttyjpDrqdNlLfdo1_XiI-cLy0nSp4JI6beeTCj";
            var itemId = "01XDBAENDKLZBJRW6S4BGISBC5LSKATZI6";
            var moveTo = "01XDBAENAGEI7YF5TK2BCIVFPHN2LPGUJ4";
            var type = GraphResourceTypes.users.ToString();

            var body = new DriveItem
            {
                ParentReference = new ItemReference
                {
                    DriveId = driveId,
                    Id = moveTo
                }
            };

            var movedItem = await graphFileRepo.CopyItem(type, itemId, body, userId);
            Assert.IsNotNull(movedItem);
        }

        [Test]
        public async Task DownloadFile()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var itemId = "01XDBAENBSUWBDA5OUCBGKLOETKH2NSLJC";
            var type = GraphResourceTypes.users.ToString();

            var movedItem = await graphFileRepo.DownloadFile(type, itemId, userId);
            Assert.IsNotEmpty(movedItem);
        }
        [Test]
        public async Task TrackChanges()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var type = GraphResourceTypes.users.ToString();

            var items = await graphFileRepo.TrackChanges(type, userId);
            Assert.IsNotEmpty(items);
        }

        [Test]
        public async Task UploadNew()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var type = GraphResourceTypes.users.ToString();
            var path = "New Folder 1/testUploadFile.txt";
            byte[] file = Encoding.ASCII.GetBytes("My text");

            var item = await graphFileRepo.UploadNew(type, path, file, userId);
            Assert.IsNotNull(item);
        }
        [Test]
        public async Task UploadReplace()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var type = GraphResourceTypes.users.ToString();
            var fileId = "01XDBAENDDWIGHOH3CWFBKBZLZOU2TVSAU";
            byte[] file = Encoding.ASCII.GetBytes("My text updated");

            var item = await graphFileRepo.UploadReplace(type, fileId, file, userId);
            Assert.IsNotNull(item);
        }
        [Test]
        public async Task ListVersions()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var type = GraphResourceTypes.users.ToString();
            var fileId = "01XDBAENDDWIGHOH3CWFBKBZLZOU2TVSAU";

            var versions = await graphFileRepo.ListVersions(type, fileId, userId);
            Assert.IsNotEmpty(versions);
        }
        [Test]
        public async Task PreviewItem()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var type = GraphResourceTypes.users.ToString();
            var fileId = "01XDBAENDDWIGHOH3CWFBKBZLZOU2TVSAU";

            var response = await graphFileRepo.PreviewItem(type, fileId, userId);
            Assert.IsNotNull(response);
        }
        [Test]
        public async Task CreateShareLink()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var type = GraphResourceTypes.users.ToString();
            var itemId = "01XDBAENDDWIGHOH3CWFBKBZLZOU2TVSAU";
            var linkType = "view";

            var response = await graphFileRepo.CreateShareLink(type, itemId, linkType, userId);
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task ListPermissions()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var type = GraphResourceTypes.users.ToString();
            var itemId = "01XDBAENDDWIGHOH3CWFBKBZLZOU2TVSAU";

            var permissions = await graphFileRepo.ListPermissions(type, itemId, userId);
            Assert.IsNotEmpty(permissions);
        }

        [Test]
        public async Task GetPermission()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var type = GraphResourceTypes.users.ToString();
            var itemId = "01XDBAENDDWIGHOH3CWFBKBZLZOU2TVSAU";
            var permissionId = "8c98d190-bde5-4bc5-b8cd-85374519c223";

            var permission = await graphFileRepo.GetPermission(type, itemId, permissionId, userId);
            Assert.IsNotNull(permission);
        }

        [Test]
        public async Task AddPermission()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var type = GraphResourceTypes.users.ToString();
            var itemId = "01XDBAENDDWIGHOH3CWFBKBZLZOU2TVSAU";
            var message = "labas";
            var roles = new List<string> { "read", "write" };
            var emails = new List<string> { "mantas.daunoravicius@ktu.edu" };
            var recipients = new List<DriveRecipient>();
            foreach (var em in emails)
            {
                recipients.Add(new DriveRecipient { Email = em });
            }

            var data = new
            {
                requireSignIn = false,//one of these must be true
                sendInvitation = true,
                roles,
                recipients,
                message
            };

            var permission = await graphFileRepo.AddPermission(type, itemId, data, userId);
            Assert.IsNotNull(permission);
        }
        [Test]
        public async Task DeletePermission()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var type = GraphResourceTypes.users.ToString();
            var itemId = "01XDBAENDDWIGHOH3CWFBKBZLZOU2TVSAU";
            var permissionId = "a81342f1-f426-410b-a519-3b04a83eee8a";

            var permission = await graphFileRepo.DeletePermission(type, itemId, permissionId, userId);
            Assert.IsTrue(permission);
        }
        [Test]
        public async Task UpdatePermission()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var type = GraphResourceTypes.users.ToString();
            var itemId = "01XDBAENDDWIGHOH3CWFBKBZLZOU2TVSAU";
            var permissionId = "8c98d190-bde5-4bc5-b8cd-85374519c223";
            var roles = new List<string> { "wirte" };
            var data = new { roles };

            var permission = await graphFileRepo.UpdatePermission(type, itemId, permissionId, data, userId);
            Assert.IsNotNull(permission);
        }

        [Test]
        public async Task ListRecent()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var type = GraphResourceTypes.users.ToString();

            var recent = await graphFileRepo.ListRecent(type, userId);
            Assert.IsEmpty(recent);
        }
        [Test]
        public async Task Search()
        {
            var userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";
            var type = GraphResourceTypes.users.ToString();

            var results = await graphFileRepo.Search(type, ".txt", userId);
            Assert.IsNotEmpty(results);
        }

    }
}