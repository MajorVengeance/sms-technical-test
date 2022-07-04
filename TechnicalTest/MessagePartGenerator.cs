using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalTest
{
    /// <summary>
    /// A Logic class to split up a given message into separate SMS Message parts
    /// </summary>
    public class MessagePartGenerator
    {
        /// <summary>
        /// Dictionary of characters, and their value
        /// </summary>
        private readonly Dictionary<char, int> _gsmCharacters = new Dictionary<char, int>
        {
            { '@', 1 }, {'£', 1 }, { '$', 1 }, { '¥', 1 }, { 'è', 1 }, { 'é', 1 }, { 'ù', 1 },
            { 'ì', 1 }, { 'ò', 1 }, { 'Ç', 1 }, { 'Ø', 1 }, { 'ø', 1 }, { 'Å', 1 }, { 'å', 1 },
            { 'Δ', 1 }, { '_', 1 }, { 'Φ', 1 }, { 'Γ', 1 }, { 'Λ', 1 }, { 'Ω', 1 }, { 'Π', 1 },
            { 'Ψ', 1 }, { 'Σ', 1 }, { 'Θ', 1 }, { 'Ξ', 1 }, { 'Æ', 1 }, { 'æ', 1 }, { 'ß', 1 },
            { 'É', 1 }, { '!', 1 }, { '"', 1 }, { '#', 1 }, { '¤', 1 }, { '%', 1 }, { '&', 1 },
            { '\'', 1 }, { '(', 1 }, { ')', 1 }, { '*', 1 }, { '+', 1 }, { ',', 1 }, { '-', 1 },
            { '.', 1 }, { '/', 1 }, { '0', 1 }, { '1', 1 }, { '2', 1 }, { '3', 1 }, { '4', 1 },
            { '5', 1 }, { '6', 1 }, { '7', 1 }, { '8', 1 }, { '9', 1 }, { ':', 1 }, { ';', 1 },
            { '<', 1 }, { '=', 1 }, { '>', 1 }, { '?', 1 }, { '¡', 1 }, { 'A', 1 }, { 'B', 1 },
            { 'C', 1 }, { 'D', 1 }, { 'E', 1 }, { 'F', 1 }, { 'G', 1 }, { 'H', 1 }, { 'I', 1 },
            { 'J', 1 }, { 'K', 1 }, { 'L', 1 }, { 'M', 1 }, { 'N', 1 }, { 'O', 1 }, { 'P', 1 },
            { 'Q', 1 }, { 'R', 1 }, { 'S', 1 }, { 'T', 1 }, { 'U', 1 }, { 'V', 1 }, { 'W', 1 },
            { 'X', 1 }, { 'Y', 1 }, { 'Z', 1 }, { 'Ä', 1 }, { 'Ö', 1 }, { 'Ñ', 1 }, { 'Ü', 1 },
            { '§', 1 }, { '¿', 1 }, { 'a', 1 }, { 'b', 1 }, { 'c', 1 }, { 'd', 1 }, { 'e', 1 },
            { 'f', 1 }, { 'g', 1 }, { 'h', 1 }, { 'i', 1 }, { 'j', 1 }, { 'k', 1 }, { 'l', 1 },
            { 'm', 1 }, { 'n', 1 }, { 'o', 1 }, { 'p', 1 }, { 'q', 1 }, { 'r', 1 }, { 's', 1 },
            { 't', 1 }, { 'u', 1 }, { 'v', 1 }, { 'w', 1 }, { 'x', 1 }, { 'y', 1 }, { 'z', 1 },
            { 'ä', 1 }, { 'ö', 1 }, { 'ñ', 1 }, { 'ü', 1 }, { 'à', 1 }, { '\r', 1 }, { '\n', 1 },
            { ' ', 1 }, {'€', 2 }, { '[', 2 }, { ']', 2 }, { '|', 2 }, { '^', 2 }, { '\\', 2 },
            { '~', 2 }, { '{', 2 }, { '}', 2 }
        };

        /// <summary>
        /// Parses a message into separate Message Parts.
        /// </summary>
        /// <param name="message">The raw message to be parsed</param>
        /// <returns>A List of <see cref="MessagePart"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If non GSM characters are passed in</exception>
        /// <exception cref="IndexOutOfRangeException">If the message is split into more than 255 parts</exception>
        public List<MessagePart> GetMessageParts(string message)
        {
            // If we have no message, just pass back a single empty message part
            if (string.IsNullOrEmpty(message))
                return new List<MessagePart> { new MessagePart { Characters = 0, Message = string.Empty, Part = 1 } };

            var messageParts = new List<MessagePart>();
            var characterPosition = new List<CharacterPosition>() { };
            foreach (var character in message)
            {
                // Confirm each character is actually a GSM character, throw exception if not.
                if (!_gsmCharacters.ContainsKey(character))
                    throw new ArgumentOutOfRangeException(nameof(message), $"Non GSM character detected in message. Character: '{character}'");

                // Add the character to our buffer -
                // after working out the new position based on the previous character's postion, and our current character's value
                characterPosition.Add(new CharacterPosition { 
                    Character = character, 
                    Position = (characterPosition.LastOrDefault()?.Position ?? 0) + _gsmCharacters[character] 
                });
            }

            // Quickly check whether we're using a multipart message or not - this affects how large our parts can be
            var isMultiPart = characterPosition.Last().Position > 160;

            // Use a while loop as we'll be subtracting from the buffer with each part
            while (characterPosition.Count > 0)
            {
                // 255 is our max allowed parts, so if we hit this statement, we're at part 256
                if (messageParts.Count == 255)
                    throw new IndexOutOfRangeException("Too many message parts required");

                // Get the next part's worth of characters out the buffer (either 160 for single part, or 153 for multipart)
                var splitCharacters = characterPosition.Where(c => c.Position <= (isMultiPart ? 153 : 160));
                // Load the characters into an array, and create a new string out of it.
                var msg = new string(splitCharacters.Select(x => x.Character).ToArray());
                
                //Need to take stock of what our last position is - if we have a special character at position "153" it won't be added
                //due to technically being 154 - so we only want to remove 152 from position tally then - else we'd end up with a 
                //special character being worth 1 rather than 2.
                var charactersToLoad = splitCharacters.Last().Position;

                messageParts.Add(new MessagePart { Message = msg, Characters = charactersToLoad, Part = messageParts.Count + 1 });
                
                // Remove all the characters that were just added using the same predicate as the select
                characterPosition.RemoveAll(c => c.Position <= (isMultiPart ? 153 : 160));
                
                // If this is a multi part message, we're going to need to subtract the buffer down, so remove
                // the amount of characters loaded in from the remaining characters
                if (isMultiPart)
                    characterPosition.ForEach(c => c.Position -= charactersToLoad);
            }

            return messageParts;
        }
    }
}
