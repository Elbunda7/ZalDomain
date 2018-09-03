using System;
using System.Collections.Generic;
using ZalApiGateway.Models;
using ZalApiGateway.Models.ApiCommunicationModels;
using ZalDomain.ActiveRecords;

namespace ZalDomain.Models
{
    public class BaseChangedActiveRecords<AR> where AR : IActiveRecord
    {
        public bool IsChanged { get; set; }
        public DateTime Timestamp { get; set; }
        public IEnumerable<AR> Changed { get; set; }

        public BaseChangedActiveRecords(BaseChangesRespondFlags rawModel, IEnumerable<AR> activeRecords) {
            IsChanged = rawModel.IsChanged;
            Timestamp = rawModel.Timestamp;
            Changed = activeRecords;
        }
    }

    public class ChangedActiveRecords<AR> : BaseChangedActiveRecords<AR> where AR : IActiveRecord 
    {
        public bool IsHardChanged { get; set; }
        public bool HasAnyChanges => IsChanged || IsHardChanged;
        public int[] Deleted { get; set; }

        public ChangedActiveRecords(FullChangesRespondFlags rawModel, IEnumerable<AR> activeRecords) : base(rawModel, activeRecords) {
            IsHardChanged = rawModel.IsHardChanged;
            Deleted = rawModel.Deleted;
        }
    }
}
