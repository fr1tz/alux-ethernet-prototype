//------------------------------------------------------------------------------
// Alux Ethernet Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

datablock OrbitProjectileData(WpnBadgerSpore)
{
   muzzleVelocity = 15;
   maxTrackingAbility = 6;
	trackingAgility = 1;
	laserTail = WpnBadgerProjectileLaserTail;
	laserTailLen = 0.5;
	//laserTrail[0]   = FrmCrateSpore_LaserTrailZero;
   lifetime = 20000;
   //projectileShapeName = "share/shapes/alux/light.dts";
   unlimitedBounces = true;
   bounceElasticity = 0.999;
};
