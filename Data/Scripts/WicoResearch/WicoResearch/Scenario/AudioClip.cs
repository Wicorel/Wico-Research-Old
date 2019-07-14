using System.Collections.Generic;
using Duckroll;
using VRage.Game;

namespace WicoResearch
{
	internal class Speaker
	{
        //https://www.youtube.com/watch?v=e4Be2itSVWI

        // https://translate.google.com/  Echo delay 0.05 Decay 0.5
        internal static readonly Speaker Research = new Speaker("Research", MyFontEnum.Green);
		internal static readonly Speaker None = new Speaker("None", MyFontEnum.BuildInfo);

// radio sound  https://www.youtube.com/watch?v=YantpouC4Mk

		internal string Name { get; }
		internal MyFontEnum Font { get; }

		private Speaker(string name, MyFontEnum font)
		{
			Font = font;
			Name = name;
		}
	}

	public class AudioClip : IAudioClip
	{
		private static int _nextId = 1;
		private static readonly Dictionary<int, AudioClip> Index = new Dictionary<int, AudioClip>();

		internal static readonly AudioClip OxygenGeneratorUnlocked = Create("OxygenGeneratorUnlocked", WicoResearch.Speaker.Research,
			"Oxygen generator technology unlocked!", 2000);

		internal static readonly AudioClip UnlockAtmospherics = Create("UnlockAtmospherics", WicoResearch.Speaker.Research, 
            "Atmospheric thruster technology unlocked!", 2000);

		internal static readonly AudioClip UnlockedMissiles = Create("UnlockedMissiles", WicoResearch.Speaker.Research,
			"Missile technology unlocked!", 2000);

        internal static readonly AudioClip GasStorageUnlocked = Create("GasStorageUnlocked", WicoResearch.Speaker.Research,
            "Gas storage technology unlocked.", 2000);

        internal static readonly AudioClip OxygenFarmUnlocked = Create("OxygenFarmUnlocked", WicoResearch.Speaker.Research,
			"Oxygen Farm technology unlocked!", 2000);

        internal static readonly AudioClip BasicWeaponsUnlocked = Create("BasicWeaponsUnlocked", WicoResearch.Speaker.Research,
            "Basic Weapon technology unlocked!", 2000);

		internal static readonly AudioClip AllTechUnlocked = Create("AllTechUnlocked", WicoResearch.Speaker.Research,
	        "All technologies unlocked.", 2000);

        // uncategorized technology unlocked
        internal static readonly AudioClip TechUnlocked = Create("TechUnlocked", WicoResearch.Speaker.Research,
            "Technology unlocked.", 2000);

        // Research UI sounds.
        internal static readonly AudioClip HackingSound = Create("HackingSound", WicoResearch.Speaker.None,	"", 14000);

		internal static readonly AudioClip ConnectionLostSound = Create("ConnectionLostSound", WicoResearch.Speaker.None, "", 2000);

		internal static readonly AudioClip HackFinished = Create("HackFinished", WicoResearch.Speaker.None, "", 2000);


        private static AudioClip Create(string subTypeName, Speaker speaker, string subtitle, int disappearTimeMs = 4200)
		{
			var id = _nextId++;
			var clip = new AudioClip(id, subTypeName, speaker, subtitle, disappearTimeMs);
			Index.Add(id, clip);
			return clip;
		}

		public static AudioClip GetClipFromId(int id)
		{
			return Index[id];
		}

		public string Filename { get; }
		public string Subtitle { get; }
		public string Speaker { get; }
		public MyFontEnum Font { get; }
		public int DisappearTimeMs { get; }
		public int Id { get; }

		private AudioClip(int id, string filename, Speaker speaker, string subtitle, int disappearTimeMs)
		{
			Id = id;
			DisappearTimeMs = disappearTimeMs;
			Speaker = speaker.Name;
			Font = speaker.Font;
			Filename = filename;
			Subtitle = subtitle;
		}
	}
}

