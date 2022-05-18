# RPG

Unity 2018.4.1f1
https://unity3d.com/get-unity/download/archive

This project was made during this Udemy course: https://www.udemy.com/course/unityrpg

Implements fundamental knowledge of:
- Unity Programming design (component style, modularity, C# interfaces, C# delegates and events, serializing, unity events vs C# events, inspector hook-ups/dependencies vs component search intialization)
- UI (anchoring, pivoting, layout grouping, hierarchy organization, content size fitting, etc)
- Save System
- Combat System (health, damage, attack conditions, weapon configs, ranged vs melee, animations, stat modifiers)
- AI Systems (patrolling, aggrevation, mob aggrevation)
- Root Character, Weapon Pickup, other Game instance implementations (nested prefabs, prefab variants)
- Cinematics (cinemachine, playable directors)
- Stats and Progression (all characters deriving from a game designer's reference sheet on a ScriptableObject)
- Scene Management (storing and restoring state between scenes, persistent objects)
- Movement (navmesh, pathing)
- Cursor visuals (Raycasting, Spherecasting, 2D Textures, Raycast sorting)
- Basic Level design (Terrain creation, foliage placement, prop placement, texture painting)

To-do:
- Change serializer from BinaryFormatter to a json serializer
- Refactor out Lazy Intialization
