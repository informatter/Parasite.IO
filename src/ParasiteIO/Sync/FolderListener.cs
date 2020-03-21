using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parasite;
namespace Parasite.Core.Sync
{
    public class FolderListener
    {
        private readonly FileSystemWatcher fileSystemWatcher;

        public bool CanExpire = false;

        public FolderListener()
        {


            fileSystemWatcher = new FileSystemWatcher(FolderInfo.dirPath)
            {
                EnableRaisingEvents = true,

                NotifyFilter = NotifyFilters.LastAccess
              | NotifyFilters.LastWrite
              | NotifyFilters.FileName
              | NotifyFilters.DirectoryName
              | NotifyFilters.Size
            };

            fileSystemWatcher.Changed += new FileSystemEventHandler(OnChanged);
            // Instruct the file system watcher to call the FileCreated method
            // when there are files created at the folder.
            fileSystemWatcher.Created += new FileSystemEventHandler(FileCreated);

        } 


        private void FileCreated(Object sender, FileSystemEventArgs e)
        {

            CanExpire = true;

        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            CanExpire = true;
        }
    }
}
