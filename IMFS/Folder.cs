using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMFS
{
    public class Folder
    {
        public long ID { get; private set; }
        public string Name { get; private set; }
        public long PID { get; private set; }
        public bool IsDeleted { get; private set; } = false;

        Dictionary<long, File> files = new Dictionary<long, File> { };
        Dictionary<long, Folder> folders = new Dictionary<long, Folder> { };
        GarbageCollector gc;

        public Folder(long id, string name, long pid)
        {
            this.ID = id;
            this.Name = name;
            this.PID = pid;

            gc = new GarbageCollector(files, folders, 5000);
        }

        public void AddFile(File file)
        {
            if (files.ContainsKey(file.ID))
            {
                if (files[file.ID].IsDeleted)
                {
                    gc.RemoveFileForce(file);
                }
                else
                {
                    throw new Exception(
                        $"Folder already contains a file with the same ID (Folder = {Name}, File ID = {file.ID})");
                }
            }
            files.Add(file.ID, file);
        }

        public void AddFolder(Folder folder)
        {
            if (folders.ContainsKey(folder.ID))
            {
                if (folders[folder.ID].IsDeleted)
                {
                    gc.RemoveFolderForce(folder);
                }
                else
                {
                    throw new Exception(
                        $"Folder already contains a Sub-Folder with the same ID (Folder = {Name}, Sub-Folder ID = {folder.ID})");
                }
            }
            folders.Add(folder.ID, folder);
        }

        public List<File> GetFiles()
        {
            return files.Values.Where(file => file.IsDeleted == false).ToList();
        }

        public List<Folder> GetFolders()
        {
            return folders.Values.Where(folder => folder.IsDeleted == false).ToList();
        }

        public void Delete()
        {
            IsDeleted = true;
        }

        public void RemoveFile(long fileID)
        {
            if (files.ContainsKey(fileID))
            {
                files[fileID].Delete();
            }
            else
            {
                throw new Exception($"Cant find file with ID = {fileID}");
            }
        }

        public void RemoveFolder(long folderID)
        {
            if (folders.ContainsKey(folderID))
            {
                folders[folderID].Delete();
            }
            else
            {
                throw new Exception($"Cant find folder with ID = {folderID}");
            }
        }
    }
}
