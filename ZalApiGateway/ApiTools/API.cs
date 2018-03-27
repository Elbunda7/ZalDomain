using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.ApiTools
{
    internal abstract class API
    {
        internal abstract class ENDPOINT
        {
            public const string ACTIONS = "actions";
            public const string ARTICLES = "articles";
            public const string BADGES = "badges";
            public const string DOCUMENTS = "docs";
            public const string GALLERY = "gallery";
            public const string GROUPS = "groups";
            public const string RANKS = "ranks";
            public const string USERS = "users";
        }

        internal abstract class METHOD
        {
            public const string GET = "Get";
            public const string GET_ALL = "GetAll";
            public const string ADD = "Add";
            public const string UPDATE = "Update";
            public const string DELETE = "Delete";
            public const string JOIN = "Join";
            public const string GET_CHANGED = "GetChanged";
            public const string GET_ALL_BY_YEAR = "GetAllByYear";
        }
    }
}
