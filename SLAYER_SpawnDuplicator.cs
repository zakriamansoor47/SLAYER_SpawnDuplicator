using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using System.Text.Json.Serialization;

public class SLAYER_SpawnDuplicatorConfig : BasePluginConfig
{
    [JsonPropertyName("SpawnDuplicatorSettings")] public Dictionary<string, SpawnDuplicatorSettings> SpawnDuplicatorSettings { get; set; } = new Dictionary<string, SpawnDuplicatorSettings>(StringComparer.OrdinalIgnoreCase)
    {
        ["global"] = new SpawnDuplicatorSettings(),
        ["de_nuke"] = new SpawnDuplicatorSettings()
        {
            TotalSpawns = 32,
            TSpawnXOffset = 50.0f,
            TSpawnYOffset = 20.0f,
            TSpawnZOffset = 0.0f,
            CTSpawnXOffset = 0.0f,
            CTSpawnYOffset = 20.0f,
            CTSpawnZOffset = 0.0f,
            MinimumSpawnGap = 20.0f,
            LateralSampleOffset = 26.0f,
            ClusterLinkDistance2D = 320.0f,
            ClusterZTolerance = 128.0f,
            DominantClusterRatio = 0.55f
        },
        ["de_inferno"] = new SpawnDuplicatorSettings()
        {
            TotalSpawns = 32,
            TSpawnXOffset = 0.0f,
            TSpawnYOffset = 0.0f,
            TSpawnZOffset = 0.0f,
            CTSpawnXOffset = 0.0f,
            CTSpawnYOffset = 0.0f,
            CTSpawnZOffset = 0.0f,
            MinimumSpawnGap = 20.0f,
            LateralSampleOffset = 24.0f,
            ClusterLinkDistance2D = 320.0f,
            ClusterZTolerance = 128.0f,
            DominantClusterRatio = 0.55f
        },
        ["de_ancient"] = new SpawnDuplicatorSettings()
        {
            TotalSpawns = 32,
            TSpawnXOffset = 0.0f,
            TSpawnYOffset = 0.0f,
            TSpawnZOffset = 0.0f,
            CTSpawnXOffset = 0.0f,
            CTSpawnYOffset = 0.0f,
            CTSpawnZOffset = 0.0f,
            MinimumSpawnGap = 20.0f,
            LateralSampleOffset = 34.0f,
            ClusterLinkDistance2D = 320.0f,
            ClusterZTolerance = 128.0f,
            DominantClusterRatio = 0.55f
        }
    };
}
public class SpawnDuplicatorSettings
{
    [JsonPropertyName("TotalSpawns")] public int TotalSpawns { get; set; } = 32; // Total number of spawns per team (including original spawns). The plugin will try to create as many extra spawns as possible up to this limit, while maintaining reasonable spacing between them. Setting this to a very high number may result in fewer spawns being created if there are not enough valid locations to place them without being too close to each other.
    [JsonPropertyName("TSpawnXOffset")] public float TSpawnXOffset { get; set; } = 0.0f; // X offset applied to all Terrorist spawns (original and duplicated). This can be used to shift all T spawns in a certain X direction
    [JsonPropertyName("TSpawnYOffset")] public float TSpawnYOffset { get; set; } = 0.0f; // Y offset applied to all Terrorist spawns (original and duplicated). This can be used to shift all T spawns in a certain Y direction
    [JsonPropertyName("TSpawnZOffset")] public float TSpawnZOffset { get; set; } = 0.0f; // Z offset applied to all Terrorist spawns (original and duplicated). This can be used to shift all T spawns in a certain Z direction
    [JsonPropertyName("CTSpawnXOffset")] public float CTSpawnXOffset { get; set; } = 0.0f; // X offset applied to all Counter-Terrorist spawns (original and duplicated). This can be used to shift all CT spawns in a certain X direction
    [JsonPropertyName("CTSpawnYOffset")] public float CTSpawnYOffset { get; set; } = 0.0f; // Y offset applied to all Counter-Terrorist spawns (original and duplicated). This can be used to shift all CT spawns in a certain Y direction
    [JsonPropertyName("CTSpawnZOffset")] public float CTSpawnZOffset { get; set; } = 0.0f; // Z offset applied to all Counter-Terrorist spawns (original and duplicated). This can be used to shift all CT spawns in a certain Z direction
    [JsonPropertyName("MinimumSpawnGap")] public float MinimumSpawnGap { get; set; } = 20.0f; // Minimum desired 2D distance between any two spawns (original or duplicated) to ensure good spacing and avoid overcrowding. The plugin will prioritize creating spawns that are at least this far apart, but if it cannot find enough valid locations with this gap, it may create some spawns that are closer together. Setting this to a higher value will result in fewer extra spawns being created, while setting it to a lower value may allow for more spawns but with less spacing between them.
    [JsonPropertyName("LateralSampleOffset")] public float LateralSampleOffset { get; set; } = 24.0f; // When generating candidate spawn positions along the line between two original spawns, this value determines how far to the side of that line (perpendicular) the plugin will also sample for potential spawn locations. This can help create more varied spawn positions that are not just in a straight line between existing spawns. Setting this to 0 will disable lateral sampling and only generate candidates directly between original spawns, while setting it to a higher value will create candidates further to the sides, which may increase the chances of finding valid spawn locations but could also result in more outliers if set too high.
    [JsonPropertyName("ClusterLinkDistance2D")] public float ClusterLinkDistance2D { get; set; } = 320.0f; // When analyzing the original spawns to determine where to create duplicates, the plugin will group nearby spawns into clusters based on this 2D distance threshold. Spawns that are within this distance of each other (and within the Z tolerance) will be considered part of the same cluster. The plugin will then try to distribute duplicated spawns across these clusters to maintain good coverage of the map. Setting this to a lower value may result in more clusters with fewer spawns in each, while setting it to a higher value may group all spawns into one cluster, which could lead to less optimal distribution of duplicates.
    [JsonPropertyName("ClusterZTolerance")] public float ClusterZTolerance { get; set; } = 128.0f; // The tolerance for the Z coordinate when grouping spawns into clusters. Spawns that are within this distance vertically will be considered part of the same cluster.
    [JsonPropertyName("DominantClusterRatio")] public float DominantClusterRatio { get; set; } = 0.55f; // The ratio of spawns that must belong to the dominant cluster for it to be considered the primary cluster for distributing duplicates. If the largest cluster contains at least this percentage of the total original spawns, and has at least 8 spawns, and there are at least 12 original spawns in total, then the plugin will only use that dominant cluster for placing duplicates. This can help avoid placing duplicates in outlier clusters that may be far away from where most players spawn. Setting this to a higher value will make it more likely that only the dominant cluster is used, while setting it to a lower value will allow for more clusters to be used for duplication.

}
public class SpawnDuplicator : BasePlugin, IPluginConfig<SLAYER_SpawnDuplicatorConfig>
{
    public override string ModuleName => "SLAYER_SpawnDuplicator";
    public override string ModuleVersion => "1.0";
    public override string ModuleAuthor => "SLAYER";
    public required SLAYER_SpawnDuplicatorConfig Config {get; set;}
    private const string GlobalSettingsKey = "global";
    private SpawnDuplicatorSettings ActiveSettings = new SpawnDuplicatorSettings();
    private string ActiveSettingsKey = GlobalSettingsKey;
    public void OnConfigParsed(SLAYER_SpawnDuplicatorConfig config)
    {
        Config = config;
        NormalizeSettingsDictionary();
        ActiveSettings = ResolveSettingsForMap(null, out ActiveSettingsKey);
    }
    private bool BotsSwitched = false;
    private Dictionary<string, List<(Vector, QAngle)>> SpawnsPositions = new Dictionary<string, List<(Vector, QAngle)>>();
    private int TIndex = 0;
    private int CTIndex = 0;
    public override void Load(bool hotReload)
    {
        RegisterListener<Listeners.OnMapStart>((string mapName) =>
        {
            AddTimer(1.0f, () =>
            {
                SpawnsPositions.Clear();
                ActiveSettings = ResolveSettingsForMap(mapName, out ActiveSettingsKey);
                // Thanks to devPoland (Discord: madeinpoland) for providing the code for adding new spawns, and information for adding all bots to the game if 24v24 situation happens
                DuplicateSpawnsForTeam("info_player_terrorist");
                DuplicateSpawnsForTeam("info_player_counterterrorist");
            });
        });

        RegisterEventHandler<EventPlayerSpawn>((@event, @info) =>
        {
            if (SpawnsPositions.Count == 0) return HookResult.Continue; // If there are no duplicated spawns, we don't need to do anything, so we just continue with the normal spawn process.

            var player = @event.Userid;
            if (player == null || !player.IsValid || player.PlayerPawn.Value == null) return HookResult.Continue;

            if (player.TeamNum == 2) // If the player is on the T team and we have duplicated T spawns, we teleport the player to one of the duplicated T spawns based on the current index, and then increment the index for the next player. We use modulo operator to loop back to the start of the spawn list if we exceed it, which allows us to reuse the same spawns for multiple players if there are more players than spawns.
            {
                QueueSpawnTeleport(player, "info_player_terrorist", true);
            }
            else if (player.TeamNum == 3)
            {
                QueueSpawnTeleport(player, "info_player_counterterrorist", false);
            }


            return HookResult.Continue;
        });
        RegisterEventHandler<EventCsPreRestart>((@event, @info) =>
        {
            TIndex = 0;
            CTIndex = 0;
            return HookResult.Continue;
        });
        RegisterEventHandler<EventRoundStart>((@event, @info) =>
        {
            var players = new List<CCSPlayerController>();
            AddTimer(0.8f, () =>
            {
                var playersCount = 0;
                var botsCount = 0;
                foreach (var player in Utilities.GetPlayers().Where(p => p.IsValid && p.TeamNum >= 2 && p.PlayerPawn.Value != null && !p.IsHLTV))
                {
                    players.Add(player);
                    playersCount++;
                    if (player.IsBot) botsCount++;
                    //player.Respawn();
                }
                
                int botQuota = ConVar.Find("bot_quota")?.GetPrimitiveValue<int>() ?? 0;
                if (BotsSwitched && botQuota < 48) // Reset the flag if there are less than 48 bots, which means some bots might have left or been kicked, allowing the plugin to switch teams again next time if bots are 48.
                {
                    BotsSwitched = false;
                    return; // No need to switch teams if there are less than 48 bots, but we reset the flag so that if bots are added back up to 48, the plugin can switch teams again.
                }
                if (playersCount == 0 || playersCount < 48 || BotsSwitched || playersCount >= ((ActiveSettings.TotalSpawns * 2) - 1)) return;
                foreach (var player in players)
                {
                    if (!player.IsBot) continue; // Just a safety check, we only want to switch teams for bots, not real players.

                    if (player.TeamNum == 2)
                    {
                        player.SwitchTeam(CsTeam.CounterTerrorist);
                        Server.NextFrame(() =>
                        {
                            player.CommitSuicide(false, true); // Slaying is necessary to make all bots spawn on the new spawn points
                        });
                    }
                    else if (player.TeamNum == 3)
                    {
                        player.SwitchTeam(CsTeam.Terrorist);
                        Server.NextFrame(() =>
                        {
                            player.CommitSuicide(false, true); // Slaying is necessary to make all bots spawn on the new spawn points
                        });
                    }
                    BotsSwitched = true;
                }
                if (BotsSwitched)
                {
                    AddTimer(5f, ()=>   // Terminate the round to make sure all players respawn on the new spawn points
                    {
                        if(GetGameRules() != null) GetGameRules().TerminateRound(1f, RoundEndReason.Unknown);
                    });

                }
            });

            return HookResult.Continue;
        });
    }

    private void NormalizeSettingsDictionary()
    {
        var normalized = new Dictionary<string, SpawnDuplicatorSettings>(StringComparer.OrdinalIgnoreCase);

        if (Config.SpawnDuplicatorSettings != null)
        {
            foreach (var entry in Config.SpawnDuplicatorSettings)
            {
                if (string.IsNullOrWhiteSpace(entry.Key))
                {
                    continue;
                }

                normalized[entry.Key.Trim()] = entry.Value ?? new SpawnDuplicatorSettings();
            }
        }

        if (!normalized.ContainsKey(GlobalSettingsKey))
        {
            normalized[GlobalSettingsKey] = new SpawnDuplicatorSettings();
        }

        Config.SpawnDuplicatorSettings = normalized;
    }

    private SpawnDuplicatorSettings ResolveSettingsForMap(string? mapName, out string resolvedKey)
    {
        NormalizeSettingsDictionary();

        if (!string.IsNullOrWhiteSpace(mapName))
        {
            string rawMapName = mapName.Trim();
            if (Config.SpawnDuplicatorSettings.TryGetValue(rawMapName, out var directSettings) && directSettings != null)
            {
                resolvedKey = rawMapName;
                return directSettings;
            }

            string canonicalMapName = CanonicalizeMapName(rawMapName);
            if (!string.Equals(canonicalMapName, rawMapName, StringComparison.OrdinalIgnoreCase)
                && Config.SpawnDuplicatorSettings.TryGetValue(canonicalMapName, out var canonicalSettings)
                && canonicalSettings != null)
            {
                resolvedKey = canonicalMapName;
                return canonicalSettings;
            }
        }

        if (Config.SpawnDuplicatorSettings.TryGetValue(GlobalSettingsKey, out var globalSettings) && globalSettings != null)
        {
            resolvedKey = GlobalSettingsKey;
            return globalSettings;
        }

        if (Config.SpawnDuplicatorSettings.Count > 0)
        {
            var first = Config.SpawnDuplicatorSettings.First();
            resolvedKey = first.Key;
            return first.Value ?? new SpawnDuplicatorSettings();
        }

        resolvedKey = GlobalSettingsKey;
        var fallback = new SpawnDuplicatorSettings();
        Config.SpawnDuplicatorSettings[GlobalSettingsKey] = fallback;
        return fallback;
    }
    private static string CanonicalizeMapName(string mapName)
    {
        string normalized = mapName.Trim().Replace('\\', '/');
        int slashIndex = normalized.LastIndexOf('/');
        if (slashIndex >= 0 && slashIndex < normalized.Length - 1)
        {
            normalized = normalized.Substring(slashIndex + 1);
        }

        const string bspSuffix = ".bsp";
        if (normalized.EndsWith(bspSuffix, StringComparison.OrdinalIgnoreCase))
        {
            normalized = normalized.Substring(0, normalized.Length - bspSuffix.Length);
        }

        return normalized;
    }

    private void QueueSpawnTeleport(CCSPlayerController player, string className, bool isTTeam)
    {
        if (!SpawnsPositions.TryGetValue(className, out var spawnList) || spawnList.Count == 0) return;

        int spawnNumber;
        (Vector, QAngle) spawnInfo;
        if (isTTeam)
        {
            spawnInfo = spawnList[TIndex % spawnList.Count];
            TIndex++;
            spawnNumber = TIndex;
        }
        else
        {
            spawnInfo = spawnList[CTIndex % spawnList.Count];
            CTIndex++;
            spawnNumber = CTIndex;
        }

        if (!IsFiniteVector(spawnInfo.Item1) || !IsFiniteQAngle(spawnInfo.Item2)) return;

        AddTimer(0.1f, () =>
        {
            if (!player.IsValid || player.PlayerPawn.Value == null) return;
            if (isTTeam && player.TeamNum != 2) return;
            if (!isTTeam && player.TeamNum != 3) return;

            var livePawn = player.PlayerPawn.Value;
            if (!livePawn.IsValid || livePawn.LifeState != (byte)LifeState_t.LIFE_ALIVE) return;

            livePawn.Teleport(spawnInfo.Item1, spawnInfo.Item2, Vector.Zero);
        });

        // Debug
        // if (!isTTeam)
        // {
        //     Server.PrintToChatAll($" {ChatColors.Blue}[SLAYER SpawnDuplicator] Teleporting CT player {player.PlayerName} to spawn #{spawnNumber} at position {spawnInfo.Item1} with angle {spawnInfo.Item2}");
        // }
        // else
        // {
        //     Server.PrintToChatAll($" {ChatColors.Orange}[SLAYER SpawnDuplicator] Teleporting T player {player.PlayerName} to spawn #{spawnNumber} at position {spawnInfo.Item1} with angle {spawnInfo.Item2}");
        // }
    }
    
    private void DuplicateSpawnsForTeam(string className)
    {
        var originals = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>(className);
        if (!originals.Any()) return;

        int copiesCreated = 0;
        List<(Vector, QAngle)> allOriginalPositions = new List<(Vector, QAngle)>();

        // Store original positions and angles of the spawns
        foreach (var original in originals)
        {
            if (!original.IsValid) continue;

            Vector pos = ApplySpawnOffset(original.AbsOrigin ?? Vector.Zero, className);
            QAngle ang = original.AbsRotation ?? new QAngle(0, 0, 0);

            if (!IsFiniteVector(pos) || !IsFiniteQAngle(ang))
            {
                continue;
            }

            if (MinDistanceToSpawns(pos, allOriginalPositions) < 1.0f)
            {
                continue;
            }

            allOriginalPositions.Add((pos, ang));
        }

        if (allOriginalPositions.Count == 0) return;

        var spawnClusters = BuildSpawnClusters(allOriginalPositions);
        if (spawnClusters.Count == 0) return;

        float dominantClusterRatio = Math.Clamp(ActiveSettings.DominantClusterRatio, 0.5f, 0.95f);
        bool useDominantClusterOnly = spawnClusters.Count > 1
            && allOriginalPositions.Count >= 12
            && spawnClusters[0].Count >= 8
            && (spawnClusters[0].Count / (float)allOriginalPositions.Count) >= dominantClusterRatio;

        var activeClusters = useDominantClusterOnly
            ? new List<List<(Vector, QAngle)>> { spawnClusters[0] }
            : spawnClusters;

        if (useDominantClusterOnly)
        {
            Console.WriteLine($"[SLAYER SpawnDuplicator] {className}: Using dominant spawn cluster ({spawnClusters[0].Count}/{allOriginalPositions.Count}) to avoid cross-map outliers.");
        }

        var orderedClusters = activeClusters
            .Where(cluster => cluster.Count > 0)
            .Select(cluster => OrderSpawnsByNearestNeighbor(cluster))
            .ToList();

        if (orderedClusters.Count == 0) return;

        SpawnsPositions[className] = new List<(Vector, QAngle)>();
        foreach (var cluster in orderedClusters)
        {
            foreach (var spawn in cluster)
            {
                if (MinDistanceToSpawns(spawn.Item1, SpawnsPositions[className]) < 1.0f)
                {
                    continue;
                }

                SpawnsPositions[className].Add(spawn);
            }
        }

        int baseSpawnCount = SpawnsPositions[className].Count;
        if (baseSpawnCount == 0) return;

        int extraCopies = Math.Clamp(ActiveSettings.TotalSpawns - baseSpawnCount, 0, 64); // Calculate how many extra copies we need to create based on the total spawns specified in the config, clamped to a maximum of 64 (since that's the max number of spawns that can be active at once in CS2).
        if (extraCopies <= 0)
        {
            Console.WriteLine($"[SLAYER SpawnDuplicator] {className}: Using {baseSpawnCount} original spawns (no extras required).");
            return;
        }

        int[] copiesPerCluster = DistributeCopiesAcrossClusters(orderedClusters, extraCopies);
        for (int clusterIndex = 0; clusterIndex < orderedClusters.Count; clusterIndex++)
        {
            int clusterCopies = copiesPerCluster[clusterIndex];
            if (clusterCopies <= 0)
            {
                continue;
            }

            var candidateSpawns = BuildSegmentCandidates(orderedClusters[clusterIndex], clusterCopies);
            if (candidateSpawns.Count == 0)
            {
                continue;
            }

            var selectedSpawns = SelectBestSpacedSpawns(candidateSpawns, SpawnsPositions[className], clusterCopies);
            foreach (var (pos, ang) in selectedSpawns)
            {
                var newSpawn = Utilities.CreateEntityByName<CBaseEntity>(className);
                if (newSpawn == null || !newSpawn.IsValid) continue;

                newSpawn.Teleport(pos, ang, Vector.Zero);
                newSpawn.DispatchSpawn();

                SpawnsPositions[className].Add((pos, ang));
                copiesCreated++;
            }
        }

        if (copiesCreated < extraCopies)
        {
            Console.WriteLine($"[SLAYER SpawnDuplicator] Requested {extraCopies} extra {className} spawns but only created {copiesCreated} due to spacing constraints. Base {baseSpawnCount}");
        }
        else
        {
            Console.WriteLine($"[SLAYER SpawnDuplicator] Created {copiesCreated} extra {className} spawns. Base {baseSpawnCount}");
        }
    }
    private List<(Vector, QAngle)> BuildSegmentCandidates(List<(Vector, QAngle)> orderedSpawns, int extraCopies)
    {
        int segmentCount = orderedSpawns.Count;
        if (segmentCount < 2 || extraCopies <= 0)
        {
            return new List<(Vector, QAngle)>();
        }

        float[] segmentLengths = new float[segmentCount];
        float totalLoopLength = 0f;
        for (int segment = 0; segment < segmentCount; segment++)
        {
            var (pos1, _) = orderedSpawns[segment];
            var (pos2, _) = orderedSpawns[(segment + 1) % segmentCount];
            float length = Distance2D(pos1, pos2);
            if (!float.IsFinite(length) || length <= 0.001f)
            {
                length = 0f;
            }

            segmentLengths[segment] = length;
            totalLoopLength += length;
        }

        if (totalLoopLength <= 0.001f)
        {
            return new List<(Vector, QAngle)>();
        }

        int totalSamples = Math.Max(extraCopies * 24, segmentCount * 12);
        float lateralOffset = Math.Clamp(ActiveSettings.LateralSampleOffset, 0f, 50f);
        var candidates = new List<(Vector, QAngle)>(totalSamples);

        for (int segment = 0; segment < segmentCount; segment++)
        {
            if (segmentLengths[segment] <= 0.001f)
            {
                continue;
            }

            int samplesInSegment = Math.Max(2, (int)MathF.Round((segmentLengths[segment] / totalLoopLength) * totalSamples));
            var (pos1, ang1) = orderedSpawns[segment];
            var (pos2, _) = orderedSpawns[(segment + 1) % segmentCount];
            float dirX = pos2.X - pos1.X;
            float dirY = pos2.Y - pos1.Y;
            float dirLength2D = MathF.Sqrt((dirX * dirX) + (dirY * dirY));
            float perpX = 0f;
            float perpY = 0f;
            if (dirLength2D > 0.001f)
            {
                perpX = -dirY / dirLength2D;
                perpY = dirX / dirLength2D;
            }

            void TryAddCandidate(Vector candidatePos)
            {
                if (!IsFiniteVector(candidatePos))
                {
                    return;
                }

                if (MinDistanceToSpawns(candidatePos, candidates) < 6.0f)
                {
                    return;
                }

                candidates.Add((candidatePos, ang1));
            }

            for (int sample = 1; sample <= samplesInSegment; sample++)
            {
                float t = sample / (samplesInSegment + 1f);
                Vector basePos = Lerp(pos1, pos2, t);

                TryAddCandidate(basePos);

                if (lateralOffset > 0f && dirLength2D > 0.001f)
                {
                    TryAddCandidate(new Vector(basePos.X + (perpX * lateralOffset), basePos.Y + (perpY * lateralOffset), basePos.Z));
                    TryAddCandidate(new Vector(basePos.X - (perpX * lateralOffset), basePos.Y - (perpY * lateralOffset), basePos.Z));
                }
            }
        }

        return candidates;
    }
    private List<List<(Vector, QAngle)>> BuildSpawnClusters(List<(Vector, QAngle)> spawns)
    {
        var clusters = new List<List<(Vector, QAngle)>>();
        if (spawns.Count == 0)
        {
            return clusters;
        }

        float linkDistance2D = Math.Clamp(ActiveSettings.ClusterLinkDistance2D, 64f, 2048f);
        float zTolerance = Math.Clamp(ActiveSettings.ClusterZTolerance, 16f, 1024f);

        bool[] visited = new bool[spawns.Count];
        for (int i = 0; i < spawns.Count; i++)
        {
            if (visited[i])
            {
                continue;
            }

            var cluster = new List<(Vector, QAngle)>();
            var queue = new Queue<int>();
            queue.Enqueue(i);
            visited[i] = true;

            while (queue.Count > 0)
            {
                int currentIndex = queue.Dequeue();
                var currentSpawn = spawns[currentIndex];
                cluster.Add(currentSpawn);

                for (int candidateIndex = 0; candidateIndex < spawns.Count; candidateIndex++)
                {
                    if (visited[candidateIndex])
                    {
                        continue;
                    }

                    if (!AreSpawnsConnected(currentSpawn.Item1, spawns[candidateIndex].Item1, linkDistance2D, zTolerance))
                    {
                        continue;
                    }

                    visited[candidateIndex] = true;
                    queue.Enqueue(candidateIndex);
                }
            }

            if (cluster.Count > 0)
            {
                clusters.Add(cluster);
            }
        }

        return clusters
            .OrderByDescending(cluster => cluster.Count)
            .ToList();
    }
    private static int[] DistributeCopiesAcrossClusters(List<List<(Vector, QAngle)>> clusters, int totalCopies)
    {
        int[] copies = new int[clusters.Count];
        if (clusters.Count == 0 || totalCopies <= 0)
        {
            return copies;
        }

        int totalBaseSpawns = clusters.Sum(cluster => cluster.Count);
        if (totalBaseSpawns <= 0)
        {
            return copies;
        }

        float[] fractions = new float[clusters.Count];
        int assignedCopies = 0;
        for (int i = 0; i < clusters.Count; i++)
        {
            float exactCopies = (clusters[i].Count / (float)totalBaseSpawns) * totalCopies;
            int baseCopies = (int)MathF.Floor(exactCopies);
            copies[i] = baseCopies;
            fractions[i] = exactCopies - baseCopies;
            assignedCopies += baseCopies;
        }

        int remainingCopies = totalCopies - assignedCopies;
        if (remainingCopies > 0)
        {
            var order = Enumerable.Range(0, clusters.Count)
                .OrderByDescending(i => fractions[i])
                .ThenByDescending(i => clusters[i].Count)
                .ToList();

            for (int i = 0; i < remainingCopies; i++)
            {
                copies[order[i % order.Count]]++;
            }
        }

        return copies;
    }
    private static bool AreSpawnsConnected(Vector first, Vector second, float maxDistance2D, float zTolerance)
    {
        if (MathF.Abs(first.Z - second.Z) > zTolerance)
        {
            return false;
        }

        return Distance2D(first, second) <= maxDistance2D;
    }
    private List<(Vector, QAngle)> SelectBestSpacedSpawns(List<(Vector, QAngle)> candidates, List<(Vector, QAngle)> existingSpawns, int requiredCopies)
    {
        var selected = new List<(Vector, QAngle)>();
        var remaining = new List<(Vector, QAngle)>(candidates);
        var spacingReference = new List<(Vector, QAngle)>(existingSpawns);

        int FindBestCandidateIndex(out float bestDistance)
        {
            int bestIndex = -1;
            bestDistance = float.NegativeInfinity;

            for (int i = 0; i < remaining.Count; i++)
            {
                float minDistance = MinDistanceToSpawns(remaining[i].Item1, spacingReference);
                if (minDistance > bestDistance)
                {
                    bestDistance = minDistance;
                    bestIndex = i;
                }
            }

            return bestIndex;
        }

        float desiredGap = Math.Clamp(ActiveSettings.MinimumSpawnGap, 0f, 256f);
        float[] distanceThresholds = desiredGap <= 0f
            ? new float[] { 96f, 80f, 64f, 48f, 32f, 16f, 8f, 0f }
            : new float[] { desiredGap + 24f, desiredGap + 16f, desiredGap + 8f, desiredGap };

        foreach (float threshold in distanceThresholds)
        {
            while (selected.Count < requiredCopies && remaining.Count > 0)
            {
                int bestIndex = FindBestCandidateIndex(out float bestDistance);
                if (bestIndex < 0 || bestDistance < threshold)
                {
                    break;
                }

                var chosen = remaining[bestIndex];
                selected.Add(chosen);
                spacingReference.Add(chosen);
                remaining.RemoveAt(bestIndex);
            }

            if (selected.Count >= requiredCopies)
            {
                break;
            }
        }

        // Only do unrestricted fallback when gap is explicitly disabled.
        while (desiredGap <= 0f && selected.Count < requiredCopies && remaining.Count > 0)
        {
            int bestIndex = FindBestCandidateIndex(out _);
            if (bestIndex < 0)
            {
                break;
            }

            var chosen = remaining[bestIndex];
            selected.Add(chosen);
            spacingReference.Add(chosen);
            remaining.RemoveAt(bestIndex);
        }

        return selected;
    }
    private List<(Vector, QAngle)> OrderSpawnsByNearestNeighbor(List<(Vector, QAngle)> spawns)
    {
        if (spawns.Count <= 2)
        {
            return new List<(Vector, QAngle)>(spawns);
        }

        var remaining = new List<(Vector, QAngle)>(spawns);
        int startIndex = 0;
        for (int i = 1; i < remaining.Count; i++)
        {
            var currentPos = remaining[i].Item1;
            var startPos = remaining[startIndex].Item1;
            if (currentPos.X < startPos.X || (MathF.Abs(currentPos.X - startPos.X) < 0.001f && currentPos.Y < startPos.Y))
            {
                startIndex = i;
            }
        }

        var ordered = new List<(Vector, QAngle)>(spawns.Count)
        {
            remaining[startIndex]
        };
        remaining.RemoveAt(startIndex);
        while (remaining.Count > 0)
        {
            Vector lastPos = ordered[ordered.Count - 1].Item1;
            int nearestIndex = 0;
            float nearestDistance = Distance2D(lastPos, remaining[0].Item1);

            for (int i = 1; i < remaining.Count; i++)
            {
                float candidateDistance = Distance2D(lastPos, remaining[i].Item1);
                if (candidateDistance < nearestDistance)
                {
                    nearestDistance = candidateDistance;
                    nearestIndex = i;
                }
            }

            ordered.Add(remaining[nearestIndex]);
            remaining.RemoveAt(nearestIndex);
        }

        return ordered;
    }
    public Vector Lerp(Vector from, Vector to, float t)
    {
        return new Vector(
            from.X + (to.X - from.X) * t,
            from.Y + (to.Y - from.Y) * t,
            from.Z + (to.Z - from.Z) * t
        );
    }
    private Vector ApplySpawnOffset(Vector position, string className)
    {
        if (!float.IsFinite(position.X) || !float.IsFinite(position.Y) || !float.IsFinite(position.Z))
        {
            return position;
        }

        float xOffset = 0f;
        float yOffset = 0f;
        float zOffset = 0f;

        if (string.Equals(className, "info_player_terrorist", StringComparison.OrdinalIgnoreCase))
        {
            xOffset += Math.Clamp(ActiveSettings.TSpawnXOffset, -4096f, 4096f);
            yOffset += Math.Clamp(ActiveSettings.TSpawnYOffset, -4096f, 4096f);
            zOffset += Math.Clamp(ActiveSettings.TSpawnZOffset, -1024f, 1024f);
        }
        else if (string.Equals(className, "info_player_counterterrorist", StringComparison.OrdinalIgnoreCase))
        {
            xOffset += Math.Clamp(ActiveSettings.CTSpawnXOffset, -4096f, 4096f);
            yOffset += Math.Clamp(ActiveSettings.CTSpawnYOffset, -4096f, 4096f);
            zOffset += Math.Clamp(ActiveSettings.CTSpawnZOffset, -1024f, 1024f);
        }

        return new Vector(position.X + xOffset, position.Y + yOffset, position.Z + zOffset);
    }
    private static bool IsFiniteVector(Vector vector)
    {
        return float.IsFinite(vector.X)
            && float.IsFinite(vector.Y)
            && float.IsFinite(vector.Z);
    }
    private static bool IsFiniteQAngle(QAngle angle)
    {
        return float.IsFinite(angle.X)
            && float.IsFinite(angle.Y)
            && float.IsFinite(angle.Z);
    }
    private static float Distance2D(Vector from, Vector to)
    {
        float dx = to.X - from.X;
        float dy = to.Y - from.Y;
        return MathF.Sqrt(dx * dx + dy * dy);
    }
    private static float MinDistanceToSpawns(Vector position, List<(Vector, QAngle)> spawns)
    {
        if (spawns.Count == 0)
        {
            return float.MaxValue;
        }

        float minDistance = float.MaxValue;
        foreach (var (spawnPos, _) in spawns)
        {
            float distance = Distance2D(position, spawnPos);
            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }

        return minDistance;
    }
    public static CCSGameRules GetGameRules()
    {
        return Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").First().GameRules!;
    }
}