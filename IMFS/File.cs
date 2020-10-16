using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMFS
{
    public class File
    {
        public long ID { get; private set; }
        public string Name { get; private set; }
        public long PID { get; private set; }
        public bool IsDeleted { get; private set; } = false;
        private byte[] data;
        public byte[] Data {
            get {
                byte[] res = new byte[data.Length];
                data.CopyTo(res, 0);
                return res;
            }
        }

        public File(long id, string name, byte[] data, long pid)
        {
            this.ID = id;
            this.Name = name;
            this.data = data;
            this.PID = pid;
        }

        public void Rename(string newName)
        {
            this.Name = newName;
        }

        public void Delete()
        {
            this.IsDeleted = true;
        }
    }
}
