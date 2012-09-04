using Microsoft.Xna.Framework;
using ThreeDWindowsGameLibrary.Simulation;

namespace ScreenGame
{
	public static class GameStateManager
	{
		public static bool Running = false;
		public static JitterGameManager GameManager;

		public static void StartGame(JitterGameManager gameManager)
		{
			GameManager = gameManager;
			Running = true;
		}

		#region Update
		public static void Update(GameTime gameTime)
		{
			if (Running && GameManager != null)
				GameManager.Update(gameTime);
		}
		#endregion
	}
}
