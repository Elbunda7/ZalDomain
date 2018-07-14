﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalDomain.consts
{
    public abstract class ZAL
    {
        public static DateTime DATE_OF_ORIGIN = new DateTime(1997, 2, 1);

        public readonly static string[] GROUP_NAME_PLURAL = { "Neoddíloví", "Zbloudilí jedinci", "Lišky", "Bobři", "Ještěrky", "Svišti", "Veverky", "Trosky" };        
        public readonly static string[] GROUP_NAME_SINGULAR = { "Neoddílový", "Nestálý člen", "Liška", "Bobr", "Ještěrka", "Svišť", "Veverka", "Troska" };
        public readonly static string[] RANK_NAME = { "Liška", "Nováček", "Člen", "Kadet", "Podrádce", "Rádce", "Vedoucí", "Vedoucí", "Hlavní vedoucí" };

        public enum Group
        {
            Non = 1,
            Casual = 2,
            Lisky = 4,
            Bobri = 8,
            Jesterky = 16,
            Svisti = 32,
            Veverky = 64,
            Trosky = 128,

            BasicMemberGroups = 120,
            AllMemberGroups = 124,
            AllClub = 252,
            CompletlyAll = 255,
        }

        public enum UserRole
        {
            Registered = 1,
            Parent = 2,
            Admin = 4,

            All = 7,
        }

        public enum Rank
        {
            Liska = 0,
            Novacek = 1,
            Clen = 2,
            Kadet = 4,
            Podradce = 8,
            Radce = 16,
            Vedouci = 32,
            VedouciRada = 64,
            VedouciHlavni = 128,

            AllLeaders = 224,
            All = 255,
        }

        public abstract class MEMBERSHIP
        {
            public static string NECLEN = "nečlen";
            public static string CLEN = "člen";
        }

        public abstract class ACTUALITY_TYPE
        {
            public const string INFO = "info";
            public const string RECORD = "zapis";
            public const string ARTICLE = "clanek";
        }

        public abstract class YEAR
        {
            public const int UPCOMING = 9999;
        }

        public enum ActionUserRole
        {
            Garant, Member, Any
        }
    }
}
