using IMFS.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMFS
{
    public enum PrintType
    {
        DOS,
        JSON,
        XML,
        HTML
    }

    public class IMFS
    {
        private Dictionary<long, File> files = new Dictionary<long, File>();
        private Dictionary<long, Folder> folders = new Dictionary<long, Folder>();
        private GarbageCollector gc;


        public IMFS()
        {
            // Init Garbage Collector and Run every 5sec
            gc = new GarbageCollector(files, folders, 5000);

            // Create root folder
            Folder root = new Folder(0, "root", -1);
            folders.Add(0, root);
        }

        public void addFile(long id, string name, byte[] data, long pid)
        {
            // *** validate input ***
            // Check that file ID does not exist
            if (files.ContainsKey(id))
            {
                // Check if the file ID that we are trying to add
                // exists in the files list as isDeleted, 
                // we must remove it by force before inserting another
                // file with the same ID.
                if (files[id].IsDeleted)
                {
                    gc.RemoveFileForce(files[id]);
                }
                else
                {
                    throw new Exception("File ID is already in use");
                }
            }
            // Check that folder ID exists
            if (!FolderExists(pid))
            {
                throw new Exception("Folder ID doesn't exists");
            }

            // Create a new file
            File file = new File(id, name, data, pid);
            files.Add(id, file);

            // Add file to folder
            Folder folder = GetFolderByID(pid);
            folder.AddFile(file);
        }

        public void addDir(long id, string name, long pid)
        {
            // *** validate input ***
            // Check that new folder ID doesn't exists
            if (folders.ContainsKey(id))
            {
                // Check if the folder ID that we are trying to add
                // exists in the folders list as isDeleted, 
                // we must remove it by force before inserting another
                // folder with the same ID.
                if (folders[id].IsDeleted)
                {
                    gc.RemoveFolderForce(folders[id]);
                }
                else
                {
                    throw new Exception($"Folder ID already in use (ID = {id})");
                }
            }
            // Check that parent folder ID exists
            if (!FolderExists(pid))
            {
                throw new Exception($"Parent Folder ID doesn't exists (PID = {id})");
            }

            // Add the new Folder
            Folder newFolder = new Folder(id, name, pid);
            Folder parentFolder = GetFolderByID(pid);
            parentFolder.AddFolder(newFolder);
            folders.Add(newFolder.ID, newFolder);
        }

        public void delete(long id)
        {
            // Check if this is a File
            if (FileExists(id))
            {
                File file = GetFileByID(id);

                if (file.IsDeleted == false)
                {
                    // Logically Delete File
                    gc.RemoveFile(file);
                }
            }

            // Check if this is a Folder
            else if (FolderExists(id))
            {
                // Get Folder
                Folder folderToRemove = GetFolderByID(id);
                List<Folder> subFolders = folderToRemove.GetFolders();
                subFolders.ForEach(folder => delete(folder.ID));

                // Remove Files from Folder
                List<File> filesInFolder = folderToRemove.GetFiles();
                filesInFolder.ForEach(file => delete(file.ID));

                // Logically Delete Folder
                gc.RemoveFolder(folderToRemove);
            }

            // Couldn't find matching ID
            else
            {
                throw new Exception($"Could not find ID (ID = {id})");
            }
        }

        public string Print(PrintType type)
        {
            // Get Root
            Folder root = GetFolderByID(0);

            // Select Formatter
            Formatter formatter = new Formatter();
            switch (type)
            {
                case PrintType.DOS:
                    formatter.SetStrategy(new DosFormatter());
                    break;
                case PrintType.JSON:
                    formatter.SetStrategy(new JsonFormatter());
                    break;
                default:
                    throw new NotImplementedException("Format method dot implemented");
            }

            // Format & Print
            return formatter.Print(root);
        }

        // *** Private Methods ***
        
        private bool FileExists(long id)
        {
            return files.ContainsKey(id) && files[id].IsDeleted == false;
        }
        private bool FolderExists(long id)
        {
            return folders.ContainsKey(id) && folders[id].IsDeleted == false;
        }
        private Folder GetFolderByID(long id)
        {
            Folder folder = folders[id];

            if (folder == null || folder.IsDeleted)
            {
                throw new Exception($"Failed to find Folder (Folder ID = {id})");
            }

            return folder;
        }
        private File GetFileByID(long id)
        {
            File file = files[id];

            if (file == null || file.IsDeleted)
            {
                throw new Exception($"Failed to find File (File ID = {id})");
            }

            return file;
        }

    }
}
