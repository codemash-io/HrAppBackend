using HrApp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    public class GraphTests
    {
        GraphRepository graphRepo;
        RoomBookerService roomService;

        [SetUp]
        public void Setup()
        {
            graphRepo = new GraphRepository();
            roomService = new RoomBookerService()
            {
                GraphRepository = graphRepo
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

            var result = await graphRepo.GetCalendarEventsByDate(meetingRoom, from, to);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public async Task GetAllCalenadrEvents()
        {
            var meetingRoom = MeetingRooms.Hamburg.ToString();

            var result = await graphRepo.GetAllCalendarEvents(meetingRoom);
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
        public async Task GetOffice365UserDetails()
        {
            string userId = "de2a4f5a-5370-40b4-918d-62e0ee1b867b";

            var user = await graphRepo.GetOffice365UserById(userId);
            Assert.IsNotNull(user);
        }

        [Test]
        public async Task DeleteEventById()
        {
            string eventId = "AAMkADUwZGM3ZDBhLWEyOGQtNGI3My04NTE4LTYyZjg4Yzg3NDc5MwBGAAAAAADRRo8_4__FSbFENgFO65X1BwCVbXfSRkx5TowOq2ZibiTBAAAAAAENAACVbXfSRkx5TowOq2ZibiTBAAD0p8aVAAA=";
            string roomName = MeetingRooms.Hamburg.ToString();

            var isDeleted = await graphRepo.DeleteEventById(eventId, roomName);

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
    }
}