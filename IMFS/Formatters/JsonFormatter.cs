using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMFS
{
    class JsonFormatter : IFormatterStrategy
    {
        public string Format(Folder folder)
        {
            StringBuilder res = new StringBuilder();
            GetContent(folder, 0, res);
            return res.ToString();
        }

        private void GetContent(Folder folder, uint level, StringBuilder res)
        {
            // Print Folder Name
            if (level > 0) res.Append(",");
            res.Append($"\"{folder.Name}\":[");

            // Print Files
            List<File> filesInFolder = folder.GetFiles();
            if(filesInFolder.Count > 0)
            {
                res.Append("\"files\":[");
                string files;
                if(filesInFolder.Count == 1)
                {
                    files = $"\"{filesInFolder.First().Name}\"";
                }
                else
                {
                    files = filesInFolder.Select(file => file.Name).Aggregate((i, j) => { return $"\"{i}\"" + "," + $"\"{j}\""; });
                }
                res.Append($"{files}]");
            }

            // Print Sub-Folders
            List<Folder> subFolders = folder.GetFolders();
            subFolders.ForEach(subFolder =>
            {
                GetContent(subFolder, level + 1, res);
                res.Append("]");
            });
        }
    }
}
