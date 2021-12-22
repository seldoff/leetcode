using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Leetcode
{
    public class SimplifyPathSolution
    {
        public string SimplifyPath(string path)
        {
            var result = new List<string>();
            var len = path.Length;
            while (true)
            {
                path = path.Replace("//", "/");
                if (path.Length == len)
                {
                    break;
                }
                len = path.Length;
            }

            var dirs = path.Split('/');
            foreach (var dir in dirs)
            {
                if (dir == "") continue;

                switch (dir)
                {
                    case ".": continue;
                    case "..":
                    {
                        if (result.Count > 0)
                        {
                            result.RemoveAt(result.Count - 1);
                        }
                        break;
                    }
                    default:
                    {
                        result.Add(dir);
                        break;
                    }
                }
            }

            return '/' + string.Join('/', result);
        }

        public string SimplifyPathV2(string path)
        {
            var dirs = new List<string>();

            var pos = 0;
            while (pos < path.Length)
            {
                ReadDelimiter();
                var dir = ReadDirectory();
                switch (dir)
                {
                    case "":
                    case ".": continue;
                    case "..":
                    {
                        if (dirs.Count > 0)
                        {
                            dirs.RemoveAt(dirs.Count - 1);
                        }
                        break;
                    }
                    default:
                    {
                        dirs.Add(dir);
                        break;
                    }
                }
            }

            void ReadDelimiter()
            {
                while (pos < path.Length && path[pos] == '/')
                {
                    pos++;
                }
            }

            string ReadDirectory()
            {
                var start = pos;
                while (pos < path.Length && path[pos] != '/')
                {
                    pos++;
                }

                return path.Substring(start, pos - start);
            }

            if (dirs.Count == 0)
            {
                return "/";
            }
            var result = new StringBuilder(path.Length);
            foreach (var dir in dirs)
            {
                result.Append('/');
                result.Append(dir);
            }

            return result.ToString();
        }
    }

    [TestFixture]
    public class SimplifyPathTest
    {
        [Test]
        public void Test()
        {
            var s = new SimplifyPathSolution();
            Assert.AreEqual("/", s.SimplifyPath("/"));
            Assert.AreEqual("/", s.SimplifyPath("//"));
            Assert.AreEqual("/", s.SimplifyPath("///"));
            Assert.AreEqual("/", s.SimplifyPath("/."));
            Assert.AreEqual("/", s.SimplifyPath("/./"));
            Assert.AreEqual("/", s.SimplifyPath("/.."));
            Assert.AreEqual("/", s.SimplifyPath("/../"));
            Assert.AreEqual("/a/b/c", s.SimplifyPath("/a/b/c"));
            Assert.AreEqual("/a/b/c", s.SimplifyPath("/a/b/c/"));
            Assert.AreEqual("/a/b/c", s.SimplifyPath("/a/b/./c"));
            Assert.AreEqual("/a/c", s.SimplifyPath("/a/b/../c"));
            
            Assert.AreEqual("/", s.SimplifyPathV2("/"));
            Assert.AreEqual("/", s.SimplifyPathV2("//"));
            Assert.AreEqual("/", s.SimplifyPathV2("///"));
            Assert.AreEqual("/", s.SimplifyPathV2("/."));
            Assert.AreEqual("/", s.SimplifyPathV2("/./"));
            Assert.AreEqual("/", s.SimplifyPathV2("/.."));
            Assert.AreEqual("/", s.SimplifyPathV2("/../"));
            Assert.AreEqual("/a/b/c", s.SimplifyPathV2("/a/b/c"));
            Assert.AreEqual("/a/b/c", s.SimplifyPathV2("/a/b/c/"));
            Assert.AreEqual("/a/b/c", s.SimplifyPathV2("/a/b/./c"));
            Assert.AreEqual("/a/c", s.SimplifyPathV2("/a/b/../c"));
        }
    }
}
