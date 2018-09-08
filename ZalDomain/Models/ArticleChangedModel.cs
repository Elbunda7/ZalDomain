using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalApiGateway.Models.ApiCommunicationModels;
using ZalDomain.ActiveRecords;

namespace ZalDomain.Models
{
    public class ArticleChangedModel : BaseChangedActiveRecords<Article>
    {
        public int[] Ids { get; set; }

        public ArticleChangedModel(ArticlesChangesRespondModel rawModel, IEnumerable<Article> activeRecords) : base(rawModel, activeRecords) {
            Ids = rawModel.Ids;
        }
    }
}
