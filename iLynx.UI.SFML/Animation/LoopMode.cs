namespace iLynx.UI.Sfml.Animation
{
    public enum LoopMode
    {
        /// <summary>
        /// This mode will result in an animation going back and forth between start and end
        /// </summary>
        Reverse = 1,
        /// <summary>
        /// This mode will restart the animation from start whenever end is reached
        /// </summary>
        Restart = 2,
        /// <summary>
        /// This mode will not loop the animation at all
        /// </summary>
        None = 0
    }
}