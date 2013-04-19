//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

datablock OrbitProjectileData(WpnRaptorSpore)
{
   muzzleVelocity = 15;
   maxTrackingAbility = 4;
	trackingAgility = 1;
	laserTail = WpnRaptorProjectileLaserTail;
	laserTailLen = 0.5;
	//laserTrail[0]   = FrmCrateSpore_LaserTrailZero;
   lifetime = 20000;
   //projectileShapeName = "share/shapes/alux/light.dts";
   unlimitedBounces = true;
   bounceElasticity = 0.999;
};
