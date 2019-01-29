using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.IO;

namespace ParseEnum
{
    class Program
    {
        private static string[] GetFileNames(string path, string filter)
        {
            string[] files = Directory.GetFiles(path, filter);
            for (int i = 0; i < files.Length; i++)
                files[i] = Path.GetFileName(files[i]);
            return files;
        }

        static void Main(string[] args)
        {
            try
            {
                var path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Common\Consts");
                var fileNames = GetFileNames(path, "*.cs");

                foreach (var fileName in fileNames)
                {
                    var filePath = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Common\Consts\" + fileName);
                    string content = File.ReadAllText(filePath);

                    AntlrInputStream inputStream = new AntlrInputStream(content);
                    EnumLexer lexer = new EnumLexer(inputStream);
                    CommonTokenStream tokens = new CommonTokenStream(lexer);
                    EnumParser parser = new EnumParser(tokens);
                    IParseTree tree = parser.prog(); // parse

                    ParseTreeWalker walker = new ParseTreeWalker(); // create standard walker
                    EnumListener extractor = new EnumListener(parser);
                    walker.Walk(extractor, tree); // initiate walk of tree with listener

                    string pathToFile = Environment.CurrentDirectory + "\\" + extractor.name + ".js";

                    using (FileStream fs = new FileStream(pathToFile, FileMode.OpenOrCreate))
                    using (StreamWriter file = new StreamWriter(fs))
                    {
                        file.Write(extractor.builder.ToString());
                        file.Close();
                        fs.Close();
                    }
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                Console.ReadKey();
            }
        }
    }
}
