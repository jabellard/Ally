using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Ally.Threading
{
    public class LazyTask<T>: Lazy<Task<T>>
    {
        public LazyTask(Func<T> valueFactory):
            base(() => Task.Factory.StartNew(valueFactory))
        {
        }

        public LazyTask(Func<Task<T>> taskFactory):
            base(() => Task.Factory.StartNew(taskFactory).Unwrap())
        {
        }

        public TaskAwaiter<T> GetAwaiter() => Value.GetAwaiter();
    }
}