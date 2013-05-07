//------------------------------------------------------------------------------
// Alux Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Projectile impact decal

datablock DecalData(WpnStyckBulletHoleDecal)
{
	sizeX = "0.15";
	sizeY = "0.15";
	textureName = "share/textures/alux/styckhole";
	SelfIlluminated = false;
};

//-----------------------------------------------------------------------------
// laser tail...

datablock LaserBeamData(WpnStyckProjectileLaserTail)
{
	hasLine = true;
	lineStartColor	= "1.00 0.00 0.00 0.0";
	lineBetweenColor = "1.00 0.00 0.00 0.25";
	lineEndColor	  = "1.00 0.00 0.00 0.5";
	lineWidth		  = 2.0;

	hasInner = true;
	innerStartColor = "1.00 0.00 0.00 0.5";
	innerBetweenColor = "1.00 0.00 0.00 0.9";
	innerEndColor = "1.00 1.00 1.00 0.9";
	innerStartWidth = "0.05";
	innerBetweenWidth = "0.05";
	innerEndWidth = "0.05";

	hasOuter = false;
	outerStartColor = "0.00 0.00 0.90 0.0";
	outerBetweenColor = "0.50 0.00 0.90 0.8";
	outerEndColor = "1.00 1.00 1.00 0.8";
	outerStartWidth = "0.3";
	outerBetweenWidth = "0.25";
	outerEndWidth = "0.1";
	
//	bitmap = "share/shapes/rotc/weapons/blaster/lasertail.red";
//	bitmapWidth = 0.20;
//	crossBitmap = "share/shapes/rotc/weapons/blaster/lasertail.red.cross";
//	crossBitmapWidth = 0.10;

	betweenFactor = 0.5;
	blendMode = 1;
};

//-----------------------------------------------------------------------------
// laser trail

datablock MultiNodeLaserBeamData(WpnStyckProjectileLaserTrail)
{
	hasLine = true;
	lineColor	= "1.00 0.50 0.50 1.0";

	hasInner = true;
	innerColor = "1.00 0.00 0.00 0.5";
	innerWidth = "0.10";

	hasOuter = false;
	outerColor = "1.00 0.00 1.00 0.1";
	outerWidth = "0.90";

	//bitmap = "share/shapes/rotc/weapons/blaster/lasertrail.red";
	//bitmapWidth = 0.70;

	blendMode = 1;
	renderMode = $MultiNodeLaserBeamRenderMode::FaceViewer;
	fadeTime = 250;
    
	windCoefficient = 0.0;
    
	// node x movement...
	nodeMoveMode[0]     = $NodeMoveMode::None;
	nodeMoveSpeed[0]    = 6.0;
	nodeMoveSpeedAdd[0] = -12.0;
	// node y movement...
	nodeMoveMode[1]     = $NodeMoveMode::None;
	nodeMoveSpeed[1]    = 6.0;
	nodeMoveSpeedAdd[1] = -12.0;
	// node z movement...
	nodeMoveMode[2]     = $NodeMoveMode::None;
	nodeMoveSpeed[2]    = 6.0;
	nodeMoveSpeedAdd[2] = -12.0;
    
    nodeDistance = 5;
};

//-----------------------------------------------------------------------------
// hit enemy...

datablock ParticleData(WpnStyckProjectileHit_Particle)
{
	dragCoefficient    = 0.0;
	windCoefficient    = 0.0;
	gravityCoefficient	= 0.0;
	inheritedVelFactor	= 0.0;

	lifetimeMS			  = 250;
	lifetimeVarianceMS	= 0;

	useInvAlpha =  false;

	textureName	= "share/textures/alux/circle1";

	colors[0]	  = "1.0 1.0 1.0 1.0";
	colors[1]	  = "1.0 0.0 0.0 0.5";
	colors[2]	  = "1.0 0.0 0.0 0.0";
	sizes[0]		= 0.5;
	sizes[1]		= 1.0;
	sizes[2]		= 1.5;
	times[0]		= 0.0;
	times[1]		= 0.5;
	times[2]		= 1.0;

	allowLighting = false;
	renderDot = true;
};

datablock ParticleEmitterData(WpnStyckProjectileHit_Emitter)
{
	ejectionOffset	= 0;

	ejectionPeriodMS = 40;
	periodVarianceMS = 0;

	ejectionVelocity = 0.0;
	velocityVariance = 0.0;

	thetaMin			= 0.0;
	thetaMax			= 60.0;

	lifetimeMS		 = 100;

	particles = "WpnStyckProjectileHit_Particle";
};

datablock ExplosionData(WpnStyckProjectileHit)
{
	soundProfile = WpnTridentProjectileImpactSound;

	lifetimeMS = 450;

	particleEmitter = WpnStyckProjectileHit_Emitter;
	particleDensity = 1;
	particleRadius = 0;

	// Dynamic light
	lightStartRadius = 2;
	lightEndRadius = 0;
	lightStartColor = "1.0 1.0 1.0";
	lightEndColor = "1.0 1.0 1.0";
   lightCastShadows = false;
};

//-----------------------------------------------------------------------------
// impact...

datablock ParticleData(WpnStyckProjectileImpact_Smoke)
{
	dragCoeffiecient	  = 0.4;
	gravityCoefficient	= -0.4;
	inheritedVelFactor	= 0.025;

	lifetimeMS			  = 500;
	lifetimeVarianceMS	= 200;

	useInvAlpha =  true;

	textureName = "share/textures/rotc/smoke_particle";

	colors[0]	  = "1.0 1.0 1.0 0.5";
	colors[1]	  = "1.0 1.0 1.0 0.0";
	sizes[0]		= 1.0;
	sizes[1]		= 1.0;
	times[0]		= 0.0;
	times[1]		= 1.0;

	allowLighting = false;
};

datablock ParticleEmitterData(WpnStyckProjectileImpact_SmokeEmitter)
{
	ejectionOffset	= 0;

	ejectionPeriodMS = 40;
	periodVarianceMS = 0;

	ejectionVelocity = 2.0;
	velocityVariance = 0.1;

	thetaMin			= 0.0;
	thetaMax			= 60.0;

	lifetimeMS		 = 100;

	particles = "WpnStyckProjectileImpact_Smoke";
};

datablock ExplosionData(WpnStyckProjectileImpact : WpnStyckProjectileHit)
{
	soundProfile = WpnTridentProjectileImpactSound;

	lifetimeMS = 3000;

 	// shape...
	//explosionShape = "share/shapes/rotc/weapons/blaster/projectile.impact.red.dts";
	faceViewer = false;
	playSpeed = 0.4;
	sizes[0] = "1 1 1";
	sizes[1] = "1 1 1";
	times[0] = 0.0;
	times[1] = 1.0;

	emitter[0] = DefaultSmallWhiteDebrisEmitter;
	emitter[1] = WpnStyckProjectileImpact_SmokeEmitter;

	//debris = WpnStyckProjectileImpact_Debris;
	//debrisThetaMin = 0;
	//debrisThetaMax = 60;
	//debrisNum = 1;
	//debrisNumVariance = 1;
	//debrisVelocity = 10.0;
	//debrisVelocityVariance = 5.0;

	// Dynamic light
	lightStartRadius = 0;
	lightEndRadius = 0;
	lightStartColor = "1.0 0.0 0.0";
	lightEndColor = "0.0 0.0 0.0";
    lightCastShadows = false;

	shakeCamera = false;
};

//-----------------------------------------------------------------------------
// missed enemy...

datablock ExplosionData(WpnStyckProjectileMissedEnemyEffect)
{
	soundProfile = WpnTridentProjectileMissedEnemySound;

	// shape...
	//explosionShape = "share/shapes/rotc/effects/explosion2_white.dts";
	faceViewer	  = true;
	playSpeed = 8.0;
	sizes[0] = "0.07 0.07 0.07";
	sizes[1] = "0.01 0.01 0.01";
	times[0] = 0.0;
	times[1] = 1.0;

	// dynamic light...
	lightStartRadius = 0;
	lightEndRadius = 2;
	lightStartColor = "0.0 0.0 0.0";
	lightEndColor = "0.0 0.0 0.0";
    lightCastShadows = false;
};


