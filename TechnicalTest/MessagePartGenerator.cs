using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalTest
{
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
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public List<MessagePart> GetMessageParts(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Message cannot be an empty string", nameof(message));
            }
            var messageParts = new List<MessagePart>();
            var characterPosition = new List<CharacterPosition>() { };
            foreach (var character in message)
            {
                characterPosition.Add(new CharacterPosition { Character = character, Position = (characterPosition.LastOrDefault()?.Position ?? 0) + _gsmCharacters[character] });
            }

            var multiPart = characterPosition.Last().Position > 160;
            while (characterPosition.Count > 0)
            {
                if (messageParts.Count == 255)
                    throw new IndexOutOfRangeException("Too many message parts required");
                var splitCharacters = characterPosition.Where(c => c.Position <= (multiPart ? 153 : 160));
                var msg = new string(splitCharacters.Select(x => x.Character).ToArray());
                //Need to take stock of what our last position is - if we have a special character at position "153" it won't be added
                //due to technically being 154 - so we only want to remove 152 from position tally then - else we'd end up with a 
                //special character being worth 1 rather than 2.
                var charactersToLoad = splitCharacters.Last().Position;
                messageParts.Add(new MessagePart { Message = msg, Characters = charactersToLoad, Part = messageParts.Count + 1 });
                characterPosition.RemoveAll(c => c.Position <= (multiPart ? 153 : 160));
                if (multiPart)
                    characterPosition.ForEach(c => c.Position -= charactersToLoad);
            }

            return messageParts;
        }
    }
}
