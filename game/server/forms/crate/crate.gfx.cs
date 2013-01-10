//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

datablock ParticleData(FrmCrate_DamageEmitter_Particle)
{
	spinSpeed = 200;
	spinRandomMin = -200.0;
	spinRandomMax =  200.0;
	dragCoefficient		= 1;
	gravityCoefficient	= 3.0;
	windCoefficient		= 0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration = 0.0;
	lifetimeMS			  = 1000;
	lifetimeVarianceMS	= 0;
	textureName = "share/textures/alux/grid1";
	colors[0]	  = "1.0 0.0 0.0 1.0";
	colors[1]	  = "1.0 0.0 0.0 1.0";
	colors[2]	  = "1.0 0.0 0.0 0.0";
	sizes[0]		= 0.3;
	sizes[1]		= 0.3;
	sizes[2]		= 0.0;
	times[0]		= 0.0;
	times[1]		= 0.5;
	times[2]		= 1.0;
	useInvAlpha =  false;
	renderDot = true;
	allowLighting = false;
};

datablock ParticleEmitterData(FrmCrate_DamageEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 0;
	ejectionVelocity = 15.0;
	velocityVariance = 5.0;
	ejectionOffset	= 0.0;
	thetaMin			= 0;
	thetaMax			= 50;
	phiReferenceVel  = 0;
	phiVariance		= 360;
	lifetimeMS		 = 0; // forever
	lifetimeVarianceMS = 0;
	overrideAdvances = false;
	orientParticles  = true;
	particles = "FrmCrate_DamageEmitter_Particle";
};
