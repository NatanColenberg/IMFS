using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMFS
{
    public class GarbageCollector
    {
        private Dictionary<long, File> files;
        private Dictionary<long, Folder> folders;
        private Queue<long> itemsToDelete = new Queue<long>();

        public GarbageCollector(Dictionary<long, File> files, Dictionary<long, Folder> folders, uint interval)
        {
            this.files = files;
            this.folders = folders;

            //Timer gcTimer = new Timer(new TimerCallback(RemoveItemsFromQueue), null, 0, interval);
        }

        public void RemoveFile(File file)
        {
            // Logically marked File as Deleted
            file.Delete();

            // Logically marked as Deleted in Parent
            if (folders.ContainsKey(file.PID))
            {
                Folder parent = folders[file.PID];
                parent.RemoveFile(file.ID);
            }

            // Add ID to itemsToDelete Queue
            itemsToDelete.Enqueue(file.ID);
        }

        public void RemoveFolder(Folder folder)
        {
            // Logically marked as Deleted
            folder.Delete();

            // Logically marked as Deleted in Parent
            if (folders.ContainsKey(folder.PID))
            {
                Folder parent = folders[folder.PID];
                parent.RemoveFolder(folder.ID);
            }

            // Add ID to itemsToDelete Queue
            itemsToDelete.Enqueue(folder.ID);
        }

        public void RemoveFileForce(File file)
        {
            // Logically marked as Deleted
            file.Delete();

            // Remove File
            if (files.ContainsKey(file.ID))
            {
                files.Remove(file.ID);
            }
        }

        public void RemoveFolderForce(Folder folder)
        {
            // Logically marked as Deleted
            folder.Delete();

            // Remove Folder
            if (folders.ContainsKey(folder.ID))
            {
                folders.Remove(folder.ID);
            }
        }

        private void RemoveItemsFromQueue(object o)
        {
            while(itemsToDelete.Count > 0)
            {
                long itemID = itemsToDelete.Dequeue();

                // Check if this is a file
                if (files.ContainsKey(itemID))
                {
                    files.Remove(itemID);
                }
                // Check if this is a folder
                else if (folders.ContainsKey(itemID))
                {
                    folders.Remove(itemID);
                }
                else
                {
                    throw new Exception($"GC - Failed to item ID (itemID = {itemID})");
                }
            }
        }
    }
}
