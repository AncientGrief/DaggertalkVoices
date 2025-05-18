This Repository is for Daggerfall Unity mod authors, that want to create their own voice packs for the [Daggertalk mod](https://www.nexusmods.com/daggerfallunity/mods/1037)
# 🎙️ Create Your Own Daggertalk Voice Pack

Creating a voice pack for **Daggertalk** involves the following steps:

1. ✅ Install Unity + Daggerfall Unity source code (this is the trickiest part 😀).
2. 🎤 Record all voice lines and name the files correctly.
3. 📁 Set up a folder structure in Unity for your voice pack mod and copy your recorded lines there.
4. 📓 Create a subtitles.txt file for the subtitles.
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

1. Download the script: [VoicePackCreator.cs](https://github.com/AncientGrief/DaggertalkVoices/blob/main/VoicePackCreator.cs) (`Download raw file` in the top right of the text window)
2. Place it inside your Unity project at:  
   `Assets/Editor`

If placed correctly, you'll find a new menu entry in Unity:  
**Daggertalk → Create Voice Pack Script** (You'll need it later)

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

Take a look at the [VoiceLines.md](https://github.com/AncientGrief/DaggertalkVoices/blob/main/Voicelines.md) in this repo. These are all the recorded voice lines for the default voice packs from Daggertalk.
You can choose to strictly record the vanilla Daggertalk lines in your voice, add more lines for each action type or completley record new lines.
You don't have to follow the vanilla scripts, except for the 10 dialogs. 

If you choose to support these, keep to the script or at least change them in a way that these will make sense when a vanilla voice is choosen against your custom voice 😄
Feel free to add new Dialogs, but keep in mind that no vanilla voice will respond to these.

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

Take a look at the [subtitles.txt](https://github.com/AncientGrief/DaggertalkVoices/blob/main/subtitles.txt) file in this repository for reference.  
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

## 👾 Customize Enemy Usage

In the generated script, you’ll find a section that defines **which enemies** can use your voice pack:

```csharp
// Enemy IDs that can use this voice pack
private HashSet<int> _enemyIds = new HashSet<int>()
{
    // Vanilla Humans
    128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146

    // DEX Humans
    , 384, 385, 386, 387, 388, 389, 390, 391, 392, 393, 394, 395, 396, 397, 398
};
```

If you want your voice pack to be used by **non-human enemies** (like monsters, undead, Daedra, etc.), you need to **replace or extend** this list with the appropriate enemy IDs.
Make sure the last number isn't followed by a comma.

### 📑 Reference Tables

Use these references to find the correct enemy IDs:

- ✅ **Vanilla Daggerfall Unity enemy IDs:**  
  👉 [Quests-Foes.txt on GitHub](https://github.com/Interkarma/daggerfall-unity/blob/master/Assets/StreamingAssets/Tables/Quests-Foes.txt)

- ✅ **DEX modded enemies (Daggerfall Enemy Expander):**  
  👉 [DEX MonsterBase.mdb.csv](https://github.com/SquidKamer/DaggerfallBestiaryProject/blob/main/MonsterBase.mdb.csv)

If you want to create a voice pack for rats, it would look like this:
```csharp
// Enemy IDs that can use this voice pack
private HashSet<int> _enemyIds = new HashSet<int>()
{
   // Rats
   0
};
```

or for Nymphs:
```csharp
// Enemy IDs that can use this voice pack
private HashSet<int> _enemyIds = new HashSet<int>()
{
   //Vanilla Nymph
   10
   
   //DEX Nymph
   , 268
};
```

Daggertalk also provides an option to silence the default monster noises and add an optional skill check for a language skill.
See at the end for further information.

---

**Make sure to test the mod in Unity directly, hit the play button, activate Daggertalk and you custom voice pack only and see if everything works fine in game**

If everything is working => you're ready to build your mod and share it with the world! Refer to the [DFU Modding Wiki – Making Mods](https://dfu-modding.fandom.com/wiki/Making_Mods) on how to build your mod.
The script already created your .dfmod file and referenced all necessary files for you! 

❗Make sure to check `Precompile (experimental)` in the Mod Builder!

---

## 🧠 Optional: Add Skill Check and Silence Default Sounds

Daggertalk also provides an option to:

- 🧏 **Silence the default monster sounds**
- 📚 **Require a minimum language skill** to understand the voice pack

This is useful if you're assigning voices to **non-humanoid creatures** (e.g. Nymphs, Daedra, Imps), and want to add roleplay depth or immersion.

### ✍️ How to Use It in Code

In your script, where you register the voice pack like this:

```csharp
Debug.Log("Registering VoicePack: " + VoicePackId + " for " + VoiceGender + " in Daggertalk");
ModManager.Instance.SendModMessage(daggertalk.Title, "RegisterVoicePack", data);
```

👉 You can **optionally** add the following right after:

```csharp
// Add a skill requirement (e.g. Nymph language skill)
var skillData = new Tuple<string, string, short>(VoicePackId, nameof(DFCareer.Skills.Nymph), 30); //Needs at least skill level 30
ModManager.Instance.SendModMessage(daggertalk.Title, "SetVoicePackSkill", skillData, null);

// Mute the default monster sounds
var silenceData = new Tuple<string, bool>(VoicePackId, true);
ModManager.Instance.SendModMessage(daggertalk.Title, "SilenceDefaultVoice", silenceData, null);
```

### 📚 Available Skills

You can find a list of all language skills in the Daggerfall Unity codebase here:  
👉 [DFCareer Skills Enum](https://github.com/Interkarma/daggerfall-unity/blob/bf530712ba6caee8f12939b0266ecc234ed2f388/Assets/Scripts/API/DFCareer.cs#L448)

Simply replace the `Nymph` name in `DFCareer.Skills.Nymph` with the corresponding skill name.

Example language skills:
- `Spriggan`
- `Nymph`
- `Daedric`
- `Giantish`
- `Orcish`
- `Dragonish`
- `Harpy`
- etc.

---

By using these options, you can create more immersive and lore-friendly voice packs that respect player progression and character skills.

Happy modding! 🐉🗣️🎮
