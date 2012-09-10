using System;

namespace AviFile
{
	public abstract class AviStream : IDisposable
	{
		protected int aviFile;
		protected IntPtr aviStream;
		protected IntPtr compressedStream;
		protected bool writeCompressed;

        /// <summary>Pointer to the unmanaged AVI file</summary>
        internal int FilePointer {
            get { return aviFile; }
        }

        /// <summary>Pointer to the unmanaged AVI Stream</summary>
        internal virtual IntPtr StreamPointer {
            get { return aviStream; }
        }

        /// <summary>Flag: The stream is compressed/uncompressed</summary>
        internal bool WriteCompressed {
            get { return writeCompressed; }
        }

        /// <summary>Close the stream</summary>
        public virtual void Close(){
			if(writeCompressed&&compressedStream!=IntPtr.Zero){
				Avi.AVIStreamRelease(compressedStream);
                compressedStream = IntPtr.Zero;
			}
            if (aviStream != IntPtr.Zero)
            {
                Avi.AVIStreamRelease(aviStream);
                aviStream = IntPtr.Zero;
            }
		}

        /// <summary>Export the stream into a new file</summary>
        /// <param name="fileName"></param>
		public abstract void ExportStream(String fileName);


        #region IDisposable ≥…‘±
        protected bool _disposed = false;

        ~AviStream()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposed)
            {
                Close();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
