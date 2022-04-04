using System;
using System.Runtime.CompilerServices;

namespace MultiInput.Internal
{
    public class CommitLockVal<T>
    {
        private T currentValue;
        private T committedValue;

        public CommitLockVal(T initialValue = default)
        {
            currentValue = committedValue = initialValue;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Reset(T initialValue = default)
        {
            currentValue = committedValue = initialValue;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetCurrent(T value)
        {
            currentValue = value;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Commit(T newCurrentValue = default)
        {
            committedValue = currentValue;
            currentValue = newCurrentValue;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public T GetCurrent()
        {
            return currentValue;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public T GetCommitted()
        {
            return committedValue;
        }
    }
}