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
using TwoDWindowsGameLibrary.HexTileMap;

namespace HexMapGame
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class HexMapGame : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		Dictionary<String, Texture2D> Textures = new Dictionary<String, Texture2D>();
		HexTileMap hexMap;
		int squaresAcross = 17;
		int squaresDown = 37;
		int baseOffsetX = -14;
		int baseOffsetY = -14;

		public HexMapGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
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
			Texture2D newTexture = Content.Load<Texture2D>(@"part3_tileset");
			Textures.Add("TileMap", newTexture);

			HexTile.Texture = Textures["TileMap"];
			HexTile.Width = 33;
			HexTile.Height = 27;
			HexTile.StepX = 52;
			HexTile.StepY = 14;
			HexTile.OddRowXOffset = 26;

			hexMap = new HexTileMap(100, 100);

			// Create Sample Map Data
			hexMap.Rows[0].Columns[3].TileId = 3;
			hexMap.Rows[0].Columns[4].TileId = 3;
			hexMap.Rows[0].Columns[5].TileId = 1;
			hexMap.Rows[0].Columns[6].TileId = 1;
			hexMap.Rows[0].Columns[7].TileId = 1;

			hexMap.Rows[1].Columns[3].TileId = 3;
			hexMap.Rows[1].Columns[4].TileId = 1;
			hexMap.Rows[1].Columns[5].TileId = 1;
			hexMap.Rows[1].Columns[6].TileId = 1;
			hexMap.Rows[1].Columns[7].TileId = 1;

			hexMap.Rows[2].Columns[2].TileId = 3;
			hexMap.Rows[2].Columns[3].TileId = 1;
			hexMap.Rows[2].Columns[4].TileId = 1;
			hexMap.Rows[2].Columns[5].TileId = 1;
			hexMap.Rows[2].Columns[6].TileId = 1;
			hexMap.Rows[2].Columns[7].TileId = 1;

			hexMap.Rows[3].Columns[2].TileId = 3;
			hexMap.Rows[3].Columns[3].TileId = 1;
			hexMap.Rows[3].Columns[4].TileId = 1;
			hexMap.Rows[3].Columns[5].TileId = 2;
			hexMap.Rows[3].Columns[6].TileId = 2;
			hexMap.Rows[3].Columns[7].TileId = 2;

			hexMap.Rows[4].Columns[2].TileId = 3;
			hexMap.Rows[4].Columns[3].TileId = 1;
			hexMap.Rows[4].Columns[4].TileId = 1;
			hexMap.Rows[4].Columns[5].TileId = 2;
			hexMap.Rows[4].Columns[6].TileId = 2;
			hexMap.Rows[4].Columns[7].TileId = 2;

			hexMap.Rows[5].Columns[2].TileId = 3;
			hexMap.Rows[5].Columns[3].TileId = 1;
			hexMap.Rows[5].Columns[4].TileId = 1;
			hexMap.Rows[5].Columns[5].TileId = 2;
			hexMap.Rows[5].Columns[6].TileId = 2;
			hexMap.Rows[5].Columns[7].TileId = 2;
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
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			// TODO: Add your update logic here
			KeyboardState ks = Keyboard.GetState();
			if (ks.IsKeyDown(Keys.Left))
			{
				Camera.Location.X = MathHelper.Clamp(Camera.Location.X - 2, 0, (hexMap.Width - squaresAcross) * HexTile.StepX);
			}

			if (ks.IsKeyDown(Keys.Right))
			{
				Camera.Location.X = MathHelper.Clamp(Camera.Location.X + 2, 0, (hexMap.Width - squaresAcross) * HexTile.StepX);
			}

			if (ks.IsKeyDown(Keys.Up))
			{
				Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y - 2, 0, (hexMap.Height - squaresDown) * HexTile.StepY);
			}

			if (ks.IsKeyDown(Keys.Down))
			{
				Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y + 2, 0, (hexMap.Height - squaresDown) * HexTile.StepY);
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			// TODO: Add your drawing code here



			spriteBatch.Begin();

			Vector2 firstSquare = new Vector2(Camera.Location.X / HexTile.StepX, Camera.Location.Y / HexTile.StepY);
			int firstX = (int)firstSquare.X;
			int firstY = (int)firstSquare.Y;

			Vector2 squareOffset = new Vector2(Camera.Location.X % HexTile.StepX, Camera.Location.Y % HexTile.StepY);
			int offsetX = (int)squareOffset.X;
			int offsetY = (int)squareOffset.Y;

			for (int y = 0; y < squaresDown; y++)
			{
				int rowOffset = 0;
				if ((firstY + y) % 2 == 1)
					rowOffset = HexTile.OddRowXOffset;

				for (int x = 0; x < squaresAcross; x++)
				{
					foreach (int tileID in hexMap.Rows[y + firstY].Columns[x + firstX].BaseTiles)
					{
						spriteBatch.Draw(
						    HexTile.Texture,
						    new Rectangle(
							   (x * HexTile.StepX) - offsetX + rowOffset + baseOffsetX,
							   (y * HexTile.StepY) - offsetY + baseOffsetY,
							   HexTile.Width, HexTile.Height),
						    HexTile.GetSourceRectangle(tileID),
						    Color.White);
					}
				}
			}

			spriteBatch.End();


			base.Draw(gameTime);
		}
	}
}
