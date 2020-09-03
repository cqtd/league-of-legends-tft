namespace CQ.LeagueOfLegends.TFT
{
	public interface IComponent
	{
		void Initialize();
		void OnUpdate();
		void OnFixedUpdate();
	}
}