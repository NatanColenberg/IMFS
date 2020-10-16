using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMFS;

namespace IMFS_Tests
{
    [TestClass]
    public class IMFS_Tests
    {
        [TestMethod]
        public void BasicExerciseTest()
        {
            IMFS.IMFS imfs = createFileSystem();

            string res = imfs.Print(PrintType.DOS);

            Assert.AreEqual(
                "root\\\r\n\tFile3\r\n\tFile4\r\n\tDirA\\\r\n\t\tFile1\r\n\t\tFile2\r\n\t\tDirC\\\r\n\tDirD\\\r\n\t\tFile5\r\n", 
                res);
        }

        [TestMethod]
        public void DeleteTest()
        {
            IMFS.IMFS imfs = createFileSystem();

            // Delete DirA
            imfs.delete(1);

            string res = imfs.Print(PrintType.DOS);

            Assert.AreEqual(
                "root\\\r\n\tFile3\r\n\tFile4\r\n\tDirD\\\r\n\t\tFile5\r\n",
                res);
        }

        [TestMethod]
        public void JsonPrintTest()
        {
            IMFS.IMFS imfs = createFileSystem();

            string res = imfs.Print(PrintType.JSON);

            Assert.AreEqual(
                "\"root\":[\"files\":[\"File3\",\"File4\"],\"DirA\":[\"files\":[\"File1\",\"File2\"],\"DirC\":[]],\"DirD\":[\"files\":[\"File5\"]]",
                res);
        }

        private IMFS.IMFS createFileSystem()
        {
            IMFS.IMFS imfs = new IMFS.IMFS();

            imfs.addDir(1, "DirA", 0);
            imfs.addFile(11, "File1", Encoding.ASCII.GetBytes("File 1 Data"), 1);
            imfs.addFile(12, "File2", Encoding.ASCII.GetBytes("File 2 Data"), 1);
            imfs.addDir(13, "DirC", 1);

            imfs.addFile(2, "File3", Encoding.ASCII.GetBytes("File 3 Data"), 0);
            imfs.addFile(3, "File4", Encoding.ASCII.GetBytes("File 4 Data"), 0);

            imfs.addDir(4, "DirD", 0);
            imfs.addFile(41, "File5", Encoding.ASCII.GetBytes("File 5 Data"), 4);

            return imfs;
        }
    }
}
