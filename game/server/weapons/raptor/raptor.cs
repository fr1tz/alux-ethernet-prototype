//------------------------------------------------------------------------------
// Revenge Of The Cats: Ethernet
// Copyright (C) 2008, mEthLab Interactive
//------------------------------------------------------------------------------

exec("./raptor.projectile.cs");
exec("./raptor.sfx.cs");

//-----------------------------------------------------------------------------
// fire particle emitter

datablock ParticleData(WpnRedRaptorFireEmitter_Particles)
{
	dragCoefficient       = 1;
	gravityCoefficient    = 0.0;
	windCoefficient       = 0.0;
	inheritedVelFactor    = 1.0;
	constantAcceleration  = 0.0;
	lifetimeMS            = 100;
	lifetimeVarianceMS    = 0;
	textureName           = "share/textures/rotc/smoke_particle";
	colors[0]             = "1.0 1.0 1.0 1.0";
	colors[1]             = "1.0 0.0 0.0 1.0";
	colors[2]             = "1.0 0.0 0.0 0.0";
	sizes[0]              = 0.5;
	sizes[1]              = 0.5;
	sizes[2]              = 0.0;
	times[0]              = 0.0;
	times[1]              = 0.5;
	times[2]              = 1.0;

};

datablock ParticleEmitterData(WpnRedRaptorFireEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 0;
	ejectionVelocity = 5.0*10;
	velocityVariance = 0.2;
	ejectionOffset   = 0;
	thetaMin         = 0;
	thetaMax         = 0;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvances = false;
	orientParticles  = false;
	lifetimeMS       = 0;
	particles        = "WpnRedRaptorFireEmitter_Particles";
};

datablock ParticleData(WpnBlueRaptorFireEmitter_Particles)
{
	dragCoefficient       = 1;
	gravityCoefficient    = 0.0;
	windCoefficient       = 0.0;
	inheritedVelFactor    = 1.0;
	constantAcceleration  = 0.0;
	lifetimeMS            = 100;
	lifetimeVarianceMS    = 0;
	textureName           = "share/textures/rotc/smoke_particle";
	colors[0]             = "1.0 1.0 1.0 1.0";
	colors[1]             = "0.0 0.0 1.0 1.0";
	colors[2]             = "0.0 0.0 1.0 0.0";
	sizes[0]              = 0.5;
	sizes[1]              = 0.5;
	sizes[2]              = 0.0;
	times[0]              = 0.0;
	times[1]              = 0.5;
	times[2]              = 1.0;

};

datablock ParticleEmitterData(WpnBlueRaptorFireEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 0;
	ejectionVelocity = 5.0*10;
	velocityVariance = 0.2;
	ejectionOffset   = 0;
	thetaMin         = 0;
	thetaMax         = 0;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvances = false;
	orientParticles  = false;
	lifetimeMS       = 0;
	particles        = "WpnBlueRaptorFireEmitter_Particles";
};

//------------------------------------------------------------------------------
// weapon image which does all the work...
// (images do not normally exist in the world, they can only
// be mounted on ShapeBase objects)

datablock ShapeBaseImageData(WpnRedRaptorImage)
{
	// add the WeaponImage namespace as a parent
	className = WeaponImage;

	// basic item properties
	shapeFile = "share/shapes/alux/raptor.image.dts";
	emap = true;

	// mount point & mount offset...
	mountPoint  = 0;
	offset      = "0 0 0";
	rotation    = "0 0 0";
   eyeOffset   = "0.15 0.2 0.0";
	eyeRotation = "0 0 0 0";

	// Adjust firing vector to eye's LOS point?
	correctMuzzleVector = true;

	usesEnergy = true;
	minEnergy = 2;

	projectile = WpnRedRaptorProjectile;

    // charging...
    minCharge = 0.4;
 
	// script fields...
	iconId = 5;
	armThread = "holdblaster";  // armThread to use when holding this weapon
	crosshair = "blaster"; // crosshair to display when holding this weapon
	
	//-------------------------------------------------
	// image states...
	//
		// preactivation...
		stateName[0]                     = "Preactivate";
		stateTransitionOnAmmo[0]         = "Activate";
		stateTransitionOnNoAmmo[0]       = "NoAmmo";

		// when mounted...
		stateName[1]                     = "Activate";
		stateTransitionOnTimeout[1]      = "Ready";
		stateTimeoutValue[1]             = 0.5;
		stateSequence[1]                 = "idle";
		stateSpinThread[1]               = "Stop";

		// ready to fire, just waiting for the trigger...
		stateName[2]                     = "Ready";
		stateTransitionOnNoAmmo[2]       = "NoAmmo";
		stateTransitionOnNotLoaded[2]    = "Disabled";
		stateTransitionOnTriggerDown[2]  = "Fire";
		stateArmThread[2]                = "holdblaster";
		stateSpinThread[2]               = "Stop";
		//stateSequence[2]                 = "idle";

		// fire!...
		stateName[3]                     = "Fire";
		stateTransitionOnTriggerUp[3]    = "KeepAiming";
		stateTransitionOnNoAmmo[3]       = "NoAmmo";
		stateTransitionOnTimeout[3]      = "Fire";
		stateTimeoutValue[3]             = 0.06;
		stateFire[3]                     = true;
		stateFireProjectile[3]           = WpnRedRaptorProjectile;
		stateRecoil[3]                   = MediumRecoil;
		stateAllowImageChange[3]         = false;
		stateEjectShell[3]               = true;
		stateArmThread[3]                = "aimblaster";
		stateSequence[3]                 = "fire";
		stateSound[3]                    = WpnRaptorFireSound;
		//stateEmitter[3]                  = WpnRedRaptorFireEmitter;
		stateEmitterNode[3]              = "fireparticles";
		stateEmitterTime[3]              = 0.1;
		stateSpinThread[3]               = "FullSpeed";
		stateScript[3]                   = "onFire";
		
		// after fire...
		stateName[4]                     = "AfterFire";
		stateTransitionOnTriggerUp[4]    = "KeepAiming";

		// keep aiming...
		stateName[5]                     = "KeepAiming";
		stateTransitionOnNoAmmo[5]       = "NoAmmo";
		stateTransitionOnTriggerDown[5]  = "Fire";
		stateTransitionOnTimeout[5]      = "Ready";
		stateTransitionOnNotLoaded[5]    = "Disabled";
		stateWaitForTimeout[5]           = false;
		stateTimeoutValue[5]             = 2.00;
		stateSpinThread[5]               = "Stop";
			
		// no ammo...
		stateName[6]                     = "NoAmmo";
        stateTransitionOnTriggerDown[6]  = "DryFire";
		stateTransitionOnAmmo[6]         = "Ready";
		stateTimeoutValue[6]             = 0.50;
		stateSequence[6]                 = "idle";
  
        // dry fire...
		stateName[7]                     = "DryFire";
		stateTransitionOnTriggerUp[7]    = "NoAmmo";
		stateSound[7]                    = WeaponEmptySound;
		//stateSequence[7]                 = "idle";

		// disabled...
		stateName[8]                     = "Disabled";
		stateTransitionOnLoaded[8]       = "Ready";
		stateAllowImageChange[8]         = false;
		//stateSequence[8]                 = "idle";
	//
	// ...end of image states
	//-------------------------------------------------
};

function WpnRedRaptorImage::getBulletSpread(%this, %obj)
{
   return 0.02;
}

function WpnRedRaptorImage::onMount(%this, %obj, %slot)
{
   Parent::onMount(%this, %obj, %slot);

   // Set up crosshair
   %client = %obj.client;
   if(!isObject(%client)) return;
   commandToClient(%client, 'Crosshair', 0);
   commandToClient(%client, 'Crosshair', 7, 2);
   commandToClient(%client, 'Crosshair', 2, 1, 64);
   commandToClient(%client, 'Crosshair', 3, 1, 20);
   commandToClient(%client, 'Crosshair', 5, "./rotc/ch1");
   commandToClient(%client, 'Crosshair', 1);

   // Set up inaccuracy
   %obj.setImageInaccuracy(%slot, "radiusmin", 1.0);
   %obj.setImageInaccuracy(%slot, "radiusmax", 20.0);
   %obj.setImageInaccuracy(%slot, "a1", 1.0);
   %obj.setImageInaccuracy(%slot, "a2", 1.0);
   %obj.setImageInaccuracy(%slot, "b1", 0.95);
   %obj.setImageInaccuracy(%slot, "b2", 0.0);
   %obj.setImageInaccuracy(%slot, "c", 10.0);
   %obj.setImageInaccuracy(%slot, "d", 0.25);
   %obj.setImageInaccuracy(%slot, "f1", 1.00);
   %obj.setImageInaccuracy(%slot, "f2", 0.40);
   %obj.setImageInaccuracy(%slot, "enabled", true);

   // Set up recoil
   %obj.setImageRecoilEnabled(%slot, true);
   %obj.setImageCurrentRecoil(%slot, 5);
   %obj.setImageMaxRecoil(%slot, 5);
   %obj.setImageRecoilAdd(%slot, 0);
   %obj.setImageRecoilDelta(%slot, -0);
}

//------------------------------------------------------------------------------

datablock ShapeBaseImageData(WpnBlueRaptorImage : WpnRedRaptorImage)
{
	shapeFile = "share/shapes/rotc/weapons/blaster/image2.blue.dts";
	projectile = WpnBlueRaptorProjectile;
	stateFireProjectile[3] = WpnBlueRaptorProjectile;
    stateEmitter[3] = WpnBlueRaptorFireEmitter;
};

function WpnBlueRaptorImage::getBulletSpread(%this, %obj)
{
   return WpnRedRaptorImage::getBulletSpread(%this, %obj);
}

function WpnBlueRaptorImage::onMount(%this, %obj, %slot)
{
   WpnRedRaptorImage::onMount(%this, %obj, %slot);
}


