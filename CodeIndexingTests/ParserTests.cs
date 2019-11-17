using CodeIndexing.Dto;
using CodeIndexing.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace CodeIndexingTests
{
    public class ParserTests
    {
        private string testString =
            @"using System;
            using System.Text;

            namespace MyTest
            {
                public class MyTestClass
                {
                    public void TestMethod(int a)
                    {
                        if(true)
                        {
                            a = 1;
                        }
                    }
                }
            }
            ";

        private Parser _parser = new Parser();

        [Fact]
        public async void happy_path()
        {
            using (var stream = new MemoryStream())
            {
                var streamWriter = new StreamWriter(stream);
                streamWriter.Write(testString);
                streamWriter.Flush();
                stream.Position = 0;
                var result = await _parser.ParseFile(stream, "");
            }
        }

        [Fact]
        public void can_parse_method_string()
        {
            var joinedString = "public void test(int a)";
            var methodDto = new MethodDto();
            _parser.ProcessMethodString(joinedString, methodDto);

            Assert.Equal("test", methodDto.MethodName);
            Assert.Collection(methodDto.Parameters, param =>
            {
                Assert.Equal("a", param.ParameterName);
                Assert.Equal("int", param.ParamterType);
            });
            Assert.Equal("void", methodDto.ReturnType);
        }

        [Fact]
        public void can_parse_method_string_with_extra_spaces()
        {
            var joinedString = "public    void     test             (    int     a   )  {";
            var methodDto = new MethodDto();
            _parser.ProcessMethodString(joinedString, methodDto);

            Assert.Equal("test", methodDto.MethodName);
            Assert.Collection(methodDto.Parameters, param =>
            {
                Assert.Equal("a", param.ParameterName);
                Assert.Equal("int", param.ParamterType);
            });
            Assert.Equal("void", methodDto.ReturnType);
        }

        [Fact]
        public void can_parse_method_string_with_many_parameters()
        {
            var joinedString = "public void test(int a, Object something, Car vehicle, double d, float f)";
            var methodDto = new MethodDto();
            _parser.ProcessMethodString(joinedString, methodDto);

            Assert.Equal("test", methodDto.MethodName);
            Assert.Collection(methodDto.Parameters,
            param =>
            {
                Assert.Equal("f", param.ParameterName);
                Assert.Equal("float", param.ParamterType);
            },
            param =>
            {
                Assert.Equal("d", param.ParameterName);
                Assert.Equal("double", param.ParamterType);
            },
            param =>
            {
                Assert.Equal("vehicle", param.ParameterName);
                Assert.Equal("Car", param.ParamterType);
            },
            param =>
            {
                Assert.Equal("something", param.ParameterName);
                Assert.Equal("Object", param.ParamterType);
            },
            param =>
            {
                Assert.Equal("a", param.ParameterName);
                Assert.Equal("int", param.ParamterType);
            });
            Assert.Equal("void", methodDto.ReturnType);
        }
    }
}
