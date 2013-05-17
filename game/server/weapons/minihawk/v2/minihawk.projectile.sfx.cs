//------------------------------------------------------------------------------
// Alux Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

datablock AudioProfile(WpnMinihawkProjectileImpactSound)
{
	filename = "share/sounds/rotc/impact1.wav";
	description = AudioDefault3D;
	preload = true;
};

datablock AudioProfile(WpnMinihawkProjectileFlybySound)
{
	filename = "share/sounds/cat5/lasershot2.wav";
	description = AudioCloseLooping3D;
	preload = true;
};

datablock AudioProfile(WpnMinihawkProjectileMissedEnemySound)
{
	filename = "share/sounds/cat5/lasershot2.wav";
	description = AudioClose3D;
	preload = true;
};
