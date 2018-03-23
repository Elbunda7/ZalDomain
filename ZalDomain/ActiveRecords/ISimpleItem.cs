using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalDomain.ActiveRecords
{
    public interface ISimpleItem
    {
        //int Id { get; }
        string Title { get; }
        string Text { get; }
    }
}
