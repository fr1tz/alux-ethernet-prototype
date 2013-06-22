//------------------------------------------------------------------------------
// Alux Ethernet Prototype
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
// laser trail

datablock MultiNodeLaserBeamData(WpnStyckProjectileLaserTrailOne)
{
	hasLine   = false;
	lineColor = "1.00 1.00 1.00 1.0";
   lineWidth = 1.0;

	hasInner = true;
	innerColor = "1.00 1.00 1.00 1.0";
	innerWidth = "0.2";

	hasOuter = true;
	outerColor = "1.00 1.00 1.00 1.0";
	outerWidth = "0.2";

	bitmap = "share/textures/alux/laser0";
	bitmapWidth = 1.0;

	blendMode = 1;
	renderMode = $MultiNodeLaserBeamRenderMode::FaceViewer;
	fadeTime = 100;

   windCoefficient = 0.0;

	// node x movement...
	nodeMoveMode[0]     = $NodeMoveMode::None;
	nodeMoveSpeed[0]    = 16.0;
	nodeMoveSpeedAdd[0] = -32.0;
	// node y movement...
	nodeMoveMode[1]     = $NodeMoveMode::None;
	nodeMoveSpeed[1]    = 16.0;
	nodeMoveSpeedAdd[1] = -32.0;
	// node z movement...
	nodeMoveMode[2]     = $NodeMoveMode::None;
	nodeMoveSpeed[2]    = 16.0;
	nodeMoveSpeedAdd[2] = -32.0;

	nodeDistance = 4;
};

datablock MultiNodeLaserBeamData(WpnStyckProjectileLaserTrailTwo)
{
	hasLine   = false;
	lineColor = "1.00 0.00 0.00 1.0";
   lineWidth = 1.0;

	hasInner = false;
	innerColor = "1.00 0.00 0.00 1.0";
	innerWidth = "0.38";

	hasOuter = false;
	outerColor = "1.00 0.00 0.00 1.0";
	outerWidth = "0.35";

	bitmap = "share/textures/alux/laser4";
	bitmapWidth = 1.7;

	blendMode = 1;
	renderMode = $MultiNodeLaserBeamRenderMode::FaceViewer;
	fadeTime = 125;

    windCoefficient = 0.0;

	// node x movement...
	nodeMoveMode[0]     = $NodeMoveMode::None;
	nodeMoveSpeed[0]    = 16.0;
	nodeMoveSpeedAdd[0] = -32.0;
	// node y movement...
	nodeMoveMode[1]     = $NodeMoveMode::None;
	nodeMoveSpeed[1]    = 16.0;
	nodeMoveSpeedAdd[1] = -32.0;
	// node z movement...
	nodeMoveMode[2]     = $NodeMoveMode::None;
	nodeMoveSpeed[2]    = 16.0;
	nodeMoveSpeedAdd[2] = -32.0;

	nodeDistance = 4;
};

datablock MultiNodeLaserBeamData(WpnStyckProjectileLaserTrailThree)
{
	hasLine = false;
	lineColor	= "0.00 1.00 0.00 0.80";

	hasInner = true;
	innerColor = "1.00 1.00 1.00 0.10";
	innerWidth = "0.3";

	hasOuter = false;
	outerColor = "1.00 1.00 1.00 0.02";
	outerWidth = "0.05";

	//bitmap = "share/textures/rotc/sniper_trail";
	//bitmapWidth = 0.3;

	blendMode = 1;
 	renderMode = $MultiNodeLaserBeamRenderMode::FaceViewer;
	fadeTime = 400;

    windCoefficient = 0.0;

	// node x movement...
	nodeMoveMode[0]     = $NodeMoveMode::ConstantSpeed;
	nodeMoveSpeed[0]    = 1.0;
	nodeMoveSpeedAdd[0] = -2.0;
	// node y movement...
	nodeMoveMode[1]     = $NodeMoveMode::ConstantSpeed;
	nodeMoveSpeed[1]    = 1.0;
	nodeMoveSpeedAdd[1] = -2.0;
	// node z movement...
	nodeMoveMode[2]     = $NodeMoveMode::ConstantSpeed;
	nodeMoveSpeed[2]    = 1.0;
	nodeMoveSpeedAdd[2] = -2.0;

	nodeDistance = 4;
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
	soundProfile = WpnStyckProjectileImpactSound;

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
	soundProfile = WpnStyckProjectileImpactSound;

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
	soundProfile = WpnStyckProjectileMissedEnemySound;

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


