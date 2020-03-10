using System;
using System.IO;


namespace Parasite.Core.Sync
{
    internal static class FolderInfo
    {
        internal readonly static string userProfileDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        internal readonly static string dirPath = Path.Combine(userProfileDir, "Parasite.IO.Exchange");
    }
}
