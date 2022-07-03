using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TechnicalTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var message = string.Empty;
            if (args[0] == "file" || args[0] == "--file" || args[0] == "\\file")
            {
                var fileName = args[1];
                message = File.ReadAllText(fileName);
            }
            else if (args[0] == "text" || args[0] == "--text" || args[0] == "\\text")
            {
                Console.WriteLine("Please enter the message to send: ");
                message = Console.ReadLine();
            }
            else
            {
                message = "abc def ghi jkl mno pqrs tuv wxyz ABC DEF GHI JKL MNO PQRS TUV WXYZ !\"§ $% & / () =? *'<> #|; {} abc def ghi jkl mno pqrs tuv wxyz ABC DEF GHI JKL MNO PQRS TUV WXYZ !\"§ $%& /() =?* ' <> #";
            }
            var messagePartGenerator = new MessagePartGenerator();
            try
            {
                var messageParts = messagePartGenerator.GetMessageParts(message);

                Console.WriteLine("Full message:");
                Console.WriteLine(message);
                Console.WriteLine("-----------");
                Console.WriteLine($"We have {messageParts.Count} parts");
                Console.WriteLine($"Total character size: {messageParts.Sum(m => m.Characters)}");
                Console.WriteLine("=========");
                foreach (var part in messageParts)
                {
                    Console.WriteLine($"Part: {part.Part}");
                    Console.WriteLine($"message: {part.Message}");
                    Console.WriteLine($"characters used: {part.Characters}");
                    Console.WriteLine("----------");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
