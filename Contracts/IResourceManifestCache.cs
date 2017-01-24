using System.Collections.Generic;

namespace WebApplication.Contracts {
    public interface IResourceManifestCache {
        Dictionary<string, string> GetManifest();
    }
}