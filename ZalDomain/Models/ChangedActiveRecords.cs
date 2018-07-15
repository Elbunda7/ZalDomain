using System;
using System.Collections.Generic;
using ZalApiGateway.Models;
using ZalApiGateway.Models.ApiCommunicationModels;
using ZalDomain.ActiveRecords;

namespace ZalDomain.Models
{
    public class ChangedActiveRecords<T, M> where T : IActiveRecord where M : IModel
    {
        public bool IsChanged { get; set; }
        public bool IsHardChanged { get; set; }
        public bool HasAnyChanges => IsChanged || IsHardChanged;
        public DateTime Timestamp { get; set; }
        public int[] Deleted { get; set; }
        public IEnumerable<T> Changed { get; set; }

        public ChangedActiveRecords(ChangesRespondModel<M> rawModel, IEnumerable<T> activeRecords) {
            IsChanged = rawModel.IsChanged;
            IsHardChanged = rawModel.IsHardChanged;
            Timestamp = rawModel.Timestamp;
            Deleted = rawModel.GetDeleted();
            Changed = activeRecords;
        }
    }
}
