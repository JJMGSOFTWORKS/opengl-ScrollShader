using InfiniteLayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
public class Game1 : Game
{
    private SpriteBatch _spriteBatch;
    private Texture2D _bg;
    private ScrollCamera _scrollCamera;
    private Effect _effect;
    private SpriteFont _font;

    public Game1()
    {
        new GraphicsDeviceManager(this) { PreferredBackBufferWidth = 800, PreferredBackBufferHeight = 600 };
        Content.RootDirectory = "Content";
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _bg = Content.Load<Texture2D>("bg");
        _scrollCamera = new ScrollCamera(GraphicsDevice.Viewport);
        _effect = Content.Load<Effect>("NewEffect");
        
        _font = Content.Load<SpriteFont>("font");

        //var test = _effect.Parameters["ViewportSize"];


        _effect.Parameters["ViewportSize"].SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
    }

    protected override void Update(GameTime gameTime)
    {
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

        KeyboardState state = Keyboard.GetState();

        const float cameraTranslationSpeed = 1000f;
        const float cameraRotationSpeed = 2f;
        const float cameraZoomSpeed = 1f;
        const float minCameraZoom = 0.1f;

        // Camera Translation
        if (state.IsKeyDown(Keys.Left))
            _scrollCamera.Move(-Vector2.UnitX * elapsed * cameraTranslationSpeed, true);
        if (state.IsKeyDown(Keys.Right))
            _scrollCamera.Move(Vector2.UnitX * elapsed * cameraTranslationSpeed, true);
        if (state.IsKeyDown(Keys.Up))
            _scrollCamera.Move(-Vector2.UnitY * elapsed * cameraTranslationSpeed, true);
        if (state.IsKeyDown(Keys.Down))
            _scrollCamera.Move(Vector2.UnitY * elapsed * cameraTranslationSpeed, true);

        // Camera Zoom
        if (state.IsKeyDown(Keys.PageUp))
            _scrollCamera.Zoom += cameraZoomSpeed * elapsed;
        if (state.IsKeyDown(Keys.PageDown))
            _scrollCamera.Zoom = MathHelper.Max(_scrollCamera.Zoom - cameraZoomSpeed * elapsed, minCameraZoom);

        // Camera Rotation
        if (state.IsKeyDown(Keys.Insert))
            _scrollCamera.Rotation += cameraRotationSpeed * elapsed;
        if (state.IsKeyDown(Keys.Delete))
            _scrollCamera.Rotation -= cameraRotationSpeed * elapsed;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Prepare the effect with a view matrix
        _effect.Parameters["ScrollMatrix"].SetValue(_scrollCamera.GetScrollMatrix(new Vector2(_bg.Width, _bg.Height)));

        // Start the SpriteBatch using LinearWrap and the scroll effect
        _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, _effect);

        // Draw texture to viewport-sized quad setting both source and destination rectangles
        _spriteBatch.Draw(_bg, GraphicsDevice.Viewport.Bounds, GraphicsDevice.Viewport.Bounds, Color.White);

        _spriteBatch.End();

        DrawText();

        base.Draw(gameTime);
    }

    private void DrawText()
    {
        _spriteBatch.Begin();
        _spriteBatch.DrawString(_font, "Move: Arrows\nZoom: Page Up/Down\nRotate: Insert/Delete", new Vector2(22, 22), Color.Black);
        _spriteBatch.DrawString(_font, "Move: Arrows\nZoom: Page Up/Down\nRotate: Insert/Delete", new Vector2(20, 20), Color.White);
        _spriteBatch.DrawString(_font, "http://www.david-gouveia.com", new Vector2(22, 562), Color.Black);
        _spriteBatch.DrawString(_font, "http://www.david-gouveia.com", new Vector2(20, 560), Color.White);
        _spriteBatch.DrawString(_font, "Infinite Background", new Vector2(532, 22), Color.Black);
        _spriteBatch.DrawString(_font, "Infinite Background", new Vector2(530, 20), Color.Yellow);
        _spriteBatch.End();
    }

}