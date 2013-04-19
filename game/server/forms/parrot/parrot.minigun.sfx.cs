//------------------------------------------------------------------------------
// Alux Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

datablock AudioProfile(WpnParrotMinigunSpinUpSound)
{
	filename = "share/sounds/rotc/spin2up.wav";
	description = AudioDefault3D;
	preload = true;
};

datablock AudioProfile(WpnParrotMinigunSpinDownSound)
{
	filename = "share/sounds/rotc/spin2down.wav";
	description = AudioDefault3D;
	preload = true;
};

datablock AudioProfile(WpnParrotMinigunSpinSound)
{
	filename = "share/sounds/rotc/spin2.wav";
	description = AudioDefaultLooping3D;
	preload = true;
};

datablock AudioProfile(WpnParrotMinigunFireSound)
{
	filename = "share/sounds/rotc/spin2fire.wav";
	description = AudioDefaultLooping3D;
	preload = true;
};

datablock AudioProfile(WpnParrotMinigunProjectileImpactSound)
{
	filename = "share/sounds/rotc/impact1.wav";
	description = AudioDefault3D;
	preload = true;
};

datablock AudioProfile(WpnParrotMinigunProjectileFlybySound)
{
	filename = "share/sounds/rotc/flyby1.wav";
	description = AudioCloseLooping3D;
	preload = true;
};

datablock AudioProfile(WpnParrotMinigunProjectileMissedEnemySound)
{
	filename = "share/sounds/rotc/ricochet1-1.wav";
	alternate[0] = "share/sounds/rotc/ricochet1-1.wav";
	alternate[1] = "share/sounds/rotc/ricochet1-2.wav";
	alternate[2] = "share/sounds/rotc/ricochet1-3.wav";
	alternate[3] = "share/sounds/rotc/ricochet1-4.wav";
	alternate[4] = "share/sounds/rotc/ricochet1-5.wav";
	alternate[5] = "share/sounds/rotc/ricochet1-6.wav";
	description = AudioClose3D;
	preload = true;
};


