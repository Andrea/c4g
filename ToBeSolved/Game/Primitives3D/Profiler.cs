using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives3D
{
	public class Profiler
	{
		private static Dictionary<string, ProfileData> _profiles;

		static Profiler()
		{
			_profiles = new Dictionary<string, ProfileData>();
		}

		public static void Start(string profilingName)
		{
			ProfileData profileData;
			if(_profiles.TryGetValue(profilingName, out profileData) == false)
			{
				profileData = new ProfileData {Stopwatch = new Stopwatch(), Name = profilingName};
				_profiles.Add(profilingName, profileData);
			}

			profileData.Stopwatch.Start();
		}

		public static void End(string profilingName)
		{
			var profileData = _profiles[profilingName];
			profileData.Stopwatch.Stop();
			profileData.ElapsedTime = profileData.Stopwatch.ElapsedTicks;
		}

		public static void DrawStats(SpriteBatch spriteBatch, SpriteFont spriteFont)
		{
			var output = "";
			foreach (var profileData in _profiles)
			{
				output += profileData.Value.Name + ":" + (((float)profileData.Value.ElapsedTime / Stopwatch.Frequency)*1000).ToString("0.000000") + "ms\n";
			}

			spriteBatch.DrawString(spriteFont, output, new Vector2(20, 30), Color.White);
		}

		public static void Clear()
		{
			foreach (var profileData in _profiles)
			{
				profileData.Value.ElapsedTime = 0;
				profileData.Value.Stopwatch.Reset();
			}
		}

		private class ProfileData
		{
			public Stopwatch Stopwatch { get; set; }
			public long ElapsedTime { get; set; }
			public string Name { get; set; }
		}
	}
}