using ZalDomain.ActiveRecords;
using System.Collections.Generic;


namespace ZalDomain.tools
{
    public class ActualityComparer : Comparer<Article>
    {
        public override int Compare(Article x, Article y) {
            int comparison = Comparer<long>.Default.Compare(x.DateOfCreation.Ticks, y.DateOfCreation.Ticks);
            comparison *= -1;
            if (comparison == 0) {
                comparison = Comparer<int>.Default.Compare(x.Id, y.Id);
            } 
            return comparison;
        }
    }
}
