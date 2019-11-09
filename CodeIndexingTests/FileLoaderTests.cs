using CodeIndexing.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace CodeIndexingTests
{
    public class FileLoaderTests
    {
        private string testString = 
            @"using System;
            using System.Text;

            namespace MyTest
            {
                public class MyTestClass
                {
                    public void TestMethod()
                    {
                        if(true)
                        {
                            a = 1;
                        }
                    }
                }
            }
            ";

        private FileLoader _loader = new FileLoader("");

        [Fact]
        public async void happy_path()
        {
            using (var stream = new MemoryStream())
            {
                var streamWriter = new StreamWriter(stream);
                streamWriter.Write(testString);
                streamWriter.Flush();
                stream.Position = 0;
                var result = await _loader.ParseFile(stream);
            }
        }
    }
}
