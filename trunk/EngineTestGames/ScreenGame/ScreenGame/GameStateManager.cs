using System;
using Microsoft.Xna.Framework;
using ThreeDWindowsGameLibrary.Simulation;

namespace ScreenGame
{
	public static class GameStateManager
	{
		private static bool _running = false;
		public static JitterGameManager GameManager;

		public static bool Running
		{
			get
			{
				return _running;
			}
		}
		public static void StartGame()
		{
			if (!Running || GameManager != null)
			{
				GameManager = new JitterGameManager();
				_running = true;
				GameManager.StartGame();
			}
			else
				throw new Exception("Can't start game when game is already running.");
		}

		public static void EndGame()
		{
			_running = false;
			GameManager.EndGame();
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
