using System;
using Xunit;

namespace TechnicalTest.Tests
{
    public class MessagePartGeneratorTests
    {
        public MessagePartGeneratorTests()
        {
            _messagePartGenerator = new MessagePartGenerator();
        }

        private MessagePartGenerator _messagePartGenerator;

        [Fact]
        public void GivenEmptyMessage_When_GetMessageParts_Then_ArgumentExceptionThrown()
        {
            var message = string.Empty;
            Assert.Throws<ArgumentException>(() => _messagePartGenerator.GetMessageParts(message));
        }
    }
}
