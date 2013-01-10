//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

datablock AudioProfile(WpnPythonFireSound)
{
	filename = "share/sounds/rotc/fire2.wav";
	description = AudioDefault3D;
	preload = true;
};

datablock AudioProfile(WpnPythonProjectileSound)
{
	filename = "share/sounds/rotc/charge5.wav";
	description = AudioCloseLooping3D;
	preload = true;	
};

datablock AudioProfile(WpnPythonProjectileExplosionSound)
{
	filename = "share/sounds/rotc/explosion2.wav";
	description = AudioDefault3D;
	preload = true;	
};

datablock AudioProfile(WpnPythonProjectileImpactSound)
{
	filename = "share/sounds/rotc/explosion2.wav";
	description = AudioDefault3D;
	preload = true;
};

datablock AudioProfile(WpnPythonProjectileHitSound)
{
	filename = "share/sounds/rotc/explosion2.wav";
	description = AudioDefault3D;
	preload = true;
};

datablock AudioProfile(WpnPythonProjectileBounceSound)
{
	filename = "share/sounds/rotc/bounce1.wav";
//	alternate[0] = "share/sounds/rotc/impact3-1.wav";
//	alternate[1] = "share/sounds/rotc/impact3-2.wav";
//	alternate[2] = "share/sounds/rotc/impact3-3.wav";
//	alternate[3] = "share/sounds/rotc/ricochet2-1.wav";
//	alternate[4] = "share/sounds/rotc/ricochet2-1.wav";
//	alternate[5] = "share/sounds/rotc/ricochet2-1.wav";
	description = AudioClose3D;
	preload = true;
};

datablock AudioProfile(WpnPythonProjectileMissedEnemySound)
{
	filename = "share/sounds/rotc/flyby1.wav";
	description = AudioClose3D;
	preload = true;
};


