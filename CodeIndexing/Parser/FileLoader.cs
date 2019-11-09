using CodeIndexing.Dto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeIndexing.Parser
{
    public class FileLoader
    {
        public string ProjectRoot { get; set; }

        public FileLoader(string projectRoot)
        {
            if (Directory.Exists(projectRoot))
            {
                ProjectRoot = projectRoot;
            }
        }

        public async Task WalkProject()
        {
            var entries = Directory.GetFiles(ProjectRoot, "*.cs", SearchOption.AllDirectories);
            ConcurrentQueue<string> fileNames = new ConcurrentQueue<string>(entries);
            
        }

        public async Task<bool> ParseFile(Stream file, string filePath)
        {
            var level = ParseLevel.Root;
            using(var reader = new StreamReader(file))
            {
                string currentLine;
                string currentNamespace = null;
                List<ClassDto> classes = new List<ClassDto>();
                int bracketCount = 0;
                List<string> storedLines = new List<string>();
                while (!reader.EndOfStream)
                {
                    currentLine = await reader.ReadLineAsync();
                    switch (level)
                    {
                        case ParseLevel.Root:
                            if (currentLine.Contains("namespace"))
                            {
                                var lineWords = currentLine.Split(' ');
                                currentNamespace = lineWords[1];
                            }
                            if (currentLine.Contains("{"))
                            {
                                level = ParseLevel.Namespace;
                            }
                            break;
                        case ParseLevel.Namespace:
                            if (currentLine.Contains("class"))
                            {
                                var lineWords = currentLine.Split(' ');
                                foreach(var word in lineWords)
                                {
                                    if(!Keywords.signatureKeywords.Any(x => x == word) && word != "class")
                                    {
                                        classes.Add(new ClassDto
                                        {
                                            ClassName = word,
                                            FilePathAndName = filePath,
                                            Namespace = currentNamespace,
                                            UnderlyingType = TypeCode.Object
                                        });
                                    }
                                }
                            }
                            if (currentLine.Contains("{"))
                            {
                                level = ParseLevel.Class;
                            }
                            if (currentLine.Contains("}")){
                                level = ParseLevel.Root;
                                currentNamespace = null;
                            }
                            break;
                        case ParseLevel.Class:
                            if (currentLine.Contains("}"))
                            {
                                level = ParseLevel.Namespace;
                                storedLines.Clear();
                            }
                            else if (currentLine.Contains("{"))
                            {
                                level = ParseLevel.Method;
                                storedLines.Clear();
                            }
                            else
                            {
                                storedLines.Add(currentLine);
                            }

                            if (currentLine.Contains(')'))
                            {

                            }
                            break;
                        case ParseLevel.Method:
                            if (currentLine.Contains("{"))
                            {
                                bracketCount++;
                            }
                            if (currentLine.Contains("}"))
                            {
                                if(bracketCount == 0)
                                {
                                    level = ParseLevel.Class;
                                }
                                else
                                {
                                    bracketCount--;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return false;
        }
    }
}
