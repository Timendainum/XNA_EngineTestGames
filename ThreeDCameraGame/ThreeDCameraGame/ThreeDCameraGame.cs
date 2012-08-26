using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ThreeDWindowsGameLibrary.Cameras;
using ThreeDWindowsGameLibrary.Actors;

namespace ThreeDCameraGame
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class ThreeDCameraGame : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Camera Cam;

		List<BasicActor> Actors = new List<BasicActor>();

		MouseState LastMouseState;

		private ECameraMode _cameraMode;
		public ECameraMode CameraMode
		{
			get
			{
				return _cameraMode;
			}
			set
			{
				_cameraMode = value;
				switch (value)
				{
					case ECameraMode.Target:
						Cam = new TargetCamera(new Vector3(600, 600, -2000), new Vector3(0f, 300f, 0f), GraphicsDevice);
						break;
					case ECameraMode.Free:
						Cam = new FreeCamera(new Vector3(1000, 600, -2000),
										MathHelper.ToRadians(153),
										MathHelper.ToRadians(-5),
										GraphicsDevice);
						break;
				}
			}
		}

		public ThreeDCameraGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			Window.AllowUserResizing = true;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			CameraMode = ECameraMode.Target;

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
			Actors.Add(new BasicActor(Content.Load<Model>(@"ship"), new Vector3(0f, 300f, 0f), Vector3.Zero, Vector3.One, GraphicsDevice));
			Actors.Add(new BasicActor(Content.Load<Model>(@"teapot"), new Vector3(0f, 0f, -1200f), Vector3.Zero, Vector3.One * 10, GraphicsDevice));
			Actors.Add(new BasicActor(Content.Load<Model>(@"Ground"), Vector3.Zero, Vector3.Zero, Vector3.One, GraphicsDevice));

			LastMouseState = Mouse.GetState();
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			MouseState mouseState = Mouse.GetState();
			KeyboardState keyboardState = Keyboard.GetState();

			// Allows the game to exit
			if (keyboardState.IsKeyDown(Keys.Escape))
				this.Exit();

			//Change camera mode
			if (keyboardState.IsKeyDown(Keys.D1)) CameraMode = ECameraMode.Target;
			if (keyboardState.IsKeyDown(Keys.D2)) CameraMode = ECameraMode.Free;

			//Update camera base on mod and input
			switch (CameraMode)
			{
				case ECameraMode.Free:
					FreeCamera fc = (FreeCamera)Cam;

					//handle camera rotation
					float deltaX = (float)LastMouseState.X - (float)mouseState.X;
					float deltaY = (float)LastMouseState.Y - (float)mouseState.Y;

					fc.Rotate(deltaX * 0.005f, deltaY * 0.005f);

					//Handle camera movement
					Vector3 translation = Vector3.Zero;
					if (keyboardState.IsKeyDown(Keys.W)) translation += Vector3.Forward;
					if (keyboardState.IsKeyDown(Keys.S)) translation += Vector3.Backward;
					if (keyboardState.IsKeyDown(Keys.A)) translation += Vector3.Left;
					if (keyboardState.IsKeyDown(Keys.D)) translation += Vector3.Right;

					//move 3 units per millisecond
					translation *= 1.5f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

					fc.Move(translation);

					break;
			}

			//Update camera
			Cam.Update();


			LastMouseState = mouseState;

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here
			foreach (BasicActor actor in Actors)
			{
				actor.Draw(Cam.View, Cam.Projection);
			}

			base.Draw(gameTime);
		}
	}
}
