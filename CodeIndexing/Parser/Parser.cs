using CodeIndexing.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CodeIndexing.Parser
{
    public class Parser
    {
        public async Task<bool> ParseFile(Stream file, string filePath)
        {
            var level = ParseLevel.Root;
            string currentLine;
            string currentNamespace = null;
            ClassDto currentClass = null;
            MethodDto currentMethod;
            List<ClassDto> classes = new List<ClassDto>();
            int bracketCount = 0;
            var maxDepth = 0;
            List<string> storedLines = new List<string>();
            using (var reader = new StreamReader(file))
            {
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
                                foreach (var word in lineWords)
                                {
                                    if (!Keywords.signatureKeywords.Any(x => x == word) && word != "class")
                                    {
                                        currentClass = new ClassDto
                                        {
                                            ClassName = word,
                                            FilePathAndName = filePath,
                                            Namespace = currentNamespace,
                                            UnderlyingType = TypeCode.Object
                                        };
                                        classes.Add(currentClass);
                                    }
                                }
                            }
                            if (currentLine.Contains("{"))
                            {
                                level = ParseLevel.Class;
                            }
                            if (currentLine.Contains("}"))
                            {
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
                                var joinedString = string.Join(' ', storedLines);
                                currentMethod = new MethodDto
                                {
                                    Namespace = currentNamespace,
                                    BelongsToClass = currentClass,
                                    FilePathAndName = filePath
                                };
                                ProcessMethodString(joinedString, currentMethod);
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
                                if(bracketCount > maxDepth)
                                {
                                    maxDepth = bracketCount;
                                }
                            }
                            if (currentLine.Contains("}"))
                            {
                                if (bracketCount == 0)
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

        public void ProcessMethodString(string joinedString, MethodDto currentMethod)
        {
            var searchState = MethodSearchState.Begin;
            ParameterDto currentParameter = null;
            var wordBuilder = new Stack<char>();

            for (var i = joinedString.Length - 1; i >= 0; i--)
            {
                switch (searchState)
                {
                    case MethodSearchState.Begin:
                        if (joinedString[i] == ')')
                        {
                            searchState = MethodSearchState.ClosingParen;
                        }
                        break;
                    case MethodSearchState.ClosingParen:
                        if (char.IsWhiteSpace(joinedString[i]) || joinedString[i] == '(')
                        {
                            if (wordBuilder.Any())
                            {
                                var word = new string(wordBuilder.ToArray());
                                wordBuilder.Clear();
                                if (currentParameter == null)
                                {
                                    currentParameter = new ParameterDto
                                    {
                                        ParameterName = word,
                                        BelongsToMethod = currentMethod
                                    };
                                }
                                else
                                {
                                    currentParameter.ParamterType = word;
                                }
                            }
                            if (joinedString[i] == '(')
                            {
                                searchState = MethodSearchState.OpenParen;
                                if(currentParameter != null)
                                {
                                    currentMethod.Parameters.Add(currentParameter);
                                    currentParameter = null;
                                }
                            }
                        }
                        else if (joinedString[i] == ',')
                        {
                            currentMethod.Parameters.Add(currentParameter);
                            currentParameter = null;
                        }
                        else
                        {
                            wordBuilder.Push(joinedString[i]);
                        }
                        break;
                    case MethodSearchState.OpenParen:
                        if (char.IsWhiteSpace(joinedString[i]))
                        {
                            if (wordBuilder.Any())
                            {
                                var word = new string(wordBuilder.ToArray());
                                wordBuilder.Clear();
                                currentMethod.MethodName = word;
                                searchState = MethodSearchState.Name;
                            }
                        }
                        else
                        {
                            wordBuilder.Push(joinedString[i]);
                        }
                        break;
                    case MethodSearchState.Name:
                        if (char.IsWhiteSpace(joinedString[i]))
                        {
                            if (wordBuilder.Any())
                            {
                                var word = new string(wordBuilder.ToArray());
                                wordBuilder.Clear();
                                currentMethod.ReturnType = word;
                                searchState = MethodSearchState.Type;
                            }
                        }
                        else
                        {
                            wordBuilder.Push(joinedString[i]);
                        }
                        break;
                    case MethodSearchState.Type:
                        return;
                    default:
                        break;
                }
            }
        }
    }
}
