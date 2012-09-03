using ClientWindowsGameLibrary.ScreenManagement;
using EngineGameLibrary.Maths;
using Microsoft.Xna.Framework;
using ScreenGame.Screens;

namespace JitterGame
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class JitterGame : Game
	{
		#region Fields

		GraphicsDeviceManager graphics;
		ScreenManager screenManager;


		// By preloading any assets used by UI rendering, we avoid framerate glitches
		// when they suddenly need to be loaded in the middle of a menu transition.
		static readonly string[] preloadAssets =
		{
			@"Textures\gradient",
		};


		#endregion

		#region Initialization


		/// <summary>
		/// The main game constructor.
		/// </summary>
		public JitterGame()
		{
			RandomManager.Init();

			IsMouseVisible = true;
			Content.RootDirectory = "Content";

			Window.AllowUserResizing = true;

			graphics = new GraphicsDeviceManager(this);

			graphics.PreferredBackBufferWidth = 1024;
			graphics.PreferredBackBufferHeight = 720;

			// Create the screen manager component.
			screenManager = new ScreenManager(this);

			Components.Add(screenManager);

			// Activate the first screens.
			screenManager.AddScreen(new BackgroundScreen());
			screenManager.AddScreen(new MainMenuScreen());
		}

		/// <summary>
		/// Loads graphics content.
		/// </summary>
		protected override void LoadContent()
		{

			foreach (string asset in preloadAssets)
			{
				Content.Load<object>(asset);
			}
		}
		#endregion

		#region Draw


		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.Black);

			// The real drawing happens inside the screen manager component.
			base.Draw(gameTime);
		}
		#endregion
	}
}
