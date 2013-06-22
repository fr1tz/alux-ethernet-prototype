//------------------------------------------------------------------------------
// Alux Ethernet Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

datablock AudioProfile(FrmSoldierpodEngineSound)
{
   filename = "share/sounds/rotc/slide2.wav";
   description = AudioDefaultLooping3D;
	preload = true;
};

datablock AudioProfile(FrmSoldierpodExplosionSound)
{
	filename = "share/sounds/rotc/explosion1.wav";
	description = AudioFar3D;
	preload = true;
};

datablock AudioProfile(FrmSoldierpodSoftImpactSound)
{
   filename = "~/data/vehicles/tank/sound_softimpact.wav";
   description = AudioDefault3D;
	preload = true;
};

datablock AudioProfile(FrmSoldierpodHardImpactSound)
{
   filename = "~/data/vehicles/tank/sound_hardimpact.wav";
   description = AudioDefault3D;
	preload = true;
};
