﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Foster.Framework
{
    public abstract class Texture : GraphicsResource
    {

        public int Width { get; protected set; }
        public int Height { get; protected set; }

        /// <summary>
        /// The Texture Filter to be used while drawing
        /// </summary>
        public abstract TextureFilter Filter { get; set; }

        /// <summary>
        /// The Horizontal Wrapping mode
        /// </summary>
        public abstract TextureWrap WrapX { get; set; }

        /// <summary>
        /// The Vertical Wrapping mode
        /// </summary>
        public abstract TextureWrap WrapY { get; set; }

        /// <summary>
        /// If the Texture should be flipped vertically when drawing
        /// For example, OpenGL Render Targets usually require this
        /// </summary>
        public abstract bool FlipVertically { get; }

        protected Texture(Graphics graphics) : base(graphics)
        {

        }

        /// <summary>
        /// Sets the Texture Color data from the given buffer
        /// </summary>
        /// <param name="buffer"></param>
        public abstract void SetData(Memory<Color> buffer);

        /// <summary>
        /// Writes the Texture Color data to the given buffer
        /// </summary>
        /// <param name="buffer"></param>
        public abstract void GetData(Memory<Color> buffer);

        /// <summary>
        /// Creates a Bitmap with the Texture Color data
        /// </summary>
        /// <returns></returns>
        public Bitmap AsBitmap()
        {
            var bitmap = new Bitmap(Width, Height);
            GetData(new Memory<Color>(bitmap.Pixels));
            return bitmap;
        }

        /// <summary>
        /// Creates a new Texture of the given size
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Texture Create(int width, int height)
        {
            return App.GetModule<Graphics>().CreateTexture(width, height);
        }

        /// <summary>
        /// Creates a new Texture from the given Bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Texture Create(Bitmap bitmap)
        {
            var texture = App.GetModule<Graphics>().CreateTexture(bitmap.Width, bitmap.Height);
            texture.SetData(new Memory<Color>(bitmap.Pixels));
            return texture;
        }
    }
}
