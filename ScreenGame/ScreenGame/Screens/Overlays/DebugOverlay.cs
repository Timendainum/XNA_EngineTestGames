using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ClientWindowsGameLibrary.ScreenManagement.Overlays;
using ClientWindowsGameLibrary.ScreenManagement;
using ThreeDWindowsGameLibrary.Cameras;

namespace ScreenGame.Screens.Overlays
{
	public class DebugOverlay : Overlay
	{
		private float fps;
		private float ups;
		private Vector2 mousePosition;
		private PlayGameScreen _screen;
		float elapsed;


		public DebugOverlay(PlayGameScreen screen)
			: base((GameScreen)screen)
		{
			_screen = screen;
			ScreenPosition = new Vector2(10, 10);
			Width = 600;
			Height = 210;
		}


		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			if (!IsActive)
				return;
			
			//FPS Counter
			elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
			fps = 1 / elapsed;
			spriteBatch.DrawString(_screen.GetScreenContentManager().GetFont("kootenay14"), String.Format("FPS: {0} UPS: {1}", fps, ups), TransformOverlayToScreen(new Vector2(10, 10)), Color.White);

			FreeCamera cam = (FreeCamera)_screen.GetCamera();
			spriteBatch.DrawString(_screen.GetScreenContentManager().GetFont("kootenay14"), String.Format("Camera Position: X:{0} Y:{1} Z:{2}",cam.Position.X, cam.Position.Y, cam.Position.Z), TransformOverlayToScreen(new Vector2(10, 30)), Color.White);
			spriteBatch.DrawString(_screen.GetScreenContentManager().GetFont("kootenay14"), String.Format("Camera Orientation: Yaw:{0} Pitch:{1}", cam.Yaw, cam.Pitch), TransformOverlayToScreen(new Vector2(10, 50)), Color.White);
		}

		public override void HandleInput(InputState input)
		{
			//Debug toggle --------------------------------------------------------------------
			if (input.IsNewKeyPress(Keys.OemTilde))
				ToggleActive();

			mousePosition = new Vector2(input.CurrentMouseState.X, input.CurrentMouseState.Y);
		}

		public override void Update(GameTime gameTime)
		{
			if (!IsActive)
				return;

			elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
			ups = 1 / elapsed;
		}

	}
}
