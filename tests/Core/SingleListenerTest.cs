using Moq;
using System;
using Xunit;

namespace TomRijnbeek.Audio.Tests {
    public class SingleListenerTest {
        private readonly Mock<ListenerService> svcMock;

        public SingleListenerTest() {
            AudioContext.InitializeForTest();
            this.svcMock = new Mock<ListenerService>();
            ListenerService.SetTestInstance(this.svcMock.Object);
        }

        [Fact]
        public void RegistersListener() {
            var listener = new SingleListener();
            Assert.Equal(ALListener.Get(), listener);
        }

        [Fact]
        public void ForbidsMultipleListeners() {
            ALListener.Set(new Mock<IListener>().Object);
            Assert.Throws<InvalidOperationException>(() => new SingleListener());
        }
    }
}
