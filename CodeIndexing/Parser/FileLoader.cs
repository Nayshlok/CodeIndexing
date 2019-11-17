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

        public void WalkProject()
        {
            var entries = Directory.GetFiles(ProjectRoot, "*.cs", SearchOption.AllDirectories);
            ConcurrentQueue<string> fileNames = new ConcurrentQueue<string>(entries);
        }
    }
}
