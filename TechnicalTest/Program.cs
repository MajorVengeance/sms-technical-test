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
            var message = "abc def ghi jkl mno pqrs tuv wxyz ABC DEF GHI JKL MNO PQRS TUV WXYZ !\"§ $% & / () =? *'<> #|; {} abc def ghi jkl mno pqrs tuv wxyz ABC DEF GHI JKL MNO PQRS TUV WXYZ !\"§ $%& /() =?* ' <> #";
            var messagePartGenerator = new MessagePartGenerator();
            var messageParts = messagePartGenerator.GetMessageParts(message);

            Console.WriteLine("Full message:");
            Console.WriteLine(message);
            Console.WriteLine("-----------");
            Console.WriteLine($"We have {messageParts.Count} parts");
            Console.WriteLine($"Total character size: {messageParts.Sum(m => m.Characters)}");
            Console.WriteLine("=========");
            foreach(var part in messageParts)
            {
                Console.WriteLine($"Part: {part.Part}");
                Console.WriteLine($"message: {part.Message}");
                Console.WriteLine($"characters used: {part.Characters}");
                Console.WriteLine("----------");
            }
        }   
    }
}
