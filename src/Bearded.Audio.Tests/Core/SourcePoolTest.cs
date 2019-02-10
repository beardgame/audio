using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace Bearded.Audio.Tests {
    public sealed class SourcePoolTest {
        private readonly Mock<ISourceService> svcMock;
        
        public SourcePoolTest() {
            svcMock = new Mock<ISourceService>();
            SourceService.SetTestInstance(svcMock.Object);
            BufferService.SetTestInstance(new Mock<IBufferService>().Object);
        }
        
        [Fact]
        public void CreateInstanceAndAllocateSources_CreatesInstanceWithCorrectCapacity() {
            const int numSources = 10;
            
            var instance = SourcePool.CreateInstanceAndAllocateSources(numSources);

            instance.Capacity.Should().Be(numSources);
        }
        
        [Fact]
        public void CreateInstanceAndAllocateSources_ThrowsExceptionForZeroCapacity() {
            Action createInstanceAndAllocateSources = () => SourcePool.CreateInstanceAndAllocateSources(0);

            createInstanceAndAllocateSources.Should().Throw<ArgumentException>();
        }
        
        [Fact]
        public void CreateInstanceAndAllocateSources_ThrowsExceptionForNegativeCapacity() {
            Action createInstanceAndAllocateSources = () => SourcePool.CreateInstanceAndAllocateSources(-42);

            createInstanceAndAllocateSources.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateInstanceAndAllocateSources_ImmediatelyAllocatesSources() {
            const int numSources = 7;
            
            SourcePool.CreateInstanceAndAllocateSources(numSources);
            
            svcMock.Verify(svc => svc.Generate(), Times.Exactly(numSources));
        }

        [Fact]
        public void CreateInstance_CreatesInstanceWithCorrectCapacity() {
            const int numSources = 19;
            
            var instance = SourcePool.CreateInstance(numSources);

            instance.Capacity.Should().Be(numSources);
        }
        
        [Fact]
        public void CreateInstance_ThrowsExceptionForZeroCapacity() {
            Action createInstance = () => SourcePool.CreateInstance(0);

            createInstance.Should().Throw<ArgumentException>();
        }
        
        [Fact]
        public void CreateInstance_ThrowsExceptionForNegativeCapacity() {
            Action createInstance = () => SourcePool.CreateInstance(-32);

            createInstance.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateInstanceAndAllocateSources_DoesNotImmediatelyAllocateSources() {
            const int numSources = 4;
            
            SourcePool.CreateInstance(numSources);
            
            svcMock.Verify(svc => svc.Generate(), Times.Never);
        }

        [Fact]
        public void GetSource_ReturnsASource() {
            var instance = SourcePool.CreateInstance(1);

            var source = instance.GetSource();

            source.Should().BeAssignableTo<Source>().And.NotBeNull();
        }

        [Fact]
        public void GetSource_AllocatesNewSourceIfNoneAllocated() {
            var instance = SourcePool.CreateInstance(1);
            
            svcMock.Invocations.Clear();
            
            instance.GetSource();

            svcMock.Verify(svc => svc.Generate(), Times.Once);
        }

        [Fact]
        public void GetSource_ThrowsExceptionIfThereIsNoAvailableSource() {
            var instance = SourcePool.CreateInstance(1);
            instance.GetSource();

            Action getSource = () => instance.GetSource();

            getSource.Should().Throw<InvalidOperationException>();
        }
        
        [Fact]
        public void GetSource_ShouldUseUpCapacity() {
            var instance = SourcePool.CreateInstance(1);
            
            instance.GetSource();

            instance.HasAvailableSource.Should().BeFalse();
        }
        
        [Fact]
        public void TryGetSource_ReturnsTrueIfASourceIsAvailable() {
            var instance = SourcePool.CreateInstance(1);

            var result = instance.TryGetSource(out _);

            result.Should().BeTrue();
        }

        [Fact]
        public void TryGetSource_ReturnsASource() {
            var instance = SourcePool.CreateInstance(1);

            instance.TryGetSource(out var source);

            source.Should().BeAssignableTo<Source>().And.NotBeNull();
        }

        [Fact]
        public void TryGetSource_AllocatesNewSourceIfNoneAllocated() {
            var instance = SourcePool.CreateInstance(1);
            
            svcMock.Invocations.Clear();
            
            instance.TryGetSource(out _);

            svcMock.Verify(svc => svc.Generate(), Times.Once);
        }

        [Fact]
        public void TryGetSource_ReturnsFalseIfThereIsNoAvailableSource() {
            var instance = SourcePool.CreateInstance(1);
            instance.GetSource();

            var result = instance.TryGetSource(out _);

            result.Should().BeFalse();
        }
        
        [Fact]
        public void TryGetSource_ShouldUseUpCapacity() {
            var instance = SourcePool.CreateInstance(1);
            
            instance.TryGetSource(out _);

            instance.HasAvailableSource.Should().BeFalse();
        }

        [Fact]
        public void ReclaimSource_ThrowsOnNull() {
            var instance = SourcePool.CreateInstance(1);

            Action reclaimSource = () => instance.ReclaimSource(null);

            reclaimSource.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ReclaimSource_ThrowsForDisposedSource() {
            var instance = SourcePool.CreateInstance(1);
            var source = instance.GetSource();
            source.Dispose();

            Action reclaimSource = () => instance.ReclaimSource(source);

            reclaimSource.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ReclaimSource_ThrowsForSourceNotInSourcePool() {
            var instance = SourcePool.CreateInstance(1);
            var source = new Source();

            Action reclaimSource = () => instance.ReclaimSource(source);

            reclaimSource.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ReclaimSource_ThrowsIfSourceIsStillPlaying() {
            var instance = SourcePool.CreateInstance(1);
            var source = instance.GetSource();
            source.Looping = true;
            
            Action reclaimSource = () => instance.ReclaimSource(source);

            reclaimSource.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ReclaimSource_MakesSourceAvailableForReuse() {
            var instance = SourcePool.CreateInstance(1);
            var source = instance.GetSource();
            
            instance.ReclaimSource(source);

            instance.HasAvailableSource.Should().BeTrue();
            instance.GetSource().Should().Be(source);
        }

        [Fact]
        public void ReclaimSource_ShouldRewindSource() {
            var instance = SourcePool.CreateInstance(1);
            var source = instance.GetSource();
            source.Play();
            
            svcMock.Invocations.Clear();
            
            instance.ReclaimSource(source);
            
            svcMock.Verify(svc => svc.Rewind(source), Times.Once);
        }

        [Fact]
        public void ReclaimAllFinishedSources_ReclaimsOnlyFinishedSources() {
            var instance = SourcePool.CreateInstance(2);
            var source1 = instance.GetSource();
            var source2 = instance.GetSource();
            source1.Looping = true;
            
            instance.ReclaimAllFinishedSources();

            instance.HasAvailableSource.Should().BeTrue();
            instance.GetSource().Should().Be(source2);
            instance.HasAvailableSource.Should().BeFalse();
        }
        
        [Fact]
        public void ReclaimAllFinishedSources_ThrowsIfAnySourceDisposed() {
            var instance = SourcePool.CreateInstance(2);
            var source1 = instance.GetSource();
            instance.GetSource();
            source1.Dispose();
            
            Action reclaimAllFinishedSources = () => instance.ReclaimAllFinishedSources();

            reclaimAllFinishedSources.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ReclaimAllFinishedSources_RewindsReclaimedSource() {
            var instance = SourcePool.CreateInstance(2);
            var source1 = instance.GetSource();
            var source2 = instance.GetSource();
            source1.Looping = true;
            
            svcMock.Invocations.Clear();
            
            instance.ReclaimAllFinishedSources();
            
            svcMock.Verify(svc => svc.Rewind(source2), Times.Once);
        }

        [Fact]
        public void Dispose_DisposesAllSources() {
            var instance = SourcePool.CreateInstance(1);
            var source = instance.GetSource();
            
            instance.Dispose();

            source.Disposed.Should().BeTrue();
        }
    }
}
