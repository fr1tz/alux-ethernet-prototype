//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

datablock AudioProfile(FrmLightProjectileExplosionSound)
{
	filename = "share/sounds/rotc/explosion3.wav";
	description = AudioDefault3D;
	preload = true;
};

datablock AudioProfile(FrmLightProjectileSound)
{
	filename = "share/sounds/rotc/missile1.wav";
	description = AudioCloseLooping3D;
	preload = true;
};

datablock AudioProfile(FrmLightSpawnSound)
{
	filename = "share/shapes/rotc/vehicles/etherform/sound.spawn.wav";
	description = AudioDefault3D;
	preload = true;
};

datablock AudioProfile(FrmLightImpactSound)
{
	filename = "share/shapes/rotc/vehicles/etherform/sound.impact.wav";
	description = AudioDefault3D;
	preload = true;
};

datablock AudioProfile(FrmLightSingSound)
{
	filename = "share/shapes/rotc/vehicles/etherform/sound.sing.wav";
	description = AudioClosestLooping3D;
	preload = true;
};

