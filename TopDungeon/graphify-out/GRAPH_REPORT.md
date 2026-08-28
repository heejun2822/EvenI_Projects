# Graph Report - TopDungeon  (2026-08-27)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 586 nodes · 756 edges · 48 communities (28 shown, 20 thin omitted)
- Extraction: 99% EXTRACTED · 1% INFERRED · 0% AMBIGUOUS · INFERRED: 11 edges (avg confidence: 0.82)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `8849469f`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- com.unity.2d.sprite
- Collidable
- com.unity.mathematics
- com.unity.modules.unitywebrequest
- GameManager
- dependencies
- com.unity.modules.imgui
- com.unity.modules.audio
- com.unity.modules.physics
- com.unity.modules.jsonserialize
- Enemy
- CharacterMenu
- FloatingText
- com.unity.modules.imageconversion
- Fighter
- com.unity.2d.spriteshape
- com.unity.ext.nunit
- com.unity.ugui
- dependencies
- com.unity.modules.animation
- com.unity.2d.animation
- com.unity.2d.tooling
- com.unity.2d.common
- com.unity.collections
- com.unity.modules.uielements
- com.unity.inputsystem
- com.unity.multiplayer.center
- com.unity.modules.vr
- com.unity.feature.2d
- com.unity.ide.rider
- com.unity.ide.visualstudio
- com.unity.inputsystem
- com.unity.modules.accessibility
- com.unity.modules.adaptiveperformance
- com.unity.modules.androidjni
- com.unity.modules.cloth
- com.unity.modules.screencapture
- com.unity.modules.terrainphysics
- com.unity.modules.umbra
- com.unity.modules.unitywebrequesttexture
- com.unity.modules.unitywebrequestwww
- com.unity.modules.vectorgraphics
- com.unity.modules.vehicles
- com.unity.modules.video
- com.unity.modules.wind
- com.unity.multiplayer.center
- com.unity.timeline
- com.unity.visualscripting

## God Nodes (most connected - your core abstractions)
1. `GameManager` - 22 edges
2. `com.unity.modules.jsonserialize` - 16 edges
3. `Collidable` - 14 edges
4. `Player` - 13 edges
5. `com.unity.modules.uielements` - 13 edges
6. `com.unity.mathematics` - 12 edges
7. `com.unity.2d.common` - 12 edges
8. `com.unity.modules.unitywebrequest` - 12 edges
9. `com.unity.modules.physics` - 12 edges
10. `Weapon` - 11 edges

## Surprising Connections (you probably didn't know these)
- `EnemyHitbox` --inherits--> `Collidable`  [EXTRACTED]
  Assets/Scripts/EnemyHitbox.cs → Assets/Scripts/Collidable.cs
- `GameManager` --references--> `Weapon`  [EXTRACTED]
  Assets/Scripts/GameManager.cs → Assets/Scripts/Weapon.cs
- `Mover` --inherits--> `Fighter`  [EXTRACTED]
  Assets/Scripts/Mover.cs → Assets/Scripts/Fighter.cs
- `Player` --inherits--> `Mover`  [EXTRACTED]
  Assets/Scripts/Player.cs → Assets/Scripts/Mover.cs
- `GameManager` --references--> `FloatingTextManager`  [EXTRACTED]
  Assets/Scripts/GameManager.cs → Assets/Scripts/FloatingTextManager.cs

## Import Cycles
- None detected.

## Communities (48 total, 20 thin omitted)

### Community 0 - "com.unity.2d.sprite"
Cohesion: 0.05
Nodes (44): com.unity.2d.aseprite, com.unity.2d.psdimporter, com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.2d.tilemap.extras, com.unity.modules.tilemap, com.unity.modules.tilemap, dependencies (+36 more)

### Community 1 - "Collidable"
Cohesion: 0.06
Nodes (19): Chest, Sprite, SpriteRenderer, Collectable, Collider2D, Collidable, BoxCollider2D, Collider2D (+11 more)

### Community 2 - "com.unity.mathematics"
Cohesion: 0.05
Nodes (41): com.unity.burst, com.unity.mathematics, com.unity.nuget.mono-cecil, com.unity.test-framework.performance, com.unity.test-framework, com.unity.test-framework, dependencies, depth (+33 more)

### Community 3 - "com.unity.modules.unitywebrequest"
Cohesion: 0.06
Nodes (38): com.unity.modules.assetbundle, com.unity.modules.unitywebrequest, com.unity.modules.unitywebrequestassetbundle, com.unity.modules.unitywebrequestaudio, com.unity.modules.assetbundle, com.unity.modules.unitywebrequest, com.unity.modules.unitywebrequestassetbundle, com.unity.modules.unitywebrequestaudio (+30 more)

### Community 4 - "GameManager"
Cohesion: 0.09
Nodes (12): Animator, Color, GameObject, List, RectTransform, Sprite, Vector3, GameManager (+4 more)

### Community 5 - "dependencies"
Cohesion: 0.06
Nodes (32): dependencies, depth, source, url, version, dependencies, depth, source (+24 more)

### Community 6 - "com.unity.modules.imgui"
Cohesion: 0.06
Nodes (32): com.unity.2d.pixel-perfect, com.unity.modules.hierarchycore, com.unity.modules.imgui, com.unity.modules.imgui, dependencies, depth, source, url (+24 more)

### Community 7 - "com.unity.modules.audio"
Cohesion: 0.06
Nodes (32): com.unity.modules.audio, com.unity.modules.director, com.unity.modules.particlesystem, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.particlesystem, dependencies, depth (+24 more)

### Community 8 - "com.unity.modules.physics"
Cohesion: 0.07
Nodes (29): com.unity.modules.physics, com.unity.modules.terrain, com.unity.modules.physics, com.unity.modules.terrain, dependencies, depth, source, version (+21 more)

### Community 9 - "com.unity.modules.jsonserialize"
Cohesion: 0.08
Nodes (28): com.unity.modules.subsystems, com.unity.modules.jsonserialize, com.unity.modules.jsonserialize, dependencies, depth, source, version, dependencies (+20 more)

### Community 10 - "Enemy"
Cohesion: 0.11
Nodes (12): Boss, Transform, BoxCollider2D, Collider2D, ContactFilter2D, Transform, Vector3, Enemy (+4 more)

### Community 11 - "CharacterMenu"
Cohesion: 0.12
Nodes (8): CameraMotor, Transform, CharacterMenu, RectTransform, TMP_Text, DontDestroy, Image, MonoBehaviour

### Community 12 - "FloatingText"
Cohesion: 0.14
Nodes (10): GameObject, TMP_Text, Vector3, FloatingText, Color, GameObject, List, TMP_Text (+2 more)

### Community 13 - "com.unity.modules.imageconversion"
Cohesion: 0.12
Nodes (17): com.unity.modules.imageconversion, com.unity.modules.imageconversion, dependencies, depth, source, version, dependencies, depth (+9 more)

### Community 14 - "Fighter"
Cohesion: 0.14
Nodes (7): Crate, Vector3, Damage, Collider2D, EnemyHitbox, Vector3, Fighter

### Community 15 - "com.unity.2d.spriteshape"
Cohesion: 0.13
Nodes (15): com.unity.2d.spriteshape, com.unity.modules.physics2d, com.unity.modules.physics2d, dependencies, depth, source, url, version (+7 more)

### Community 16 - "com.unity.ext.nunit"
Cohesion: 0.15
Nodes (13): com.unity.ext.nunit, dependencies, depth, source, version, dependencies, depth, source (+5 more)

### Community 17 - "com.unity.ugui"
Cohesion: 0.17
Nodes (12): com.unity.ugui, com.unity.ugui, depth, source, version, dependencies, depth, source (+4 more)

### Community 18 - "dependencies"
Cohesion: 0.25
Nodes (8): com.unity.collab-proxy, com.unity.modules.ai, com.unity.modules.unityanalytics, dependencies, com.unity.collab-proxy, com.unity.modules.ai, com.unity.modules.ui, com.unity.modules.unityanalytics

### Community 19 - "com.unity.modules.animation"
Cohesion: 0.25
Nodes (8): com.unity.modules.animation, com.unity.modules.animation, dependencies, dependencies, depth, source, version, com.unity.modules.animation

### Community 20 - "com.unity.2d.animation"
Cohesion: 0.29
Nodes (7): com.unity.2d.animation, dependencies, depth, source, url, version, com.unity.2d.animation

### Community 21 - "com.unity.2d.tooling"
Cohesion: 0.29
Nodes (7): com.unity.2d.tooling, dependencies, depth, source, url, version, com.unity.2d.tooling

### Community 22 - "com.unity.2d.common"
Cohesion: 0.33
Nodes (6): com.unity.2d.common, depth, source, url, version, com.unity.2d.common

### Community 23 - "com.unity.collections"
Cohesion: 0.33
Nodes (6): com.unity.collections, depth, source, url, version, com.unity.collections

### Community 24 - "com.unity.modules.uielements"
Cohesion: 0.33
Nodes (6): com.unity.modules.uielements, com.unity.modules.uielements, depth, source, version, com.unity.modules.uielements

### Community 25 - "com.unity.inputsystem"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.inputsystem

### Community 26 - "com.unity.multiplayer.center"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.multiplayer.center

### Community 27 - "com.unity.modules.vr"
Cohesion: 0.67
Nodes (3): com.unity.modules.vr, com.unity.modules.vr, com.unity.modules.xr

## Knowledge Gaps
- **268 isolated node(s):** `depth`, `source`, `url`, `version`, `depth` (+263 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **20 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `dependencies` connect `dependencies` to `com.unity.2d.sprite`, `com.unity.mathematics`, `com.unity.modules.unitywebrequest`, `com.unity.modules.imgui`, `com.unity.modules.audio`, `com.unity.modules.physics`, `com.unity.modules.jsonserialize`, `com.unity.modules.imageconversion`, `com.unity.2d.spriteshape`, `com.unity.ext.nunit`, `com.unity.ugui`, `com.unity.modules.animation`, `com.unity.2d.animation`, `com.unity.2d.tooling`, `com.unity.2d.common`, `com.unity.collections`, `com.unity.modules.uielements`, `com.unity.inputsystem`, `com.unity.multiplayer.center`?**
  _High betweenness centrality (0.445) - this node is a cross-community bridge._
- **Why does `dependencies` connect `dependencies` to `com.unity.2d.sprite`, `com.unity.mathematics`, `com.unity.modules.unitywebrequest`, `com.unity.modules.imgui`, `com.unity.modules.audio`, `com.unity.modules.physics`, `com.unity.modules.jsonserialize`, `com.unity.modules.imageconversion`, `com.unity.2d.spriteshape`, `com.unity.ugui`, `com.unity.modules.animation`, `com.unity.modules.uielements`, `com.unity.modules.vr`, `com.unity.feature.2d`, `com.unity.ide.rider`, `com.unity.ide.visualstudio`, `com.unity.inputsystem`, `com.unity.modules.accessibility`, `com.unity.modules.adaptiveperformance`, `com.unity.modules.androidjni`, `com.unity.modules.cloth`, `com.unity.modules.screencapture`, `com.unity.modules.terrainphysics`, `com.unity.modules.umbra`, `com.unity.modules.unitywebrequesttexture`, `com.unity.modules.unitywebrequestwww`, `com.unity.modules.vectorgraphics`, `com.unity.modules.vehicles`, `com.unity.modules.video`, `com.unity.modules.wind`, `com.unity.multiplayer.center`, `com.unity.timeline`, `com.unity.visualscripting`?**
  _High betweenness centrality (0.120) - this node is a cross-community bridge._
- **Why does `GameManager` connect `GameManager` to `Collidable`, `CharacterMenu`, `FloatingText`?**
  _High betweenness centrality (0.026) - this node is a cross-community bridge._
- **What connects `depth`, `source`, `url` to the rest of the system?**
  _268 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `com.unity.2d.sprite` be split into smaller, more focused modules?**
  _Cohesion score 0.05179704016913319 - nodes in this community are weakly interconnected._
- **Should `Collidable` be split into smaller, more focused modules?**
  _Cohesion score 0.05807200929152149 - nodes in this community are weakly interconnected._
- **Should `com.unity.mathematics` be split into smaller, more focused modules?**
  _Cohesion score 0.05121951219512195 - nodes in this community are weakly interconnected._