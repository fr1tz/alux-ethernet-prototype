//------------------------------------------------------------------------------
// Alux Ethernet Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Projectile impact decal

datablock DecalData(WpnBadgerBulletHoleDecal)
{
	sizeX = "0.15";
	sizeY = "0.15";
	textureName = "share/textures/alux/badgerhole";
	SelfIlluminated = false;
};

//-----------------------------------------------------------------------------
// laser trail

datablock MultiNodeLaserBeamData(WpnBadgerProjectileLaserTrailOne)
{
	hasLine   = true;
	lineColor = "1.00 1.00 1.00 1.0";
    lineWidth = 1.0;

	hasInner = false;
	innerColor = "1.00 0.50 0.00 1.0";
	innerWidth = "0.08";

	hasOuter = false;
	outerColor = "1.00 0.00 0.00 0.75";
	outerWidth = "0.20";

	bitmap = "share/textures/alux/laser1";
	bitmapWidth = 0.2;

	blendMode = 1;
	renderMode = $MultiNodeLaserBeamRenderMode::FaceViewer;
	fadeTime = 100;

    windCoefficient = 0.0;

	// node x movement...
	nodeMoveMode[0]     = $NodeMoveMode::DynamicSpeed;
	nodeMoveSpeed[0]    = 16.0;
	nodeMoveSpeedAdd[0] = -32.0;
	// node y movement...
	nodeMoveMode[1]     = $NodeMoveMode::DynamicSpeed;
	nodeMoveSpeed[1]    = 16.0;
	nodeMoveSpeedAdd[1] = -32.0;
	// node z movement...
	nodeMoveMode[2]     = $NodeMoveMode::DynamicSpeed;
	nodeMoveSpeed[2]    = 16.0;
	nodeMoveSpeedAdd[2] = -32.0;

    nodeDistance = 1;
};

datablock MultiNodeLaserBeamData(WpnBadgerProjectileLaserTrailTwo)
{
	hasLine   = false;
	lineColor = "1.00 0.50 0.50 0.5";
	lineWidth = 2.0;

	hasInner = false;
	innerColor = "1.00 0.50 0.50 0.5";
	innerWidth = "0.08";

	hasOuter = false;
	outerColor = "1.00 0.00 0.00 0.75";
	outerWidth = "0.20";

	bitmap = "share/textures/rotc/sniper_trail";
	bitmapWidth = 0.15;

	blendMode = 1;
	renderMode = $MultiNodeLaserBeamRenderMode::FaceViewer;
	fadeTime = 500;

    windCoefficient = 0.0;

    // node x movement...
    nodeMoveMode[0]     = $NodeMoveMode::ConstantSpeed;
    nodeMoveSpeed[0]    = -0.5;
    nodeMoveSpeedAdd[0] =  1.0;
    // node y movement...
    nodeMoveMode[1]     = $NodeMoveMode::ConstantSpeed;
    nodeMoveSpeed[1]    = -0.5;
    nodeMoveSpeedAdd[1] =  1.0;
    // node z movement...
    nodeMoveMode[2]     = $NodeMoveMode::ConstantSpeed;
    nodeMoveSpeed[2]    = -0.5;
    nodeMoveSpeedAdd[2] =  1.0;

    nodeDistance = 2;
};

//-----------------------------------------------------------------------------
// hit enemy...

datablock ParticleData(WpnBadgerProjectileHit_Particle)
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
	colors[1]	  = "0.0 0.5 1.0 0.5";
	colors[2]	  = "0.0 0.5 1.0 0.0";
	sizes[0]		= 0.5;
	sizes[1]		= 1.0;
	sizes[2]		= 1.5;
	times[0]		= 0.0;
	times[1]		= 0.5;
	times[2]		= 1.0;

	allowLighting = false;
	renderDot = true;
};

datablock ParticleEmitterData(WpnBadgerProjectileHit_Emitter)
{
	ejectionOffset	= 0;

	ejectionPeriodMS = 40;
	periodVarianceMS = 0;

	ejectionVelocity = 0.0;
	velocityVariance = 0.0;

	thetaMin			= 0.0;
	thetaMax			= 60.0;

	lifetimeMS		 = 100;

	particles = "WpnBadgerProjectileHit_Particle";
};

datablock ExplosionData(WpnBadgerProjectileHit)
{
	soundProfile = WpnBadgerProjectileImpactSound;

	lifetimeMS = 450;

	particleEmitter = WpnBadgerProjectileHit_Emitter;
	particleDensity = 1;
	particleRadius = 0;

	// Dynamic light
	lightStartRadius = 2;
	lightEndRadius = 0;
	lightStartColor = "1.0 0.0 0.0";
	lightEndColor = "0.0 0.0 0.0";
    lightCastShadows = false;
};

//-----------------------------------------------------------------------------
// impact...

datablock ParticleData(WpnBadgerProjectileImpact_Smoke)
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
	sizes[1]		= 3.0;
	times[0]		= 0.0;
	times[1]		= 1.0;

	allowLighting = false;
};

datablock ParticleEmitterData(WpnBadgerProjectileImpact_SmokeEmitter)
{
	ejectionOffset	= 0;

	ejectionPeriodMS = 20;
	periodVarianceMS = 0;

	ejectionVelocity = 2.0;
	velocityVariance = 0.1;

	thetaMin			= 0.0;
	thetaMax			= 60.0;

	lifetimeMS		 = 100;

	particles = "WpnBadgerProjectileImpact_Smoke";
};

datablock DebrisData(WpnBadgerProjectileImpact_Debris)
{
	// shape...
	shapeFile = "share/shapes/rotc/misc/debris1.white.dts";

	// bounce...
	staticOnMaxBounce = true;
	numBounces = 5;

	// physics...
	gravModifier = 2.0;
	elasticity = 0.6;
	friction = 0.1;

	// spin...
	minSpinSpeed = 60;
	maxSpinSpeed = 600;

	// lifetime...
	lifetime = 4.0;
	lifetimeVariance = 1.0;
};

datablock ExplosionData(WpnBadgerProjectileImpact)
{
	soundProfile = WpnBadgerProjectileImpactSound;

	lifetimeMS = 150;

 	// shape...
	//explosionShape = "share/shapes/rotc/weapons/blaster/projectile.impact.red.dts";
	faceViewer = false;
	playSpeed = 0.4;
	sizes[0] = "1 1 1";
	sizes[1] = "1 1 1";
	times[0] = 0.0;
	times[1] = 1.0;

	particleEmitter = WpnBadgerProjectileHit_Emitter;
	particleDensity = 1;
	particleRadius = 0;

	emitter[0] = DefaultSmallWhiteDebrisEmitter;
	emitter[1] = WpnBadgerProjectileImpact_SmokeEmitter;

	//debris = WpnBadgerProjectileImpact_Debris;
	//debrisThetaMin = 0;
	//debrisThetaMax = 60;
	//debrisNum = 1;
	//debrisNumVariance = 1;
	//debrisVelocity = 10.0;
	//debrisVelocityVariance = 5.0;

	// Dynamic light
	lightStartRadius = 2;
	lightEndRadius = 0;
	lightStartColor = "1.0 1.0 1.0";
	lightEndColor = "1.0 1.0 1.0";
   lightCastShadows = false;

	shakeCamera = false;
};

//-----------------------------------------------------------------------------
// missed enemy...

datablock ExplosionData(WpnBadgerProjectileMissedEnemyEffect)
{
	soundProfile = WpnBadgerProjectileMissedEnemySound;

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


