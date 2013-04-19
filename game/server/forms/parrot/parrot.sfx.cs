//------------------------------------------------------------------------------
// Alux Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

datablock AudioProfile(FrmParrotEngineSound)
{
   filename = "share/sounds/rotc/slide3.wav";
   description = AudioDefaultLooping3D;
	preload = true;
};

datablock AudioProfile(FrmParrotExplosionSound)
{
	filename = "share/sounds/rotc/explosion10.wav";
	description = AudioFar3D;
	preload = true;
};

datablock AudioProfile(FrmParrotSoftImpactSound)
{
   filename = "~/data/vehicles/tank/sound_softimpact.wav";
   description = AudioDefault3D;
	preload = true;
};

datablock AudioProfile(FrmParrotHardImpactSound)
{
   filename = "~/data/vehicles/tank/sound_hardimpact.wav";
   description = AudioDefault3D;
	preload = true;
};
