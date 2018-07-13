using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalApiGateway.Models;

namespace ZalDomain.Models
{
    public interface IUpdatableModel
    {
        void CopyFrom(IModel apiModel);
        void CopyInto(IModel apiModel);
    }
}
