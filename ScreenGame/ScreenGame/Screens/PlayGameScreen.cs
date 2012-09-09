using System.Collections.Generic;
using ClientWindowsGameLibrary.ScreenManagement;
using EngineGameLibrary.Maths;
using Jitter;
using Jitter.Dynamics;
using Jitter.Dynamics.Constraints;
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
using ScreenGame.Screens.Overlays;

namespace ScreenGame
{
	public class PlayGameScreen : GameScreen, ICameraEnabledGameScreen, IContentManagerScreen
	{
		private const float CAMERA_MOVE_FACTOR = 0.005f;
		ContentManager Content;
		ScreenContentManager screenContentManager;
		FreeCamera Cam;
		GraphicsDevice graphics;
		List<BasicActor> Actors = new List<BasicActor>();
		Color[] rndColors;

		public DebugDrawer DebugDrawer { get; set; }

		public override void LoadContent()
		{
			//colors for debug 
			rndColors = new Color[20];

			for (int i = 0; i < 20; i++)
			{
				rndColors[i] = new Color((float)RandomManager.TheRandom.NextDouble(), (float)RandomManager.TheRandom.NextDouble(), (float)RandomManager.TheRandom.NextDouble());
			}
			
			//Content manager
			if (Content == null)
			{
				Content = new ContentManager(ScreenManager.Game.Services, "Content");
				screenContentManager = new ScreenContentManager(Content);
			}


			DebugDrawer = new DebugDrawer(ScreenManager.Game, this);
			ScreenManager.Game.Components.Add(DebugDrawer);

			//Load models
			screenContentManager.AddModel("ground", @"Models\Ground");
			screenContentManager.AddModel("ship", @"Models\ship");
			screenContentManager.AddModel("teapot", @"Models\teapot");

			//Set graphics
			graphics = ScreenManager.GraphicsDevice;
			graphics.BlendState = BlendState.Opaque;
			graphics.DepthStencilState = DepthStencilState.Default;

			//Load fonts
			screenContentManager.AddFont("kootenay14", @"Fonts\kootenay14");

			//setup camera 
			if (Cam == null)
				Cam = new FreeCamera(new Vector3(0, 100, -250),
					MathHelper.ToRadians(180),
					MathHelper.ToRadians(5),
					graphics);

			//set mouse to invisible
			ScreenManager.Game.IsMouseVisible = false;

			//set up game state

			foreach (Entity e in GameStateManager.GameManager.Entities)
			{
				Actors.Add(new BasicActor(e, screenContentManager.GetModel(e.Name)));
			}

			screenContentManager.AddEffect("lightingEffect", @"Models\LightingEffect");

			LightingMaterial mat = new LightingMaterial();
			//mat.LightColor = Color.Red.ToVector3();
			mat.AmbientColor = new Vector3(0.1f, 0.1f, 0.1f);


			foreach (BasicActor actor in Actors)
			{
				actor.SetModelEffect(screenContentManager.GetEffect("lightingEffect"), true);
				actor.Material = mat;
			}

			//setup overlays
			Overlays.Add(new DebugOverlay(this));
			Overlays[0].IsActive = true;

			
		}

		public override void UnloadContent()
		{
			ScreenManager.Game.IsMouseVisible = true;

			Actors.Clear();
			
			if (DebugDrawer != null)
				DebugDrawer.Dispose();

			if (Content != null)
				Content.Unload();
			
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

			//move .3 units per millisecond
			translation *= 0.075f * (float)input.CurrentGameTime.ElapsedGameTime.TotalMilliseconds;

			Cam.Move(translation);

			//Update camera
			Cam.Update();

			if (input.IsNewKeyPress(Keys.Escape))
			{
				LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
			}

			//reset mouse
			Mouse.SetPosition(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2);
			input.CurrentMouseState = Mouse.GetState();

			base.HandleInput(input);
		}

		public Camera GetCamera()
		{
			return Cam;
		}

		public ScreenContentManager GetScreenContentManager()
		{
			return screenContentManager;
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
