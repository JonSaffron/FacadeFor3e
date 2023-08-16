using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FacadeFor3e;

namespace AddAttachment
    {
    public static class Program
        {
        public static void Main(string[] args)
            {
            if (args.Length == 0)
                {
                Console.WriteLine("Usage: AddAttachment <url of transaction service> ");
                return;
                }

            Queue<string> argList = new Queue<string>(args);
            if (!argList.Any())
                return;
            Uri endPoint = new Uri(argList.Dequeue());

            try
                {
                var transactionService = new TransactionServices(endPoint);
                transactionService.Ping();
                Console.WriteLine("Transaction service is responding. Ready to attach a file.");

                Console.Write("ArchetypeID? ");
                string archetypeId = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(archetypeId))
                    {
                    Console.WriteLine("Aborted");
                    return;
                    }

                Console.Write("ItemID? ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    {
                    Console.WriteLine("Aborted");
                    return;
                    }
                Guid itemId = Guid.Parse(input);

                Console.Write("File? ");
                string file = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(file))
                    {
                    Console.WriteLine("Aborted");
                    return;
                    }

                using (var fileContent = new FileStream(file, FileMode.Open))
                    {
                    var fileTitle = Path.GetFileName(file);
                    transactionService.SendAttachment.AttachNewFile(archetypeId, itemId, fileTitle, fileContent);
                    }
                }
            catch (Exception ex)
                {
                Console.WriteLine(ex.ToString());
                }
            }
        }
    }
