using System;
using System.Runtime.InteropServices;
using System.Security;
using JetBrains.Annotations;

namespace libKeyFinder.NET
{
    [PublicAPI]
    public sealed class KeyDetector : IDisposable
    {
        #region Private

        private unsafe __AudioData* _audioData;
        private unsafe __KeyFinder* _keyFinder;
        private unsafe __Workspace* _workspace;

        #endregion

        #region Public

        public unsafe KeyDetector(int frameRate, int channels, int frameLength)
        {
            if (frameRate <= 0)
                throw new ArgumentOutOfRangeException(nameof(frameRate));

            if (channels <= 0)
                throw new ArgumentOutOfRangeException(nameof(channels));

            if (frameLength <= 0)
                throw new ArgumentOutOfRangeException(nameof(frameLength));

            _audioData = new_audio_data((uint) frameRate, (uint) channels, (uint) frameLength);
            _keyFinder = new_keyfinder();
            _workspace = new_workspace();
        }

        public unsafe void SetSample(uint index, double value)
        {
            audio_data_set_sample(_audioData, index, value);
        }

        public unsafe void ProgressiveChromagram()
        {
            keyfinder_progressive_chromagram(_keyFinder, _audioData, _workspace);
        }

        public unsafe void FinalChromagram()
        {
            keyfinder_final_chromagram(_keyFinder, _workspace);
        }

        public unsafe Key KeyOfChromagram()
        {
            return keyfinder_key_of_chromagram(_keyFinder, _workspace);
        }

        #endregion

        #region IDisposable

        private bool IsDisposed { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private unsafe void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            delete_audio_data(_audioData);
            delete_keyfinder(_keyFinder);
            delete_workspace(_workspace);

            if (disposing)
            {
                // nothing
            }

            IsDisposed = true;
        }

        ~KeyDetector()
        {
            Dispose(false);
        }

        #endregion

        #region Native methods

        [SuppressUnmanagedCodeSecurity]
        [DllImport("libKeyFinder", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe __Workspace* new_workspace(
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("libKeyFinder", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void delete_workspace(
            __Workspace* workspace
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("libKeyFinder", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe __KeyFinder* new_keyfinder(
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("libKeyFinder", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void delete_keyfinder(
            __KeyFinder* keyFinder
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("libKeyFinder", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void keyfinder_progressive_chromagram(
            __KeyFinder* keyFinder,
            __AudioData* audioData,
            __Workspace* workspace
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("libKeyFinder", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe Key keyfinder_key_of_chromagram(
            __KeyFinder* keyFinder,
            __Workspace* workspace
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("libKeyFinder", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void keyfinder_final_chromagram(
            __KeyFinder* keyFinder,
            __Workspace* workspace
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("libKeyFinder", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe __AudioData* new_audio_data(
            uint frameRate,
            uint channels,
            uint samples
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("libKeyFinder", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void delete_audio_data(
            __AudioData* audioData
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("libKeyFinder", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void audio_data_set_sample(
            __AudioData* audioData,
            uint index,
            double value
        );

        #endregion
    }

   
}