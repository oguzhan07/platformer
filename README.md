# Platformer

A 2D platformer game I am building with Unity 6 and C#. This project is **work in progress**:
the core player, enemy and pickup systems work, but the game is not finished yet.

## What works now

**Player** — moving, jumping with a ground check, guarding, and two different attack animations
chosen at random. Health is shown on a bar that animates with DOTween. Attacks use an overlap
check against an enemy layer mask, so only objects on that layer can be hit.

**Enemies** — enemy stats are not written in the enemy script. Every enemy reads its name, damage,
health, move speed, jump speed, follow distance and attack distance from an `EnemyType`
ScriptableObject. This means a new enemy type is a new asset file in the editor, not new code.
Enemies patrol between two empty points, follow the player inside the follow distance, and shoot
an arrow prefab when the player is inside the attack distance.

**Pickups and UI** — collectable gold, a coin counter with TextMeshPro, and music / info buttons
in the menu.

Follow and attack distances are drawn with `OnDrawGizmos`, so the ranges are visible in the
scene view while tuning them.

## Code layout

```
Assets/Project/Scripts/
├── Player.cs           movement, jump, guard, attack, health
├── Enemy/Enemy.cs      patrol, follow, ranged attack
├── Enemy/EnemyType.cs  ScriptableObject with enemy stats
├── Gold.cs             collectable pickup
├── UiManager.cs        coin counter
└── Buttons/            music and info buttons
```

## Built with

Unity 6 (6000.3.15f1) · C# · 2D physics with Rigidbody2D · DOTween · TextMeshPro

## Not done yet

Level design, sound, save system, and a real game loop (win / lose states) are still missing.
