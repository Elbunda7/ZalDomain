using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalDomain.consts
{
    public abstract class ZAL
    {
        public static DateTime DATE_OF_ORIGIN = new DateTime(1997, 2, 1);

        private static string[] groupNamePlural = {"Neoddíloví", "Zbloudilí jedinci", "Lišky", "Bobři", "Ještěrky", "Svišti", "Veverky", "Trosky"};
        public static string[] GROUP_NAME_PLURAL => groupNamePlural;

        private static string[] groupNameSingular = { "Neoddílový", "Nestálý člen", "Liška", "Bobr", "Ještěrka", "Svišť", "Veverka", "Troska" };
        public static string[] GROUP_NAME_SINGULAR => groupNameSingular;

        private static string[] rankName = { "Liška", "Nováček", "Člen", "Kadet", "Podrádce", "Rádce", "Vedoucí", "Vedoucí", "Hlavní vedoucí" };
        public static string[] RANK_NAME => rankName;

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
        }

        public enum UserRoles
        {
            Registered,
            Parent,
            Admin,
        }

        public enum Rank
        {
            Liska = 0,
            Novacek = 1,
            Clen = 2,
            Kadet = 3,
            Podradce = 4,
            Radce = 5,
            Vedouci = 6,
            VedouciRada = 7,
            VedouciHlavni = 8,
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
