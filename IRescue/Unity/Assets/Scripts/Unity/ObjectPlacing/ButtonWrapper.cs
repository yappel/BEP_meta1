// <copyright file="ButtonWrapper.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing
{
    using Meta;
    using UnityEngine;

    /// <summary>
    /// Singleton for the button wrapper game object
    /// </summary>
    public class ButtonWrapper
    {
        /// <summary>
        /// The instance of this singleton
        /// </summary>
        private static ButtonWrapper buttonWrapper;

        /// <summary>
        /// The wrapper for all buttons to change the scale for all buttons the same
        /// </summary>
        private static GameObject wrapper;

        /// <summary>
        /// Prevents a default instance of the <see cref="ButtonWrapper"/> class from being created
        /// </summary>
        private ButtonWrapper()
        {
            wrapper = new GameObject("Buttons");
            wrapper.AddComponent<MetaBody>().hud = true;
        }

        /// <summary>
        /// Gets the wrapper transform
        /// </summary>
        public static Transform Wrapper
        {
            get
            {
                if (buttonWrapper == null || wrapper == null)
                {
                    buttonWrapper = new ButtonWrapper();
                }

                return wrapper.transform;
            }
        }

        /// <summary>
        /// Set the scale of all buttons.
        /// </summary>
        /// <param name="scale">the new scale of all buttons</param>
        public static void SetScale(float scale)
        {
            wrapper.transform.localScale = new Vector3(scale, scale, 1);
        }
    }
}