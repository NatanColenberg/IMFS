using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMFS
{
    class DosFormatter : IFormatterStrategy
    {
        private string value;
        public string Format(Folder folder)
        {
            StringBuilder res = new StringBuilder();
            GetFolderContent(folder, 0, res);
            return res.ToString();
        }

        private void GetFolderContent(Folder folder, uint level, StringBuilder res)
        {
            // Set Indentation
            string indentation = "";
            for (int i = 0; i < level; i++)
                indentation += "\t";

            // Print Folder Name
            res.AppendLine(indentation + folder.Name + "\\");

            // Print Files
            List<File> filesInFolder = folder.GetFiles();
            filesInFolder.ForEach(file =>
            {
                res.AppendLine(indentation + "\t" + file.Name);
            });

            // Print Sub-Folders
            List<Folder> subFolders = folder.GetFolders();
            subFolders.ForEach(subFolder =>
            {
                GetFolderContent(subFolder, level + 1, res);
            });
        }
    }
}
