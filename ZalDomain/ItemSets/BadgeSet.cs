﻿using ZalDomain.ActiveRecords;
using ZalDomain.consts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ZalDomain.ItemSets
{
    public class BadgeSet {
        public Collection<Badge> Badges { get; private set; }

        private DateTime LastCheck;

        public BadgeSet() {
            Badges = new Collection<Badge>();
            LastCheck = ZAL.DATE_OF_ORIGIN;
        }

        internal async void Synchronize() {
            if (LastCheck == ZAL.DATE_OF_ORIGIN) {
                Badges = (await Badge.GetAllAsync()) as Collection<Badge>;
                LastCheck = DateTime.Now;
            }
        } 

        internal void ReSynchronize() {
            LastCheck = ZAL.DATE_OF_ORIGIN;
            Synchronize();
        }

        //internal Collection<Badge> GetAcquired() {
        //    return GetAcquired(Zal.Me);
        //}

        internal async Task<IEnumerable<Badge>> GetAcquired(User user) {//vrátit jen idčka
            return await Badge.GetAcquiredAsync(user);
        }

        internal XElement GetXml(string elementName) {
            throw new NotImplementedException();
        }

        internal void LoadFromXml(XElement xElement) {
            throw new NotImplementedException();
        }

        public Badge GetByName(string value) {
            foreach (Badge badge in Badges) {
                if (badge.Title == value) {
                    return badge;
                }
            }
            return Badges.Last();
        }
    }
}
