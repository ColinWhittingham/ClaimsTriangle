using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsTriangle
{
    public static class Helper
    {
        public static string GetDefaultDirectory()
        {
            string dirName = AppDomain.CurrentDomain.BaseDirectory;
            FileInfo fileInfo = new FileInfo(dirName);
            DirectoryInfo parent2Dir = fileInfo.Directory.Parent.Parent;
            return parent2Dir.FullName;
        }
    }
}
