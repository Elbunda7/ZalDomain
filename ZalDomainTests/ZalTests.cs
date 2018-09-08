using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZalDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalDomain.ActiveRecords;
using ZalDomain.consts;
using ZalDomain.Models;

namespace ZalDomain.Tests
{
    [TestClass()]
    public class ZalTests
    {
        
        [TestMethod()]
        public async Task ActionsTest() {
            await Zal.Session.LoginAsync("pepa3@email.cz", "password", true);
            await Zal.Actions.SynchronizeAllCurrentlyActive();
            Assert.IsTrue(await Zal.Actions.AddNewActionAsync("test", "test", new DateTime(9000, 1,1), new DateTime(9000, 1, 3), 0, true));
            var actions = await Zal.Actions.GetPassedActionEventsByYear(9000);
            actions = await Zal.Actions.GetPassedActionEventsByYear(9000);
            actions = Zal.Actions.UpcomingActionEvents;
            //await Zal.Actions.ReSynchronizeAsync();
            //Assert.AreEqual(1, actions.Count);
            var act = actions.First();
            act.UnitOfWork.ToUpdate.EventType = "typ";
            Assert.IsTrue(await act.UnitOfWork.CommitAsync());
            Assert.IsTrue(await act.AddNewInfoAsync("titleInfo", "textInfo"));
            Assert.IsTrue(await act.AddNewReportAsync("titleRep", "textRep"));
            Assert.IsTrue(await act.DeleteAsync());
        }

        [TestMethod()]
        public async Task ActualityTest() {
            await Zal.Actualities.Synchronize();
            var a = Zal.Actualities.Data;
            await Zal.Actualities.LoadNext();
            //var a = await Article.LoadTopTen(new int[0], DateTime.Now);
            //var b = await Article.LoadTopTen(new int[] { 6, 24, 22, 20, 18, 16, 14, 1, 10, 3 }, DateTime.Now);
            //var c = await Article.LoadTopTen(new int[] { 6, 24, 22, 20, 18, 16, 14, 1, 10, 3 }, DateTime.Now);
            //await Zal.Actualities.SynchronizeAsync();
            await Zal.Session.LoginAsync("pepa3@email.cz", "password", true);
            Assert.IsTrue(await Zal.Actualities.AddNewArticle("Article", "Lorem ipsum", 0));
            //await Zal.Users.AddNewUser("name", "surname", (int)ZAL.Group.Bobri);
            /* await Zal.Users.SynchronizeUsers();
             var a = Zal.Users.Users;
             var filter = UserFilterModel.Default;
             filter.Groups &= ~ZAL.Group.Bobri;
             await Zal.Users.SynchronizeUsers(filter);
             var b = Zal.Users.Users;*/
        }

        [TestMethod()]
        public async Task MemberOnActionTest() {
            //await Zal.Actions.AddNewActionAsync("false", "fal", DateTime.Now, DateTime.Now, 1, true);
            await Zal.Session.LoginAsync("pepa3@email.cz", "password", true);
            //await Zal.Actions.AddNewActionAsync("true", "tru", DateTime.Now, DateTime.Now, 1, true);
            var a = await Zal.Actions.GetPassedActionEventsByYear(1999);
            /*bool b = await a.First().Join();
            bool c = await a.First().Join(true);
            bool d = await a.First().UnJoin();      */    
        }

        [TestMethod()]
        public async Task SessionTest() {
            var a = await Zal.Session.LoginAsync("pepa3@email.cz", "password", false);

            var bo = await Zal.Badges.Add("Uzlař", "vázej", "obrázek");
            var s = Zal.Badges.Badges.First();
            s.UnitOfWork.ToUpdate.Image = "img";
            Assert.IsTrue(await s.UnitOfWork.CommitAsync());
            await Zal.Badges.Synchronize();
            var b = Zal.Badges.Badges.First();
            await Zal.Badges.Synchronize();
            var c = Zal.Badges.Badges.First();
            await Zal.Session.Logout();
            /*bool isRegistered = await Zal.Session.RegisterAsync("Pepa", "Zdepa", "999456236", "pepa3@email.cz", "password");
            if (!isRegistered) {
                var a = await Zal.Session.LoginAsync("pepa3@email.cz", "password", true);
                Assert.IsFalse(a.HasAnyErrors);
                var tok = Zal.Session.Token;
                await Zal.Session.LoginWithTokenAsync();
                var tok2 = Zal.Session.Token;
                Assert.AreNotEqual(tok, tok2);
                Zal.Session.RefreshToken = "false";
                await Zal.Session.LoginWithTokenAsync();
                Assert.IsNull(Zal.Session.Token);
            }*/
            //Assert.IsTrue(Zal.Session.IsLogged);
        }

        [TestMethod()]
        public async Task ZalTest() {
            //Zal.LoadDataFrom("{\n  \"session\": {\n    \"StayLogged\": true,\n    \"RefreshToken\": \"9a1244e234f041454ee80f94e36b5a2a46d6b979896427ed1be5f030b4d5ded1\",\n    \"CurrentUser\": {\n      \"Id\": 13,\n      \"NickName\": \"Pepa Z.\",\n      \"Name\": \"Pepa\",\n      \"Surname\": \"Zdepa\",\n      \"Email\": \"pepa3@email.cz\",\n      \"Phone\": \"999456236\",\n      \"BirthDate\": null,\n      \"Id_Rank\": 6,\n      \"Id_Group\": null\n    }\n  },\n  \"actions\": [\n    {\n      \"Id\": 88,\n      \"Name\": \"\",\n      \"Date_start\": \"2018-02-13T00:00:00\",\n      \"Date_end\": \"2018-02-14T00:00:00\",\n      \"EventType\": \"cyklo\",\n      \"FromRank\": 0,\n      \"IsOfficial\": true,\n      \"Id_Gallery\": null,\n      \"Id_Info\": 8,\n      \"Id_Report\": 3\n    },\n    {\n      \"Id\": 201,\n      \"Name\": \"a\",\n      \"Date_start\": \"2018-06-09T00:00:00\",\n      \"Date_end\": \"2018-06-09T00:00:00\",\n      \"EventType\": \"b\",\n      \"FromRank\": 0,\n      \"IsOfficial\": true,\n      \"Id_Gallery\": null,\n      \"Id_Info\": null,\n      \"Id_Report\": null\n    }\n  ]\n}");
            //var a= await Zal.Session.TryLoginWithTokenAsync();
            //await Zal.StartSynchronizingAsync();
            await Zal.Actions.SynchronizeAllCurrentlyActive();
            bool a = Zal.Actions.UpcomingActionEvents.First().DoIParticipate;
            //await Zal.Actions.SynchronizeAllCurrentlyActive();
            //var a = Zal.GetDataJson();
            //Zal.LoadDataFrom(a);
            //var v = Zal.GetDataJson();
        }
    }
}