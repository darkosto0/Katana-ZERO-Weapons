# Disclaimer
This is a fan-made project inspired by Katana ZERO. All rights to the original IP, assets, and sounds belong to Askiisoft and Devolver Digital. No profit is made from this mod. If requested, I will respectfully take it down.

# Katana ZERO Weapons
Terraria mod that aims to add all 7 katanas from Katana ZERO into Terraria as their own weapons.

Current mod status: Russian and Italian translations will be updated soon

[Steam Workshop Link](https://steamcommunity.com/sharedfiles/filedetails/?id=3289273898)

# Weapons Added:
- ZERO's Katana: The original, no special effects, just bullet deflection 
(Early Pre-Hardmode)

- Prism Sword: Slightly increased stats from the original, with a colorful slash 
(Late Pre-Hardmode)

- Sword of Masters: Slower attack speed, increased range, shoots a projectile, more hits per slash (Post-Destroyer)

- Savant Knife: Faster attack speed, low range, high amount of hits per slash (Post-Mechs)

- Claymore Prototype: Slow attack speed, big hit range and width, unlimited amount of bullet deflections per swing (Post-Golem)

- Phoenix's Edge: Slow attack speed, very high range, shoots flames that burn enemies (Post-Cultist)

- Fifteen's Blade: High attack speed, very high reach, teleports to the cursor on holding and releasing right click, meant to kill most enemies in one hit (Legendary Weapon)
	- Has 1/40000 chance to drop from any hostile enemy, or 1/9 to drop from Moon Lord.

# Extra Info
- This mod is made by me in attempt to learn the ropes of programming, there might be bugs I haven't found yet that could break the mod.
- If you are more experienced and want to help me make this mod better, send me a DM on Discord (darkosto0) and I'll gladly reach back.
- I plan on maintaining the mod as long as I receive feedback.

# Special Thanks To:
- [g113g](https://github.com/g113g) for a Russian translation, code help and for uploading the mod to Steam Workshop for me.
- [Sequile](https://github.com/Sequile) for code help and balancing the weapons.
- [ChrisGug](https://steamcommunity.com/id/ChrisGug) for an Italian translation.

# Known Issues
- WeaponOut Lite mod visual bug
- TBD...

# Changelog
## **v1.3.0**
**Bug Fixes**
- 🛠️ Fixed logic with **Dragon Dash character** staying in place.
- 🛠️ Fixed the config so now it is **hot swappable**.
- 🛠️ Fixed bug where **Eternian Portals get killed by Dragon Dash**.

**Changes**
- 🔧 Fixed **inconvenience** where opening a chest does **Dragon Dash when using Dragon's Whisper**.

**Balance Changes**
- ⚖️ Added **mana usage** when using Dragon Dash with **time slow-down**.
	- **No mana is used** when slow-down is turned off in configs.

**Config Options**
- ⚙️ Added toggle to **enable scaling damage** on **all katanas** depending on bosses killed.
- ⚙️ Dragon's Whisper **1/40000 drop** can now be **toggled off** in configs.

**Localization**
- 🌎 Re-done Russian and Italian(soon) Translation

**Code**
- 🧬 **Refactored** and **optimized** the code used for **item damage scaling**.

## **v1.2.3**
**Bug Fixes**:
- 🛠️ Fixed **multiple multiplayer bugs** regarding Dragon's Whisper:
	- Dragon Dash circle getting created for every player.
	- Enemy **slowdown not working**.

**Balance Changes**:
- ⚖️ Buffed EVERY hardmode sword's damage by 25%

## **v1.2.2**
**New Features**:
- ✨ Added trails for Dragon Dash: **Default**, **Fireflies** or **Disabled**.

**Changes**:
- 🔧 Sword of Masters' projectile on collision now **spawns its dust** with **velocity** to it.

**Bug Fixes**:
- 🛠️ Added a **baseline** for how many **immunity frames** you get if you do a **short Dragon Dash** (30 frames/half a second).
- 🛠 Changed the **size of Dragon Whisper's right click dust**, now making a **smooth circle and trajectory** all around.

**Config Options**:
- ⚙️ Added **configuration for sound effects for every sword**.
- ⚙️ Added option to **disable Dragon Dash trail**, change it to Firefly dust, or keep it at default.
- ⚙️ Added **more headers** in the config for better organization and look.

## **v1.2.1**
**Bug Fixes**:
- 🛠️ Fixed bug with Fifteen's Blade that allowed **infinite Dragon Dash triggers**.

## **v1.2.0**
**Rework**:
- ✨ Completely reworked **Fifteen's Sword slowdown**:
	- Enemies and projectiles now move at **1/4 of their original speed**.
	- Speed is restored to original values (most of the time).
	
**Config Options**:
- ⚙️ Added configuration for **vector-based knockback**.
- ⚙️ Added option to disable **Fifteen's Sword slowdown**.

**Balance Changes**:
- ⚖️ Adjusted knockback for all weapons.

## **v1.1.6**
**Config Options**:
- ⚙️ Added configuration for the **lunge mechanic**.

## **v1.1.5**
**Localization**:
- 🌎 Added an Italian translation.

## **v1.1.4**
**Bug Fixes**:
- 🛠️ Fixed Prism Sword recipe **requiring Mythril Anvil** in pre-hardmode.

## **v1.1.3**
**Bug Fixes**:
- 🛠️ Item use doesn't break with **Fargos Souls Mod** anymore.

## **v1.1.2**
**Bug Fixes**:
- 🛠️ Fixed damage formula for **Dragon Dash**, it should do 5% of max HP now.

## **v1.1.1**
**Bug Fixes**:
- 🛠️ Fixed major **issues with boss scaling**, now it works with **vanilla, calamity and thorium**.

## **v1.1.0**
**Rework**:
- ✨ Reworked Fifteen's Blade:
	- Removed **crafting recipe**.
	- Has 1/40000 chance to **drop from hostile enemies**, and 1/9 chance from **Moon Lord**.
	- It's power now scales off **the strongest boss defeated** in the world, compatible with **Calamity and Thorium**.
	- Dragon Dash damage to enemies **reduced to 5x Item's damage**.
	- Dragon dash damage to bosses **reduced to 10x + 5% of Max HP**.
	- Dragon Dash no longer goes **through solid walls**.

## **v1.0.1**
**New Additions**:
- ✨ Finished **Fifteen's Blade mechanics**.
