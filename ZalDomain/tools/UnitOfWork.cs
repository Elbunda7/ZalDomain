using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalApiGateway.Models;

namespace ZalDomain.tools
{
    public class UnitOfWork<T> where T : IModel
    {
        private T currentModel;
        private Func<Task<bool>> onUpdateCommited;

        public T ToUpdate { get; private set; }

        public UnitOfWork(T model, Func<Task<bool>> callback) {
            currentModel = model;
            ToUpdate = (T)model.Copy();
            onUpdateCommited = callback;
        }

        public void UndoChanges() {
            ToUpdate = (T)currentModel.Copy();
        }

        public Task<bool> CommitAsync() {
            currentModel = (T)ToUpdate.Copy();
            return onUpdateCommited.Invoke();
        }
    }
}
