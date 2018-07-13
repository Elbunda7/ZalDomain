using System;
using System.Threading.Tasks;
using ZalApiGateway.Models;
using ZalDomain.Models;

namespace ZalDomain.tools
{
    public class UnitOfWork<T> where T : IUpdatableModel
    {
        private IModel currentModel;
        private Func<Task<bool>> onUpdateCommited;

        public T ToUpdate { get; private set; }

        internal UnitOfWork(IModel model, Func<Task<bool>> callback) {
            currentModel = model;
            ToUpdate = default(T);
            ToUpdate.CopyFrom(model);
            onUpdateCommited = callback;
        }

        public void UndoChanges() {
            ToUpdate.CopyFrom(currentModel);
        }

        public Task<bool> CommitAsync() {
            ToUpdate.CopyInto(currentModel);
            return onUpdateCommited.Invoke();
        }
    }
}
