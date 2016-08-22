namespace InfiniteLayer
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class ScrollCamera
    {
        public ScrollCamera(Viewport viewport)
        {
            Origin = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
        }

        public Vector2 Position;
        public float Zoom = 1f;
        public float Rotation;
        public Vector2 Origin;

        public Matrix GetScrollMatrix(Vector2 textureSize)
        {
            return Matrix.CreateTranslation(new Vector3(-Origin / textureSize, 0.0f)) *
                   Matrix.CreateScale(1f / Zoom) * 
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateTranslation(new Vector3(Origin / textureSize, 0.0f)) *
                   Matrix.CreateTranslation(new Vector3(Position / textureSize, 0.0f));
        }

        public void Move(Vector2 displacement, bool respectRotation = false)
        {
            if (respectRotation)
                displacement = Vector2.Transform(displacement, Matrix.CreateRotationZ(Rotation));
            Position += displacement;
        }
    }
}
