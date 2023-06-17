using System.IO;
using UnityEngine;

namespace CodeLibrary24.HandPoser
{
    public static class PackageFinder  // TODO: Make a separate package or add to utils package
    {
        private static string PackageManifestPath => Application.dataPath + "/../Packages/manifest.json";
        private static string PackagesLockPath => Application.dataPath + "/../Packages/packages-lock.json";

        public static bool IsPackageInstalled(string packageID)
        {
            string packageManifestText = File.ReadAllText(PackageManifestPath);
            if (packageManifestText.Contains(packageID))
            {
                return true;
            }
            else
            {
                string packagesLockText = File.ReadAllText(PackagesLockPath);
                return packagesLockText.Contains(packageID);
            }
        }
    }
}