// <copyright file="ProgressBarAnimation.cs" company="Visual Software Systems Ltd. and others">Copyright (c) 2019 All rights reserved</copyright>

namespace DownloadAndInstallSample.UI
{
    using System;
    using Android.Views.Animations;
    using Android.Widget;

    /// <summary>
    /// handles the animation of the progress bar
    /// </summary>
    public class ProgressBarAnimation : Animation
    {
        /// <summary>
        /// The progress bar control
        /// </summary>
        private readonly ProgressBar progressBar;

        /// <summary>
        /// The value progressing from
        /// </summary>
        private readonly float from;

        /// <summary>
        /// The value progressing to
        /// </summary>
        private readonly float to;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBarAnimation" /> class.
        /// </summary>
        /// <param name="progressBar">The progress bar control</param>
        /// <param name="from">The value progressing from</param>
        /// <param name="to">The value progressing to</param>
        public ProgressBarAnimation(ProgressBar progressBar, float @from, float to) : base()
        {
            this.progressBar = progressBar;
            this.from = @from;
            this.to = to;
        }

        /// <summary>
        /// Helper for getTransformation. Subclasses should implement this to apply their transforms given an interpolation value.
        /// Implementations of this method should always replace the specified Transformation or document they are doing otherwise.
        /// </summary>
        /// <param name="interpolatedTime">The value of the normalized time (0.0 to 1.0) after it has been run through the interpolation function</param>
        /// <param name="t">The Transformation object to fill in with the current transforms.</param>
        protected override void ApplyTransformation(float interpolatedTime, Transformation t)
        {
            base.ApplyTransformation(interpolatedTime, t);

            var value = this.from + ((this.to - this.from) * interpolatedTime);
            this.progressBar.Progress = (int)value;
        }
    }
}