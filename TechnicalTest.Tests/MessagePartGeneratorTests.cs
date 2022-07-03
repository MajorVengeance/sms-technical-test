using System;
using System.Linq;
using Xunit;

namespace TechnicalTest.Tests
{
    public class MessagePartGeneratorTests
    {
        public MessagePartGeneratorTests()
        {
            _messagePartGenerator = new MessagePartGenerator();
        }

        private readonly MessagePartGenerator _messagePartGenerator;

        /// <summary>
        /// TEST: The max number of(theoretical) parts is 255 
        /// 
        /// Test to see that if we have more than 255 message parts, we throw an out of range exception, as we have a cap of 255.
        /// </summary>
        [Fact]
        public void Given_MessageRequiresMoreThan255Parts_When_GetMessageParts_Then_IndexOutOfRangeException_Thrown()
        {
            var message = System.IO.File.ReadAllText("OversizedFile.txt");
            var exception = Assert.Throws<IndexOutOfRangeException>(() => _messagePartGenerator.GetMessageParts(message));
            Assert.Equal("Too many message parts required", exception.Message);
        }

        /// <summary>
        /// TEST: Only GSM characters allowed
        /// 
        /// We supply a file with both GSM characters, and non GSM characters (just made a sentence in google translate in Japanese - seems to do the trick!)
        /// This should then cause an exception, as we can't compare the non-GSM characters to the value dictionary
        /// </summary>
        [Fact]
        public void Given_NonGSMCharacters_When_GetMessageParts_Then_Exception_Thrown()
        {
            var message = System.IO.File.ReadAllText("NonGSMCharacterFile.txt");
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _messagePartGenerator.GetMessageParts(message));
            Assert.Equal("message", exception.ParamName);
            Assert.StartsWith("Non GSM character detected in file.", exception.Message);
        }

        /// <summary>
        /// TEST: If message content is between 0-160 characters, then 1 part is required.
        /// </summary>
        [Fact]
        public void Given_EmptyMessage_When_GetMessageParts_Then_OneMessagePart_Returned()
        {
            var message = string.Empty;
            var act = _messagePartGenerator.GetMessageParts(message);
            Assert.NotNull(act);
            Assert.Single(act);
            Assert.Equal(message, act.First().Message);
            Assert.Equal(0, act.First().Characters);
        }

        /// <summary>
        /// TEST: If message content is between 0-160 characters, then 1 part is required.
        /// 
        /// We have 100 characters with a value of 1 - which should give us a single message back, with 100 characters.
        /// </summary>
        [Fact]
        public void Given_100CharacterMessage_When_GetMessageParts_Then_OneMessagePart_Returned()
        {
            var message = System.IO.File.ReadAllText("100Characters.txt");
            var act = _messagePartGenerator.GetMessageParts(message);
            Assert.NotNull(act);
            Assert.Single(act);
            Assert.Equal(message, act.First().Message);
            Assert.Equal(100, act.First().Characters);
        }

        /// <summary>
        /// TEST: If message content is between 0-160 characters, then 1 part is required.
        /// 
        /// We have 160 characters with a value of 1 - which should give us a single message back, with 160 characters.
        /// This is the maximum value for a singular message.
        /// </summary>
        [Fact]
        public void Given_MessageIs160Characters_When_GetMessageParts_Then_OneMessagePart_Returned()
        {
            var message = System.IO.File.ReadAllText("160Characters.txt");
            var act = _messagePartGenerator.GetMessageParts(message);
            Assert.NotNull(act);
            Assert.Single(act);
            Assert.Equal(message, act.First().Message);
            Assert.Equal(160, act.First().Characters);
        }

        /// <summary>
        /// TEST: If message content is greater than 160 characters then each part is a maximum of 153 characters.
        /// TEST: Extended GSM characters are 2 bytes/characters: €[]|^\~{ }
        /// 
        /// As we have a special character (€) trailing in this file, the total value will be 161 characters, rather than 160.
        /// This should cause us to have two message parts instead of one, and each part should have a maximum of 153 characters
        /// instead of 160.
        /// </summary>
        [Fact]
        public void Given_MessageIs160Characters_With2ValueCharacterTrailing_When_GetMessageParts_Then_TwoMessageParts_Returned()
        {
            var message = System.IO.File.ReadAllText("160CharactersEuroTrailing.txt");
            var act = _messagePartGenerator.GetMessageParts(message);
            Assert.NotNull(act);
            Assert.Equal(2, act.Count);
            Assert.Equal(161, act.Sum(a => a.Characters));
            Assert.Equal(153, act.First().Characters);
            Assert.Equal(8, act.Last().Characters);
        }

        /// <summary>
        /// TEST: If message content is greater than 160 characters then each part is a maximum of 153 characters 
        /// TEST: The max number of(theoretical) parts is 255 
        /// 
        /// When given a file that is 39015 characters (153*255) - we get 255 Message parts back.
        /// Test to see whether the First and Last messages are 153 long (the max size for multipart)
        /// Also check to see we have 255 Message Parts, and that the characters add up to 39015
        /// </summary>
        [Fact]
        public void Given_MessageIsExactly255PartsLong_WhenGetMessagePartsThen_255MessageParts_Returned()
        {
            var message = System.IO.File.ReadAllText("MaxSizedFile.txt");
            var act = _messagePartGenerator.GetMessageParts(message);
            Assert.NotNull(act);
            Assert.Equal(255, act.Count);
            Assert.Equal(39015, act.Sum(a => a.Characters));
            Assert.Equal(153, act.First().Characters);
            Assert.Equal(153, act.Last().Characters);
        }
    }
}
