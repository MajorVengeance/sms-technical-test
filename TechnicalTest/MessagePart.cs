namespace TechnicalTest
{
    /// <summary>
    /// A SMS Message Part
    /// </summary>
    public class MessagePart
    {
        /// <summary>
        /// The message this part holds
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// How many characters have been used in this message part.
        /// For Single Part SMS this value should be a maximum of 160
        /// For Multi Part SMS this value should be a maximum of 153
        /// </summary>
        public int Characters { get; set; }  

        /// <summary>
        /// Which Number Part of the message this is.
        /// </summary>
        public int Part { get; set; }
    }
}
