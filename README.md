# 🎙️ Create Your Own Daggertalk Voice Pack

Creating a voice pack for **Daggertalk** involves the following steps:

1. ✅ Install Unity + Daggerfall Unity source code (this is the trickiest part 😀).
2. 🎤 Record all voice lines and name the files correctly.
3. 📁 Set up a folder structure in Unity for your voice pack mod and copy your recorded lines there.
4. 📓 Create a subtilte.txt file for the subtitles.
5. ⚙️ Use the `Create Voice Pack Script` to automatically generate all necessary code.
6. 🧱 Compile the mod for target platforms.
7. 📤 Upload your finished voice pack to Nexus Mods.
8. 📬 Contact *Excoriated* on Nexus Mods or Discord to link your pack on the Daggertalk Nexus page.

---

## 📦 Prerequisites

First, install:

- **Unity 2019.4.40f1**
- **Daggerfall Unity source code**

You can find setup instructions here:  
👉 [DFU Modding Wiki – Making Mods](https://dfu-modding.fandom.com/wiki/Making_Mods)

---

## 🛠️ Voice Pack Helper Script

After installing Unity:

1. Download the script: `VoicePackCreator.cs`
2. Place it inside your Unity project at:  
   `Assets/Editor`

If placed correctly, you'll find a new menu entry in Unity:  
**Daggertalk → Create Voice Pack Script** (You'll need it later)

---

## 🗂️ Create Your Mod and Audio Structure

1. In Unity, navigate to:  
   `Assets/Game/Mods/`

2. Create a new folder for your voice pack, e.g.:  
   `Assets/Game/Mods/MyVoicePack`

3. Inside this folder, create a subfolder named:  
   `audio`

🎧 Place all your recorded `.ogg` audio files in this `audio` folder.

---

## 🎙️ Recording and Naming Audio Files

Voice packs are gender-specific or gender-neutral. If you want different voices for different genders, create separate packs.

📝 **Important: Save audio files in `.ogg` Ogg Vorbis format!**

Take a look at the `VoiceLines.xlsx` in this repo. These are all the recorded voice lines for the default voice packs from Daggertalk

### 📛 File Naming Scheme

Example:  
```
m_idle_solo_u_all_8
```

Each part is separated by an underscore `_`:

| Segment       | Meaning |
|---------------|---------|
| `m`           | NPC gender (`m`, `f`, or `u` for undefined/monsters) |
| `idle`        | Action (see full list below) |
| `solo`        | NPC is alone (`solo`) or in a group (`multi`) |
| `u`           | Player gender (`m`, `f`, or `u` for any) |
| `all`         | Weapon type (e.g., `all` for any type – see below) |
| `8`           | Unique number to avoid filename conflicts (increment as needed) |

### 🎬 Supported Action Types

In this context "player" can also mean another enemy if in-fighting is activated in DFU

- `idle` – Enemy is idle
- `detect` – Enemy detected the player
- `detectagain` – Player was detected again
- `att` – Enemy is attacking
- `affirm` – Response to another enemy's attack
- `friendead` – Ally of the enemy got killed
- `playermiss` – Player missed their attack
- `playerdeath` – Player died
- `dialog` – Part of an enemy dialog (see next section)

Maybe there will come more action types in future updates. 

---

## 💬 Dialogs (Special Case)

Dialogs consist of **3 lines**:
1. Spoken by Enemy A
2. Answered by Enemy B
3. Final response from Enemy A

Refer to the example Excel provided in this repo to understand the flow and content.

⚠️ *Do not change the original Daggertalk dialog lines!*

### ➕ Adding Custom Dialogs

You can add new dialogs starting from ID `11` to avoid conflicts with core dialog IDs (1–10).  
I suggest to choose a custom range, e.G. start at `1000` and check if other mods already use this range.
❗Be sure to document your custom dialog range on your mod page!

### 📛 Dialog Naming Convention

Example:
```
m_dialog_100_123
```

| Segment       | Meaning |
|---------------|---------|
| `m`           | NPC gender (`m`, `f`, or `u`) |
| `dialog`      | Marks this line as part of a dialog |
| `100`         | Dialog ID (use 100+ for custom) |
| `123`         | Incremental number to maintain order |

---

## 📓 Subtitles

Take a look at the `subtitles.txt` file in the Daggertalk repository for reference.  
You’ll need to create your own version for your voice pack, containing all voice lines and their corresponding subtitles.

### 📄 Format
Each line should follow this format:
```
Filename=Subtitle
```

Example:
```
m_idle_solo_u_all_8=I could be drinking right now…
```

Save this file as `subtitles.txt` in the **root folder** of your mod (same level as `audio`).

---

## 📁 After Recording

1. Place all your correctly named `.ogg` audio files inside your mod's `audio` folder in Unity.
2. Select all audio files in the Unity Editor.
3. Select the root folder of you mod in Unity. 
4. Run the **Create Voice Pack Script** from the `Daggertalk` menu.
4. Fill out all the information

✨ Done! Your code and configuration will be auto-generated.

---

Now you're ready to build your mod and share it with the world! Refer to the [DFU Modding Wiki – Making Mods](https://dfu-modding.fandom.com/wiki/Making_Mods) on how to build your mod.
The script already created your .dfmod file and referenced all necessary files for you! 

❗Make sure to check `Precompile (experimental)`

Happy modding! 🎮🛠️
