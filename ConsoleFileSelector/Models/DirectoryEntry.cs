using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleFileSelector.Enums;

namespace ConsoleFileSelector.Models
{
    public class DirectoryEntry(int id, string displayName, string path, DirectoryEntryType type)
    {
        public int Id { get; set; } = id;
        public string DisplayName { get; set; } = displayName;
        public string Path { get; set; } = path;
        public DirectoryEntryType Type { get; set; } = type;
    }
}
