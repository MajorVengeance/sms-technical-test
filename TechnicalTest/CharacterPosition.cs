namespace TechnicalTest
{
    /// <summary>
    /// A POCO because I couldn't update the "Position" value of a Tuple
    /// </summary>
    public class CharacterPosition
    {
        /// <summary>
        /// What character this Position is for
        /// </summary>
        public char Character { get; set; }

        /// <summary>
        /// Where in the message is the character
        /// </summary>
        public int Position { get; set; }
    }
}
