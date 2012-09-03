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
using Jitter.Collision;
using Jitter;
using Jitter.Collision.Shapes;
using Jitter.Dynamics;

namespace ScreenGame
{
	public class PlayGameScreen : GameScreen
	{
		private const float CAMERA_MOVE_FACTOR = 0.005f;
		ContentManager Content;
		FreeCamera Cam;
		GraphicsDevice graphics;

		List<BasicActor> Actors = new List<BasicActor>();

		CollisionSystem collision;
		World world;

		public override void LoadContent()
		{
			if (Content == null)
				Content = new ContentManager(ScreenManager.Game.Services, "Content");

			graphics = ScreenManager.GraphicsDevice;
			graphics.BlendState = BlendState.Opaque;
			graphics.DepthStencilState = DepthStencilState.Default;


			if (Cam == null)
				Cam = new FreeCamera(new Vector3(1000, 600, -2000),
					MathHelper.ToRadians(153),
					MathHelper.ToRadians(-5),
					graphics);

			//Jitter
			collision = new CollisionSystemSAP();
			world = new World(collision);

			//ground = new RigidBody(new BoxShape(new JVector(200, 20, 200)));
			//ground.Position = new JVector(0, -10, 0);
			//ground.Tag = BodyTag.DontDrawMe;
			//ground.IsStatic = true; Demo.World.AddBody(ground);
			////ground.Restitution = 1.0f;
			//ground.Material.KineticFriction = 0.0f;


			Shape shape = new BoxShape(1.0f, 2.0f, 3.0f);
			RigidBody body = new RigidBody(shape);

			world.AddBody(body);


			//Actors.Add(new BasicActor(Content.Load<Model>(@"Models\Ground"), Vector3.Zero, Vector3.Zero, Vector3.One, graphics));
			//Actors.Add(new BasicActor(Content.Load<Model>(@"Models\teapot"), new Vector3(0f, 0f, 0f), Vector3.Zero, Vector3.One * 10, graphics));
			//Actors.Add(new BasicActor(Content.Load<Model>(@"Models\teapot"), new Vector3(250f, 0f, 0f), Vector3.Zero, Vector3.One * 10, graphics));
			//Actors.Add(new BasicActor(Content.Load<Model>(@"Models\ship"), new Vector3(-500f, 300f, 500f), Vector3.Zero, Vector3.One, graphics));

			//Effect simpleEffect = Content.Load<Effect>(@"Models\LightingEffect");

			//LightingMaterial mat = new LightingMaterial();
			//mat.LightColor = Color.Red.ToVector3();
			//mat.AmbientColor = new Vector3(0.1f, 0.1f, 0.1f);


			//foreach (BasicActor actor in Actors)
			//{
			//	actor.SetModelEffect(simpleEffect, true);
			//	actor.Material = mat;
			//}
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			//Update jitter world
			float step = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (step > 1.0f / 100.0f) step = 1.0f / 100.0f;
			world.Step(step, true);

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
