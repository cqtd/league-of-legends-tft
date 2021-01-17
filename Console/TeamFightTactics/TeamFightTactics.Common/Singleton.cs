namespace TeamFightTactics.Common
{
	public abstract class Singleton<T> where T : class, new()
	{
		private static T instance;

		public static T Instance {
			get
			{
				if (instance == default)
				{
					instance = new T();
				}

				return instance;
			}
		}
	}
}