using System;
using System.Collections.Generic;
using System.Linq;
using ZalApiGateway.Models;
using ZalApiGateway.Models.ApiCommunicationModels;
using ZalDomain.ActiveRecords;

namespace ZalDomain.Models
{
    public class ChangedActiveRecords<T> where T : IActiveRecord
    {
        public bool IsChanged { get; set; }
        public bool IsHardChanged { get; set; }
        public bool HasAnyChanges => IsChanged || IsHardChanged;
        public DateTime Timestamp { get; set; }
        public int[] Deleted { get; set; }
        public IEnumerable<T> Changed { get; set; }

        public ChangedActiveRecords(ChangesRespondModel<IModel> rawModel) {
        }

        public ChangedActiveRecords(ChangesRespondModel<ActionModel> rawModel) {
            IsChanged = rawModel.IsChanged;
            IsHardChanged = rawModel.IsHardChanged;
            Timestamp = rawModel.Timestamp;
            Deleted = rawModel.GetDeleted();
            Changed = rawModel.GetChanged().Select(x => new ActionEvent(x)) as IEnumerable<T>;
        }
    }
}
