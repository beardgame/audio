using Moq;
using System;
using Xunit;

namespace TomRijnbeek.Audio.Tests {
    public class SingleListenerTest {
        private readonly Mock<ListenerService> svcMock;

        public SingleListenerTest() {
            AudioContext.InitializeForTest();
            svcMock = new Mock<ListenerService>();
            ListenerService.SetTestInstance(svcMock.Object);
        }

        [Fact]
        public void RegistersListener() {
            ListenerService.SetTestInstance(svcMock.Object);
            ALListener.Set(null);
            var listener = new SingleListener();
            Assert.Equal(ALListener.Get(), listener);
        }

        [Fact]
        public void ForbidsMultipleListeners() {
            ListenerService.SetTestInstance(svcMock.Object);
            ALListener.Set(new Mock<IListener>().Object);
            Assert.Throws<InvalidOperationException>(() => new SingleListener());
        }
    }
}
