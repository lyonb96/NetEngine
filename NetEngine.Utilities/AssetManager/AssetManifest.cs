namespace NetEngine.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Describes asset information and contains debug facilities for auditing the manifest.
    /// </summary>
    public class AssetManifest
    {
        public List<AssetManifestData> Assets { get; set; }

        /// <summary>
        /// Audits the loaded asset manifest for cyclical or missing dependencies
        /// </summary>
        public void AuditManifest()
        {
#if DEBUG
            if (Assets == null) return;

            foreach (var asset in Assets)
            {
                if (asset.Dependencies == null || asset.Dependencies.Count == 0)
                {
                    continue;
                }

                // Find dependencies in the manifest and verify none are missing
                var missingDeps = asset.Dependencies
                    .Where(x => !Assets.Any(y => y.Name == x));
                if (missingDeps.Any())
                {
                    throw new Exception($"Asset {asset.Name} has dependencies which were not found in the manifest:\n{string.Join(", ", missingDeps)}");
                }

                CheckForCyclicalDependency(asset);
            }
#endif
        }

#if DEBUG
        /// <summary>
        /// Recursive method to scan the manifest for cyclical dependency.
        /// </summary>
        /// <param name="asset">The asset to check.</param>
        /// <param name="upstreamDeps">A list of upstream assets that depend on this one (directly and indirectly).</param>
        private void CheckForCyclicalDependency(AssetManifestData asset, List<string> upstreamDeps = null)
        {
            // Early out if an asset has no dependencies
            if (asset.Dependencies == null || asset.Dependencies.Count == 0) return;

            // Fetch dependency metadata
            var dependencyMetaData = Assets
                .Where(x => asset.Dependencies.Contains(x.Name))
                .Distinct();

            // Check if any of the dependencies are in the upstream data
            if (upstreamDeps != null)
            {
                if (asset.Dependencies.Any(x => upstreamDeps.Contains(x)))
                {
                    throw new Exception($"Cyclical dependency detected: {string.Join(" -> ", upstreamDeps)} -> {asset.Name} -> {{ {string.Join(", ", asset.Dependencies)} }}");
                }
            }

            // Append this asset to the upstream deps and check its children
            upstreamDeps ??= new List<string>();
            foreach (var dependency in dependencyMetaData)
            {
                var deps = new List<string>();
                deps.AddRange(upstreamDeps);
                deps.Add(asset.Name);
                CheckForCyclicalDependency(dependency, deps);
            }
        }
#endif
    }
}
