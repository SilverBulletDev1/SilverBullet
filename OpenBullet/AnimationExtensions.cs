using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace OpenBullet
{
	// Token: 0x02000024 RID: 36
	public static class AnimationExtensions
	{
		// Token: 0x0600007C RID: 124 RVA: 0x00002F44 File Offset: 0x00001144
		public static void BlurApply(this UIElement element, double from, double to, TimeSpan duration, bool autoReverse = false)
		{
			Storyboard storyboard = new Storyboard();
			DoubleAnimation animation = new DoubleAnimation
			{
				From = new double?(from),
				To = new double?(to),
				Duration = duration,
				AutoReverse = autoReverse
			};
			BlurEffect effect = new BlurEffect();
			element.Effect = effect;
			storyboard.Children.Add(animation);
			Storyboard.SetTarget(storyboard, element.Effect);
			Storyboard.SetTargetProperty(storyboard, new PropertyPath("Radius", Array.Empty<object>()));
			storyboard.Begin();
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002FC8 File Offset: 0x000011C8
		public static void BlurDisable(this UIElement element, TimeSpan duration)
		{
			BlurEffect blur = element.Effect as BlurEffect;
			if (blur == null || blur.Radius == 0.0)
			{
				return;
			}
			DoubleAnimation blurDisable = new DoubleAnimation(blur.Radius, 0.0, duration);
			blur.BeginAnimation(BlurEffect.RadiusProperty, blurDisable);
		}
	}
}
