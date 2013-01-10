//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// fire particle emitter

datablock ParticleData(WpnStyckFireEmitter_Particles)
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

datablock ParticleEmitterData(WpnStyckFireEmitter)
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
	particles        = "WpnStyckFireEmitter_Particles";
};

datablock ParticleData(WpnBlueTridentFireEmitter_Particles)
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

datablock ParticleEmitterData(WpnBlueTridentFireEmitter)
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
	particles        = "WpnBlueTridentFireEmitter_Particles";
};

//------------------------------------------------------------------------------
// weapon image which does all the work...
// (images do not normally exist in the world, they can only
// be mounted on ShapeBase objects)

datablock ShapeBaseImageData(WpnStyckImage)
{
	// add the WeaponImage namespace as a parent
	className = WeaponImage;

	// basic item properties
	shapeFile = "share/shapes/alux/styck.image.dts";
	emap = true;

	// mount point & mount offset...
	mountPoint  = 0;
	offset      = "0 0 0";
	rotation    = "0 0 0";
	eyeOffset	= "0.05 0.07 -0.03";
	eyeRotation = "0 0 0";

	// Adjust firing vector to eye's LOS point?
	correctMuzzleVector = true;

	usesEnergy = true;
	minEnergy = 27;

	projectile = WpnStyckProjectile;

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
		stateSpinThread[1]               = "SpinDown";

		// ready to fire, just waiting for the trigger...
		stateName[2]                     = "Ready";
		stateTransitionOnNoAmmo[2]       = "NoAmmo";
		stateTransitionOnNotLoaded[2]    = "Disabled";
		stateTransitionOnTriggerDown[2]  = "Fire";
		stateArmThread[2]                = "holdrifle";
		stateSpinThread[2]               = "Stop";
		//stateSequence[2]                 = "idle";

		// fire!...
		stateName[3]                     = "Fire";
		stateTransitionOnTimeout[3]      = "AfterFire";
		stateTimeoutValue[3]             = 0.8;
		stateFire[3]                     = true;
		stateFireProjectile[3]           = WpnStyckProjectile;
		stateRecoil[3]                   = MediumRecoil;
		stateAllowImageChange[3]         = false;
		stateEjectShell[3]               = true;
		stateArmThread[3]                = "aimrifle";
		stateSequence[3]                 = "fire";
		stateSound[3]                    = WpnStyckFireSound;
		stateEmitter[3]                  = WpnStyckFireEmitter;
		stateEmitterNode[3]              = "fireparticles";
		stateEmitterTime[3]              = 0.1;
		stateSpinThread[3]               = "Stop";
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

function WpnStyckImage::getBulletSpread(%this, %obj)
{
   return 0.12;
}

function WpnStyckImage::onMount(%this, %obj, %slot)
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
   %obj.setImageInaccuracy(%slot, "radiusmin", 0.30);
   %obj.setImageInaccuracy(%slot, "radiusmax", 20.0);
   %obj.setImageInaccuracy(%slot, "a1", 1.0);
   %obj.setImageInaccuracy(%slot, "a2", 1.0);
   %obj.setImageInaccuracy(%slot, "b1", 0.96);
   %obj.setImageInaccuracy(%slot, "b2", 0.0);
   %obj.setImageInaccuracy(%slot, "c", 10.0);
   %obj.setImageInaccuracy(%slot, "d", 0.20);
   %obj.setImageInaccuracy(%slot, "f1", 1.00);
   %obj.setImageInaccuracy(%slot, "f2", 0.00);
   %obj.setImageInaccuracy(%slot, "enabled", true);

   // Set up recoil
   %obj.setImageRecoilEnabled(%slot, true);
   %obj.setImageCurrentRecoil(%slot, 100);
   %obj.setImageMaxRecoil(%slot, 100);
   %obj.setImageRecoilAdd(%slot, 0);
   %obj.setImageRecoilDelta(%slot, -0);
}


