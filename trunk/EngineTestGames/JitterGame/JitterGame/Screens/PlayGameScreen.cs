using System;
using System.Collections.Generic;
using System.Linq;
using ClientWindowsGameLibrary.ScreenManagement;
using ThreeDWindowsGameLibrary.Cameras;
using ThreeDWindowsGameLibrary.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThreeDWindowsGameLibrary.Actors.Materials;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace JitterGame.Screens
{
	public class PlayGameScreen : GameScreen
	{
		private const float CAMERA_MOVE_FACTOR = 0.005f;
		ContentManager Content;
		FreeCamera Cam;

		List<BasicActor> Actors = new List<BasicActor>();

		public override void LoadContent()
		{
			Cam = new FreeCamera(new Vector3(1000, 600, -2000),
			MathHelper.ToRadians(153),
			MathHelper.ToRadians(-5),
			ScreenManager.GraphicsDevice);
			
			if (Content == null)
				Content = new ContentManager(ScreenManager.Game.Services, "Content");

			// TODO: use this.Content to load your game content here
			Actors.Add(new BasicActor(Content.Load<Model>(@"Models\ship"), new Vector3(0f, 300f, 0f), Vector3.Zero, Vector3.One, ScreenManager.GraphicsDevice));
			Actors.Add(new BasicActor(Content.Load<Model>(@"Models\Ground"), Vector3.Zero, Vector3.Zero, Vector3.One, ScreenManager.GraphicsDevice));

			Effect simpleEffect = Content.Load<Effect>(@"Models\LightingEffect");
			Actors[0].SetModelEffect(simpleEffect, true);
			Actors[1].SetModelEffect(simpleEffect, true);

			LightingMaterial mat = new LightingMaterial();
			//mat.LightColor = Color.Red.ToVector3();
			mat.AmbientColor = new Vector3(0.1f, 0.1f, 0.1f);


			Actors[0].Material = mat;
			Actors[1].Material = mat;
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			//Update camera
			Cam.Update();

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(GameTime gameTime)
		{
			foreach (BasicActor actor in Actors)
			{
				actor.Draw(Cam.View, Cam.Projection, Cam.Position);
			}

			base.Draw(gameTime);
		}

		public override void HandleInput(InputState input)
		{
			//handle camera rotation
			Cam.Rotate(input.MouseDeltaX * CAMERA_MOVE_FACTOR, input.MouseDeltaY * CAMERA_MOVE_FACTOR);

			//Handle camera movement
			Vector3 translation = Vector3.Zero;
			if (input.CurrentKeyboardState.IsKeyDown(Keys.W)) translation += Vector3.Forward;
			if (input.CurrentKeyboardState.IsKeyDown(Keys.S)) translation += Vector3.Backward;
			if (input.CurrentKeyboardState.IsKeyDown(Keys.A)) translation += Vector3.Left;
			if (input.CurrentKeyboardState.IsKeyDown(Keys.D)) translation += Vector3.Right;

			//move 3 units per millisecond
			translation *= 1.5f * (float)input.CurrentGameTime.ElapsedGameTime.TotalMilliseconds;

			Cam.Move(translation);

			if (input.IsNewKeyPress(Keys.Escape))
			{
				ScreenManager.Game.Exit();
			}

			base.HandleInput(input);
		}

	}
}
