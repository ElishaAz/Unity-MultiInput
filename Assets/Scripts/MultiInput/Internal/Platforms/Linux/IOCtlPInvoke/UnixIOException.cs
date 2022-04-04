using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace MultiInput.Internal.Platforms.Linux.IOCtlPInvoke
{
    [Serializable]
    public class UnixIOException : ExternalException
    {
        private readonly int nativeErrorCode;

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public UnixIOException()
            : this(Marshal.GetLastWin32Error())
        {
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public UnixIOException(int error)
            : this(error, GetErrorMessage(error))
        {
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public UnixIOException(string message)
            : this(Marshal.GetLastWin32Error(), message)
        {
        }

        public UnixIOException(int error, string message)
            : base(message)
        {
            this.nativeErrorCode = error;
        }

        public UnixIOException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UnixIOException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.nativeErrorCode = info.GetInt32("NativeErrorCode");
        }

        public int NativeErrorCode => this.nativeErrorCode;

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("NativeErrorCode", this.nativeErrorCode);
            base.GetObjectData(info, context);
        }

        private static string GetErrorMessage(int error)
        {
            var errorDescription = UnsafeNativeMethods.Strerror(error);
            return errorDescription ?? $"Unknown error (0x{error:x})";
        }
    }

}