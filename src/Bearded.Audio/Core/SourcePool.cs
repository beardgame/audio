using System;
using System.Collections.Generic;
using System.Linq;

namespace Bearded.Audio {
    /// <summary>
    /// Represents a pool of sources that can be reused to prevent continuous allocation and deallocation, and stops
    /// the program from using more sources than allowed by the sound driver.
    /// </summary>
    public sealed class SourcePool : IDisposable {
        
        private readonly List<Source> sources;
        private readonly Queue<Source> availableSources;

        private bool hasReachedCapacity => sources.Count == Capacity;

        private bool hasAvailableAllocatedSource => availableSources.Count > 0;

        /// <summary>
        /// The number of sources this source pool manages.
        /// </summary>
        public int Capacity => sources.Capacity;

        /// <summary>
        /// Whether there is currently a source available for playing.
        /// </summary>
        public bool HasAvailableSource => hasAvailableAllocatedSource || !hasReachedCapacity;

        /// <summary>
        /// Creates a new source pool with the specified number of sources, and eagerly allocates the sources to this
        /// pool.
        /// </summary>
        public static SourcePool CreateInstanceAndAllocateSources(int numSources) {
            var sourcePool = CreateInstance(numSources);
            
            for (var i = 0; i < numSources; i++) {
                sourcePool.allocateNewSource();
            }

            return sourcePool;
        }
        
        /// <summary>
        /// Creates a new source pool 
        /// </summary>
        public static SourcePool CreateInstance(int numSources) => new SourcePool(numSources);
        
        private SourcePool(int numSources) {
            if (numSources <= 0) {
                throw new ArgumentException(
                    "Cannot create a source pool with a non-positive number of sources", nameof(numSources));
            }
            
            sources = new List<Source>(numSources);
            availableSources = new Queue<Source>();
        }

        /// <summary>
        /// Gets an available source. Throws an exception if no source could be assigned.
        /// </summary>
        public Source GetSource() {
            if (TryGetSource(out var source)) {
                return source;
            }
            
            throw new InvalidOperationException("No available sources.");
        }

        /// <summary>
        /// Attempts to get an available source. Will return false if no source could be assigned.
        /// </summary>
        public bool TryGetSource(out Source source) {
            if (!HasAvailableSource) {
                source = null;
                return false;
            }
            
            if (!hasAvailableAllocatedSource) {
                allocateNewSource();
            }
            
            source = availableSources.Dequeue();
            return true;
        }

        private void allocateNewSource() {
            if (hasReachedCapacity) {
                throw new InvalidOperationException("Cannot allocate a new source because capacity reached.");
            }
            
            var source = new Source();
            sources.Add(source);
            availableSources.Enqueue(source);
        }

        /// <summary>
        /// Reclaims all sources that are not currently playing any sound.
        /// </summary>
        public void ReclaimAllFinishedSources() {
            foreach (var source in sources.Where(s => s.FinishedPlaying)) {
                ReclaimSource(source);
            }
        }

        /// <summary>
        /// Reclaims a source by adding it back to the available sources pool.
        ///
        /// <para>Sources to be reclaimed should no longer be playing and should not be disposed.</para>
        /// </summary>
        public void ReclaimSource(Source source) {
            if (source == null) {
                throw new ArgumentNullException(nameof(source));
            }
            if (!sources.Contains(source)) {
                throw new ArgumentException(
                    "Cannot reclaim a source that is not part of this source pool", nameof(source));
            }
            if (source.Disposed) {
                throw new ArgumentException("Cannot reclaim a disposed source.", nameof(source));
            }
            if (!source.FinishedPlaying) {
                throw new ArgumentException("Cannot reclaim a source that has not finished playing", nameof(source));
            }
            
            resetSource(source);
            availableSources.Enqueue(source);
        }

        private static void resetSource(Source source) {
            source.DequeueProcessedBuffers();
            source.Rewind();
        }

        public void Dispose() {
            releaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~SourcePool() {
            releaseUnmanagedResources();
        }

        private void releaseUnmanagedResources() {
            foreach (var source in sources) {
                source.Dispose();
            }
            sources.Clear();
            availableSources.Clear();
        }
    }
}
