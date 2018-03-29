using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZalDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalDomain.ActiveRecords;

namespace ZalDomain.Tests
{
    [TestClass()]
    public class ZalTests
    {
        
        [TestMethod()]
        public async Task ActionsTest() {
            await Zal.Actions.SynchronizeAsync();
            Assert.IsTrue(await Zal.Actions.AddNewActionAsync("test", "test", new DateTime(9000, 1,1), new DateTime(9000, 1, 3), 0, true));
            var actions = await Zal.Actions.GetActionEventsByYearAsync(9000);
            Assert.AreEqual(1, actions.Count);
            var act = actions.First();
            Assert.IsTrue(await act.AktualizeAsync(null, "typ", null, null, null, null));
            Assert.IsTrue(await act.AddNewInfoAsync("titleInfo", "textInfo"));
            Assert.IsTrue(await act.AddNewReportAsync("titleRep", "textRep"));
            Assert.IsTrue(await act.DeleteAsync());
        }

        [TestMethod()]
        public async Task ActualityTest() {
            await Zal.Actualities.SynchronizeAsync();
            Assert.IsTrue(await Zal.Actualities.AddNewArticle("title", "test", 0));
        }

        [TestMethod()]
        public async Task UsersTest() {
            await Zal.RegisterAsync("Pepa", "Zdepa", "999456236", "pepa@email.cz", "1234");
            Assert.IsTrue(Zal.Session.IsLogged);
        }
    }
}