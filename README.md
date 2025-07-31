# Pinkman Shooter!

A shooter game based on Speech of Jan Willem Nijman - Vlambeer - "The art of screenshake" at INDIGO Classes 2013.

## 🎮 Features

- 🔫 **Weapon System**
  - Modular weapon base class (`Weapon.cs`)
  - Semi-auto and auto weapons (e.g., `Pistol`)
  - Muzzle flash with customizable prefab
  - Recoil-based camera shake when shooting

- 🎯 **Aiming & Shooting**
  - Mouse-based directional aiming
  - Support for bullet spread
  - Fire rate and cooldown system

- 👤 **Gun Controller**
  - Controls weapon holding, aiming direction, and facing
  - Automatically equips default weapon on start
  - Supports weapon switching via `EquipWeapon()`

- 📸 **Camera Effects**
  - Lightweight camera shake system (`CameraShake.cs`)
  - Easily callable from any script: `CameraShake.Instance.Shake()`
  - Local shake offset resets automatically

### 3. Controls

| Action         | Key / Mouse            |
|----------------|------------------------|
| Shoot          | Left Click             |
| Aim            | Move Mouse             |
| Move Player    | WASD / Arrow Keys      |
| Equip Weapon   | Call `GunController.EquipWeapon()` in script |

## 🔧 Future Plans

- Add multiple weapon types (shotgun, rifle, etc.)
- Enemy AI
- Cinemachine integration for advanced camera control
- Object pooling for performance optimization

## 👨‍💻 Author

- **Yu Peng**  
  Discord: `在下有把木剑`
