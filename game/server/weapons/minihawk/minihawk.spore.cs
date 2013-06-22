//------------------------------------------------------------------------------
// Alux Ethernet Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

datablock OrbitProjectileData(WpnMinihawkSpore)
{
   muzzleVelocity = 15;
   maxTrackingAbility = 5;
	trackingAgility = 1;
	laserTail = WpnMinihawkProjectileLaserTail;
	laserTailLen = 0.5;
	//laserTrail[0]   = FrmCrateSpore_LaserTrailZero;
   lifetime = 20000;
   //projectileShapeName = "share/shapes/alux/light.dts";
   unlimitedBounces = true;
   bounceElasticity = 0.999;
};
