//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

datablock AudioProfile(FrmBumblebeeEngineSound)
{
   filename = "share/sounds/rotc/slide2.wav";
   description = AudioDefaultLooping3D;
	preload = true;
};

datablock AudioProfile(FrmBumblebeeExplosionSound)
{
	filename = "share/sounds/rotc/explosion7.wav";
	description = AudioFar3D;
	preload = true;
};

datablock AudioProfile(FrmBumblebeeSoftImpactSound)
{
   filename = "~/data/vehicles/tank/sound_softimpact.wav";
   description = AudioDefault3D;
	preload = true;
};

datablock AudioProfile(FrmBumblebeeHardImpactSound)
{
   filename = "~/data/vehicles/tank/sound_hardimpact.wav";
   description = AudioDefault3D;
	preload = true;
};
