﻿using SharpDX.Direct3D11;
using DxDevice = SharpDX.Direct3D11.Device;

namespace DeltaEngine.Graphics.SharpDX
{
	/// <summary>
	/// Simplifies DirectX sampler creation
	/// </summary>
	public class SharpDXSampler : SamplerState
	{
		public SharpDXSampler(DxDevice nativeDevice, Filter filter)
			: base(nativeDevice, BuildFilterDescription(filter)) { }

		protected static SamplerStateDescription BuildFilterDescription(Filter filter)
		{
			return new SamplerStateDescription
			{
				AddressU = TextureAddressMode.Clamp,
				AddressV = TextureAddressMode.Clamp,
				AddressW = TextureAddressMode.Clamp,
				Filter = filter
			};
		}
	}
}