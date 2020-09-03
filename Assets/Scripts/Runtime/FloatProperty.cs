using System;

namespace CQ.LeagueOfLegends.TFT
{
	[Serializable]
	public class FloatProperty
	{
		public float defaultValue;
		public float valuePerLevel;
		public bool useValuePerLevel;
		public float[] definedValues;
		int level = -1;

		public float Get(int level)
		{
			if (useValuePerLevel)
			{
				return defaultValue + valuePerLevel * level;
			}

			return definedValues[level];
		}

		public void SetLevel(int level)
		{
			this.level = level;
		}
		
		public static implicit operator FloatProperty(float defaultValue)
		{
			FloatProperty inst = new FloatProperty(defaultValue);
			return inst;
		}
		
		public static implicit operator FloatProperty((float defaultValue, float perLevel) tuple)
		{
			FloatProperty inst = new FloatProperty(tuple.defaultValue)
			{
				valuePerLevel = tuple.perLevel,
			};
			
			return inst;
		}

		public static explicit operator float(FloatProperty property)
		{
			if (property.level >= 0)
				return property.Get(property.level);
			else
				return property.Get(1);
		}

		FloatProperty(float defaultValue)
		{
			this.defaultValue = defaultValue;
			
			this.valuePerLevel = defaultValue * 0.2f;
			this.useValuePerLevel = true;
			this.definedValues = new[]
			{
				defaultValue,
				defaultValue * 1.2f,
				defaultValue * 1.4f,
				defaultValue * 1.6f,
			};
		}
	}
}