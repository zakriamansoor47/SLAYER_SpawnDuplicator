# Accepting Paid Request! Discord: Slayer47#7002
# Donation
If you like this project, consider supporting me:

<a href="https://www.buymeacoffee.com/slayer47" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png" alt="Buy Me A Coffee" style="height: 41px !important;width: 174px !important;box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;-webkit-box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;" ></a>
[![PayPal](https://www.paypalobjects.com/webstatic/mktg/logo/pp_cc_mark_37x23.jpg)](https://paypal.me/zakriamansoor)

# SLAYER_SpawnDuplicator ![](https://img.shields.io/github/downloads/zakriamansoor47/SLAYER_SpawnDuplicator/total?style=for-the-badge)

CS2 CounterStrikeSharp plugin that expands team spawn capacity by generating extra spawn entities and teleporting players to those points on spawn.

This plugin is designed for high-slot/bot-heavy servers where stock map spawns are not enough.

## Features

- Duplicates Terrorist and Counter-Terrorist spawns up to configured limits.
- Per-map settings via dictionary (`global` fallback + map-specific overrides).
- Team-specific position offsets (`T` and `CT` each have independent `X/Y/Z` offsets).
- Smart spawn clustering to avoid linking distant outlier spawn groups on complex maps.
- Spacing-aware spawn selection with configurable `MinimumSpawnGap`.
- Candidate lateral sampling for better distribution (not only strict line interpolation).
- Works with large bot counts and includes auto round terminate flow used by this plugin for high-pop scenarios.

## How It Works

1. On map start, the plugin resolves active settings for the current map.
2. It collects original `info_player_terrorist` and `info_player_counterterrorist` entities.
3. Team offsets are applied to spawn positions.
4. Original spawns are grouped into clusters (2D distance + Z tolerance).
5. Extra spawns are generated from sampled candidates and selected using spacing rules.
6. New spawn entities are created and saved.
7. On each player spawn event, players are teleported to the next prepared spawn for their team.

## Configuration

Main config object:

- `SpawnDuplicatorSettings` (dictionary)
	- key: map name (`de_inferno`, `de_nuke`, etc.)
	- value: `SpawnDuplicatorSettings`

### Example Config

```json
{
	"SpawnDuplicatorSettings": {
		"global": {
			"TotalSpawns": 32,
			"TSpawnXOffset": 0.0,
			"TSpawnYOffset": 0.0,
			"TSpawnZOffset": 0.0,
			"CTSpawnXOffset": 0.0,
			"CTSpawnYOffset": 0.0,
			"CTSpawnZOffset": 0.0,
			"MinimumSpawnGap": 20.0,
			"LateralSampleOffset": 24.0,
			"ClusterLinkDistance2D": 320.0,
			"ClusterZTolerance": 128.0,
			"DominantClusterRatio": 0.55
		},
		"de_nuke": {
			"TotalSpawns": 32,
			"TSpawnXOffset": 50.0,
			"TSpawnYOffset": 20.0,
			"TSpawnZOffset": 0.0,
			"CTSpawnXOffset": 0.0,
			"CTSpawnYOffset": 20.0,
			"CTSpawnZOffset": 0.0,
			"MinimumSpawnGap": 20.0,
			"LateralSampleOffset": 26.0,
			"ClusterLinkDistance2D": 320.0,
			"ClusterZTolerance": 128.0,
			"DominantClusterRatio": 0.55
		}
	}
}
```

## Option Reference

All distances are Hammer units.

| Option | Type | Default | Description |
|---|---|---:|---|
| `TotalSpawns` | int | `32` | Target spawn count per team (including original map spawns). |
| `TSpawnXOffset` | float | `0` | Additional X offset applied to all T spawns (original + duplicated). |
| `TSpawnYOffset` | float | `0` | Additional Y offset applied to all T spawns (original + duplicated). |
| `TSpawnZOffset` | float | `0` | Additional Z offset applied to all T spawns (original + duplicated). |
| `CTSpawnXOffset` | float | `0` | Additional X offset applied to all CT spawns (original + duplicated). |
| `CTSpawnYOffset` | float | `0` | Additional Y offset applied to all CT spawns (original + duplicated). |
| `CTSpawnZOffset` | float | `0` | Additional Z offset applied to all CT spawns (original + duplicated). |
| `MinimumSpawnGap` | float | `20` | Minimum 2D spacing target between selected spawn points. Set `0` to disable hard gap fallback behavior. |
| `LateralSampleOffset` | float | `24` | Side-sampling distance used while generating candidate points between spawn segments. |
| `ClusterLinkDistance2D` | float | `320` | Max 2D distance used to connect spawn points into the same cluster. |
| `ClusterZTolerance` | float | `128` | Max Z difference allowed when connecting spawn points into the same cluster. |
| `DominantClusterRatio` | float | `0.55` | If largest cluster ratio is high enough (with minimum count checks), plugin can prioritize that dominant cluster to avoid outlier groups. |

## Tuning Tips

- If players still collide at spawn:
	- Increase `MinimumSpawnGap`
	- Increase `LateralSampleOffset`
	- Adjust team offsets (`TSpawn*`, `CTSpawn*`)

- If spawns appear in distant/outlier areas on some maps:
	- Lower `ClusterLinkDistance2D`
	- Lower `ClusterZTolerance`
	- Increase `DominantClusterRatio`

- If too few extra spawns are created:
	- Lower `MinimumSpawnGap`
	- Lower `LateralSampleOffset`
	- Ensure `TotalSpawns` is realistic for map geometry

## Notes

- Plugin may intentionally create fewer than requested extra spawns if spacing constraints cannot be satisfied.
- Spawn placement quality depends heavily on each map's original spawn layout.
- For best results, tune values per map instead of only using `global`.

## Credits

- Original base idea and support notes from devPoland (Discord: `madeinpoland`) referenced in source comments.
