using System.Collections.Generic;
using ClientWindowsGameLibrary.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ScreenGame.Screens;
using ThreeDWindowsGameLibrary.Actors;
using ThreeDWindowsGameLibrary.Actors.Materials;
using ThreeDWindowsGameLibrary.Cameras;
using ThreeDWindowsGameLibrary.ScreenManagement;
using ThreeDWindowsGameLibrary.Simulation;
using Jitter.Dynamics.Constraints;
using Jitter;
using Jitter.Dynamics;
using EngineGameLibrary.Maths;

namespace ScreenGame
{
	public class PlayGameScreen : GameScreen, ICameraEnabledGameScreen
	{
		private const float CAMERA_MOVE_FACTOR = 0.005f;
		ContentManager Content;
		FreeCamera Cam;
		GraphicsDevice graphics;
		Dictionary<string, Model> Models = new Dictionary<string, Model>();
		List<BasicActor> Actors = new List<BasicActor>();
		Color[] rndColors;

		public DebugDrawer DebugDrawer { get; set; }

		public override void LoadContent()
		{
			if (Content == null)
				Content = new ContentManager(ScreenManager.Game.Services, "Content");

			DebugDrawer = new DebugDrawer(ScreenManager.Game, this);
			ScreenManager.Game.Components.Add(DebugDrawer);

			//Load models
               Models["ground"] = Content.Load<Model>(@"Models\Ground");
			Models["ship"] = Content.Load<Model>(@"Models\ship");
               Models["teapot"] = Content.Load<Model>(@"Models\teapot");

			//Set graphics
			graphics = ScreenManager.GraphicsDevice;
			graphics.BlendState = BlendState.Opaque;
			graphics.DepthStencilState = DepthStencilState.Default;


			if (Cam == null)
				Cam = new FreeCamera(new Vector3(50, 30, -100),
					MathHelper.ToRadians(133),
					MathHelper.ToRadians(5),
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


			rndColors = new Color[20];

			for (int i = 0; i < 20; i++)
			{
				rndColors[i] = new Color((float)RandomManager.TheRandom.NextDouble(), (float)RandomManager.TheRandom.NextDouble(), (float)RandomManager.TheRandom.NextDouble());
			}
		}

		public override void UnloadContent()
		{
			if (DebugDrawer != null)
				DebugDrawer.Dispose();
			
			base.UnloadContent();
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

			DrawJitterDebugInfo();
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

			//move .3 units per millisecond
			translation *= 0.075f * (float)input.CurrentGameTime.ElapsedGameTime.TotalMilliseconds;

			Cam.Move(translation);

			//Update camera
			Cam.Update();

			if (input.IsNewKeyPress(Keys.Escape))
			{
				LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
			}
		}

		public Camera GetCamera()
		{
			return Cam;
		}

		private void DrawJitterDebugInfo()
		{
			int cc = 0;
			World world = GameStateManager.GameManager.World;
			foreach (Constraint constr in world.Constraints)
				constr.DebugDraw(DebugDrawer);

			foreach (RigidBody body in world.RigidBodies)
			{
				DebugDrawer.Color = rndColors[cc % rndColors.Length];
				body.DebugDraw(DebugDrawer);
				cc++;
			}
		}

	}
}
