using System.Collections.Generic;
using ClientWindowsGameLibrary.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ThreeDWindowsGameLibrary.Actors;
using ThreeDWindowsGameLibrary.Actors.Materials;
using ThreeDWindowsGameLibrary.Cameras;
using ScreenGame.Screens;
using EngineGameLibrary.Simulation;
using ThreeDWindowsGameLibrary.Simulation;

namespace ScreenGame
{
	public class PlayGameScreen : GameScreen
	{
		private const float CAMERA_MOVE_FACTOR = 0.005f;
		ContentManager Content;
		FreeCamera Cam;
		GraphicsDevice graphics;

		Dictionary<string, Model> Models = new Dictionary<string, Model>();
		List<BasicActor> Actors = new List<BasicActor>();

		public override void LoadContent()
		{
			if (Content == null)
				Content = new ContentManager(ScreenManager.Game.Services, "Content");

			//Load models
               Models["ground"] = Content.Load<Model>(@"Models\Ground");
			Models["ship"] = Content.Load<Model>(@"Models\ship");
               Models["teapot"] = Content.Load<Model>(@"Models\teapot");

			//Set graphics
			graphics = ScreenManager.GraphicsDevice;
			graphics.BlendState = BlendState.Opaque;
			graphics.DepthStencilState = DepthStencilState.Default;


			if (Cam == null)
				Cam = new FreeCamera(new Vector3(1000, 600, -2000),
					MathHelper.ToRadians(153),
					MathHelper.ToRadians(-5),
					graphics);

			foreach (Entity e in GameStateManager.GameManager.Entities)
			{
				Actors.Add(new BasicActor(e, Models[e.Name]));
			}

			Effect simpleEffect = Content.Load<Effect>(@"Models\LightingEffect");

			LightingMaterial mat = new LightingMaterial();
			//mat.LightColor = Color.Red.ToVector3();
			mat.AmbientColor = new Vector3(0.1f, 0.1f, 0.1f);


			foreach (BasicActor actor in Actors)
			{
				actor.SetModelEffect(simpleEffect, true);
				actor.Material = mat;
			}
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(GameTime gameTime)
		{
			foreach (BasicActor actor in Actors)
			{
				actor.Draw(Cam.View, Cam.Projection, Cam.Position);
			}
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

			//Update camera
			Cam.Update();

			if (input.IsNewKeyPress(Keys.Escape))
			{
				LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
			}
		}

	}
}
