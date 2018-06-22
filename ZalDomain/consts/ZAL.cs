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

        private static string[] groupNamePlur = {"non", "Lišky", "Bobři", "Ještěrky", "Svišti", "Veverky", "Trosky"};
        public static string[] GROUP_NAME_PLUR => groupNamePlur;

        private static string[] groupNameSing = { "non", "Liška", "Bobr", "Ještěrka", "Svišť", "Veverka", "Troska" };
        public static string[] GROUP_NAME_SING => groupNameSing;

        private static string[] rankName = { "Nováček", "Nováček", "Člen", "Kadet", "Podrádce", "Rádce", "Vedoucí", "Vedoucí", "Hlavní vedoucí" };
        public static string[] RANK_NAME => rankName;//enums

        public abstract class GROUP
        {
            public const int NON = 0;
            public const int LISKY = 1;
            public const int BOBRI = 2;
            public const int JESTERKY = 3;
            public const int SVISTI = 4;
            public const int VEVERKY = 5;
            public const int TROSKY = 6;
        }

        public abstract class RANK
        {
            public static int LISKA = 0;
            public static int NOVACEK = 1;
            public static int CLEN = 2;
            public static int KADET = 3;
            public static int PODRADCE = 4;
            public static int RADCE = 5;
            public static int VEDOUCI = 6;
            public static int VEDOUCI_RADA = 7;
            public static int HLAVNI_VEDOUCI = 8;
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
