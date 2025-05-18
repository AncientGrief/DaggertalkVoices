using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DaggerfallWorkshop;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Dependency
{
    public string Name;
    public bool IsOptional;
    public bool IsPeer;
}

[Serializable]
public class ModInfo
{
    public string ModTitle;
    public string ModVersion;
    public string ModAuthor;
    public string ContactInfo;
    public string DFUnity_Version;
    public string ModDescription;
    public string GUID;
    public List<string> Files;

    public List<Dependency> Dependencies = new List<Dependency>()
    {
        new Dependency
        {
            Name = "daggertalk",
            IsOptional = false,
            IsPeer = false
        }
    };
}

public class VoicePackCreator : EditorWindow
{
    private string _modName = "Voice Pack XYZ";
    private string _className = "";
    private string _author = "";
    private string _version = "1.0.0";
    private string _contact = "";

    private MobileGender _selectedGender = MobileGender.Male;

    [MenuItem("Daggertalk/Create Voice Pack Script")]
    public static void ShowWindow()
    {
        VoicePackCreator window = GetWindow<VoicePackCreator>("Voice Pack Creator");
        window.minSize = new Vector2(400, 250);
        window.maxSize = new Vector2(400, 250);
    }

    private void OnGUI()
    {
        GUILayout.Label("Enter Mod Name", EditorStyles.boldLabel);
        _modName = EditorGUILayout.TextField("Mod Name:", _modName);
        _className = _modName.Replace(" ", string.Empty);

        GUILayout.Label("Enter Mod Info", EditorStyles.boldLabel);
        _author = EditorGUILayout.TextField("Mod Author:", _author);
        _version = EditorGUILayout.TextField("Mod Version:", _version);
        _contact = EditorGUILayout.TextField("Mod Contact:", _contact);

        GUILayout.Label("Select Voice Gender", EditorStyles.boldLabel);
        _selectedGender = (MobileGender)EditorGUILayout.EnumPopup("Voice Gender:", _selectedGender);

        if (GUILayout.Button("Create Script"))
        {
            CreateVoicePackScript(_className, _selectedGender);
        }
    }

    private static string SplitCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return Regex.Replace(input, "(?<=[a-z])([A-Z])", " $1");
    }

    private void CreateVoicePackScript(string className, MobileGender gender)
    {
        string folderPath = GetCurrentProjectWindowPath();
        if (string.IsNullOrEmpty(folderPath))
        {
            EditorUtility.DisplayDialog("Error", "Please open a valid folder in the Project window.", "OK");
            return;
        }

        string filePath = Path.Combine(folderPath, $"{className}.cs");
        string subtitlesPath = Path.Combine(folderPath, "subtitles.txt");

        if (File.Exists(filePath))
        {
            EditorUtility.DisplayDialog("Error", "File already exists!", "OK");
            return;
        }

        if (!File.Exists(subtitlesPath))
        {
            EditorUtility.DisplayDialog("Error", "Please provide a subtitles.txt!", "OK");
            return;
        }

        string classNameLower = className.ToLower();
        string randomId = GenerateRandomId(22);
        string genderString = gender.ToString();
        string audioDictionaryData = CreateAudioDictionaryData(subtitlesPath);

        string scriptContent = GetScriptTemplate()
            .Replace("{voicepackname}", className)
            .Replace("{voicepackname_lower}", classNameLower)
            .Replace("{22char_randomletters}", randomId)
            .Replace("{VoiceGender}", genderString)
            .Replace("{audiodictionary}", audioDictionaryData);

        File.WriteAllText(filePath, scriptContent, Encoding.UTF8);
        AssetDatabase.Refresh();

        ModInfo modInfo = new ModInfo
        {
            ModTitle = "Daggertalk - " + SplitCamelCase(className),
            ModVersion = _version,
            ModAuthor = _author,
            ContactInfo = _contact,
            DFUnity_Version = "1.1.1",
            ModDescription = "Voice pack for Daggertalk",
            GUID = Guid.NewGuid().ToString(),
            Files = new List<string>()
        };

        modInfo.Files.Add(filePath.Replace("\\", "/"));

        string[] audioFiles = Directory.GetFiles(Path.Combine(folderPath, "audio"));
        foreach (string f in audioFiles)
        {
            if(f.EndsWith(".meta"))
                continue;

            modInfo.Files.Add(f.Replace("\\", "/"));
        }

        string json = JsonUtility.ToJson(modInfo, true);
        string path = Path.Combine(folderPath, "daggertalk_" + _modName.Replace(" ", "_").ToLower() + ".dfmod.json");
        File.WriteAllText(path, json);

        EditorUtility.DisplayDialog("Success", "Voice Pack Script created successfully!", "OK");
    }

    private string CreateAudioDictionaryData(string subtitleFilePath)
    {
        string[] lines = File.ReadAllLines(subtitleFilePath);

        var sb = new StringBuilder();
        foreach (string line in lines)
        {
            string[] parts = line.Split('=');
            if (parts.Length == 2)
            {
                string audioFileName = parts[0].Trim();
                string subtitleText = parts[1].Trim();
                sb.AppendLine($"        {{\"{audioFileName}\",\"{subtitleText}\"}},");
            }
        }

        if (sb.Length > 0)
            sb.Length -= 3; // Remove last comma and newline

        return sb.ToString();
    }

    private string GetCurrentProjectWindowPath()
    {
        string path = "Assets"; // Default-Fallback
        Type projectWindowUtilType = typeof(ProjectWindowUtil);
        System.Reflection.MethodInfo method = projectWindowUtilType.GetMethod("GetActiveFolderPath",
            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

        if (method != null)
        {
            object result = method.Invoke(null, null);
            if (result is string resultPath)
            {
                path = resultPath;
            }
        }

        return path;
    }

    private string GenerateRandomId(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        System.Random random = new System.Random();
        char[] result = new char[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = chars[random.Next(chars.Length)];
        }

        return new string(result);
    }

    private string GetScriptTemplate()
    {
        return @"using System;
using System.Collections.Generic;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using UnityEngine;

public class {voicepackname} : MonoBehaviour
{
    // VoicePack ID - Add some kind of unique ID at the end
    private const string VoicePackId = ""{voicepackname_lower}-{22char_randomletters}"";

    // Gender of the voice in the pack
    private const MobileGender VoiceGender = MobileGender.{VoiceGender};

    // Enemy IDs that can use this voice pack
    private HashSet<int> _enemyIds = new HashSet<int>()
    {
        //Vanilla Humans
        128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146

        //DEX Humans
        , 384, 385, 386, 387, 388, 389, 390, 391, 392, 393, 394, 395, 396, 397, 398
    };

    #region Setup

    private static Mod mod;
    private static GameObject modGameObject;

    [Invoke(StateManager.StateTypes.Start, 0)]
    public static void Init(InitParams initParams)
    {
        mod = initParams.Mod;
        modGameObject = new GameObject(mod.Title, typeof({voicepackname}));
    }

    #endregion

    #region Start

    private void Start()
    {
        Mod daggertalk = ModManager.Instance.GetModFromGUID(""be4fe1b9-7c97-4755-a6c5-47a7ca9dce12"");
        if (daggertalk == null)
            return;

        var voiceLines = new List<Tuple<AudioClip, string, MobileGender>>();

        try
        {
            // Setup voice lines + subtitles
            foreach (var ac in _audioAndSubtitles)
            {
                AudioClip c = mod.GetAsset<AudioClip>(ac.Key);
                voiceLines.Add(new Tuple<AudioClip, string, MobileGender>(c, ac.Value, VoiceGender));
            }
        }
        catch (ArgumentException e)
        {
            Debug.LogError(""Daggertalk: "" + e.Message);
            return;
        }

        // Setup data to send to Daggertalk
        var data = new Tuple<string, MobileGender, HashSet<int>, List<Tuple<AudioClip, string, MobileGender>>>
            (VoicePackId, VoiceGender, _enemyIds, voiceLines);

        Debug.Log(""Registering VoicePack: "" + VoicePackId + "" for "" + VoiceGender + "" in Daggertalk"");
        ModManager.Instance.SendModMessage(daggertalk.Title, ""RegisterVoicePack"", data);
    }

    #endregion

    #region Audio + Subtitles
    private readonly Dictionary<string, string> _audioAndSubtitles = new Dictionary<string, string>()
    {

{audiodictionary}

    };
    #endregion
}";
    }
}
