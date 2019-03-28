using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.GameScreens;

namespace MetalSpace.Interfaces
{
    /// <summary>
    /// Interface to represents a option entry.
    /// </summary>
    public interface IOptionEntry
    {
        /// <summary>
        /// Text that represents the option.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Event handler to proccess the option.
        /// </summary>
        event EventHandler<EventArgs> Selected;

        /// <summary>
        /// Function that handle the entry.
        /// </summary>
        /// <param name="direction"></param>
        void OnSelectEntry(int direction);

        /// <summary>
        /// Update the current state of the option.
        /// </summary>
        /// <param name="screen">Reference to the <c>OptionsMenu</c> screen.</param>
        /// <param name="isSelected">true if the current option is selected, false otherwise.</param>
        /// <param name="gametime">Global time of the game.</param>
        void Update(OptionsMenu screen, bool isSelected, GameTime gametime);

        /// <summary>
        /// Draw the current state of the option.
        /// </summary>
        /// <param name="screen">Reference to the <c>OptionsMenu</c> screen.</param>
        /// <param name="position">Position where the option is drawed.</param>
        /// <param name="isSelected">true if the current option is selected, false otherwise.</param>
        /// <param name="gametime">Global time of the game.</param>
        void Draw(OptionsMenu screen, Vector2 position, bool isSelected, GameTime gameTime);
    }
}
