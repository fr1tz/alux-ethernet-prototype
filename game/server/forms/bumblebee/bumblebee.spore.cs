//------------------------------------------------------------------------------
// Alux Ethernet Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

datablock LaserBeamData(FrmBumblebeeSpore_LaserTail)
{
	hasLine = true;
	lineStartColor	= "0.00 0.50 1.00 0.0";
	lineBetweenColor = "0.00 0.50 1.00 0.5";
	lineEndColor	  = "0.00 0.50 1.00 0.5";
	lineWidth		  = 1.0;

	hasInner = true;
	innerStartColor = "0.00 0.50 1.00 0.0";
	innerBetweenColor = "0.00 0.50 1.00 0.5";
	innerEndColor = "0.00 0.50 1.00 0.5";
	innerStartWidth = "0.0";
	innerBetweenWidth = "0.8";
	innerEndWidth = "0.0";

	hasOuter = false;
	outerStartColor = "0.00 0.00 0.90 0.0";
	outerBetweenColor = "0.50 0.00 0.90 0.8";
	outerEndColor = "1.00 1.00 1.00 0.8";
	outerStartWidth = "0.3";
	outerBetweenWidth = "0.25";
	outerEndWidth = "0.1";

//	bitmap = "share/textures/rotc/violetspark";
//	bitmapWidth = 0.10;
//	crossBitmap = "share/shapes/rotc/weapons/blaster/lasertail.red.cross";
//	crossBitmapWidth = 0.10;

	betweenFactor = 0.8;
	blendMode = 1;
};

datablock MultiNodeLaserBeamData(FrmBumblebeeSpore_LaserTrailZero)
{
	hasLine = true;
	lineColor	= "0.5 1.0 0.5 0.7";

	hasInner = true;
	innerColor = "0.5 1.0 0.5 0.7";
	innerWidth = "0.1";

	hasOuter = false;
	outerColor = "1.00 0.00 1.00 0.1";
	outerWidth = "0.10";

	//bitmap = "share/shapes/rotc/vehicles/team1scoutflyer/lasertrail";
	//bitmapWidth = 1;

	blendMode = 1;
	fadeTime = 200;
};

datablock OrbitProjectileData(FrmBumblebeeSpore)
{
   orbitRadius = 10;
   orbitSpeed = 15;
   maxSpeed = 200;
   maxTrackingAbility = 4;
	laserTail = FrmBumblebeeSpore_LaserTail;
	laserTailLen = 1;
	//laserTrail[0]   = FrmBumblebeeSpore_LaserTrailZero;
   lifetime = 0; // 0 == forever
   //projectileShapeName = "share/shapes/alux/light.dts";
   unlimitedBounces = true;
   bounceElasticity = 0.999;
};
