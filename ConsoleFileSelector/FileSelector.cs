using ConsoleFileSelector.Enums;
using ConsoleFileSelector.Models;
using System.IO;
using System.Text.Json;

namespace ConsoleFileSelector
{
    public class FileSelector(string rootPath, bool showHiddenFolders = false, bool showHiddenFiles = false, Action? printHeaderCallback = null)
    {


        public string SelectFile(string currentPath, bool lastTryFailed = false)
        {
            Console.Clear();
            if (printHeaderCallback != null)
                printHeaderCallback();

            if (lastTryFailed)
            {
                Console.WriteLine("Last Attempt Failed! Try again.");
                Console.WriteLine();
            }

            Console.WriteLine($"Entries from Path: {currentPath}");
            var entries = GetEntries(currentPath, showHiddenFolders, showHiddenFiles);
            var folderEntries = entries.Where(e => e.Type == DirectoryEntryType.Folder).ToList();
            if (folderEntries.Any())
            {
                Console.WriteLine("Folders: ");
                foreach (var folder in folderEntries)
                    Console.WriteLine($"\t{folder.Id}: {folder.DisplayName}");
            }

            var fileEntries = entries.Where(e => e.Type == DirectoryEntryType.File);
            if (folderEntries.Any())
            {
                Console.WriteLine("Files:");
                foreach (var file in fileEntries)
                    Console.WriteLine($"\t{file.Id}: {file.DisplayName}");
            }
           
            Console.WriteLine();
            Console.Write("Enter Selection: ");
            var selection = Console.ReadLine();

            // Validate selection is an integer
            if (!int.TryParse(selection, out var selectedEntryId))
                return SelectFile(currentPath, true);

            // Validate selection is legit
            if(entries.All(e => e.Id != selectedEntryId))
                return SelectFile(currentPath, true);
            
            var selectedEntry = entries.First(e => e.Id == selectedEntryId);

            // if the user selected a folder, add it to the currentPath and try again
            if (selectedEntry.Type == DirectoryEntryType.Folder)
                return SelectFile(selectedEntry.Path);

            return selectedEntry.Path;
        }
        
        private static List<DirectoryEntry> GetEntries(string path, bool showHiddenFolders, bool showHiddenFiles)
        {
            var list = new List<DirectoryEntry>();

            var id = 0;
            var directoryInfo = new DirectoryInfo(path);
            
            if (!path.Equals(directoryInfo.Root.FullName))
            {
                list.Add(new DirectoryEntry(id, directoryInfo.Root.Name, directoryInfo.Root.FullName, DirectoryEntryType.Folder));
                id++;
            }
            if (directoryInfo.Parent != null && !directoryInfo.Root.FullName.Equals(directoryInfo.Parent.FullName))
            {
                list.Add(new DirectoryEntry(id, $"{directoryInfo.Parent.FullName}", directoryInfo.Parent!.FullName, DirectoryEntryType.Folder));
                id++;
            }

            var folders = directoryInfo.GetDirectories();
            foreach (var folder in folders)
            {
                if((folder.Attributes & FileAttributes.Hidden) != 0 && !showHiddenFolders)
                    continue;
                list.Add(new DirectoryEntry(id, folder.Name, folder.FullName, DirectoryEntryType.Folder));
                id++;
            }

            var files = directoryInfo.GetFiles();
            foreach (var file in files)
            {
                if ((file.Attributes & FileAttributes.Hidden) != 0 && !showHiddenFiles)
                    continue;
                list.Add(new DirectoryEntry(id, file.Name, file.FullName, DirectoryEntryType.File));
                id++;
            }

            return list;
        }



    }
}
