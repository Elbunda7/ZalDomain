using System;
using System.Collections.Generic;
using System.Linq;
using ZalApiGateway.Models;
using ZalApiGateway.Models.ApiCommunicationModels;
using ZalDomain.ActiveRecords;

namespace ZalDomain.Models
{
    public class AllActiveRecords<T> where T : IActiveRecord
    {
        public DateTime Timestamp { get; set; }
        public IEnumerable<T> ActiveRecords { get; set; }

        public AllActiveRecords(AllRespondModel<ActionModel> rawModel) {
            Timestamp = rawModel.Timestamp;
            ActiveRecords = rawModel.GetItems().Select(x => new ActionEvent(x)) as IEnumerable<T>;
        }
    }
}
