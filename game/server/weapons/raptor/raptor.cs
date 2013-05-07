//------------------------------------------------------------------------------
// Alux Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

exec("./raptor.sfx.cs");
exec("./raptor.projectile.sfx.cs");
exec("./raptor.projectile.gfx.cs");
exec("./raptor.projectile.cs");
exec("./raptor.spore.cs");

//-----------------------------------------------------------------------------
// fire particle emitter

datablock ParticleData(WpnRaptorFireEmitter_Particles)
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

datablock ParticleEmitterData(WpnRaptorFireEmitter)
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
	particles        = "WpnRaptorFireEmitter_Particles";
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

datablock ShapeBaseImageData(WpnRaptorImage)
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
	minEnergy = WpnRaptorProjectile.energyDrain-1;

	projectile = WpnRaptorProjectile;

    // charging...
    minCharge = 0.4;
 
	// script fields...
	iconId = 5;
	armThread = "holdblaster";  // armThread to use when holding this weapon
	crosshair = "blaster"; // crosshair to display when holding this weapon
   reloadTime = 2000;
	
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
		stateTimeoutValue[3]             = 0.08;
		stateFire[3]                     = true;
		stateFireProjectile[3]           = WpnRaptorProjectile;
		stateRecoil[3]                   = MediumRecoil;
		stateAllowImageChange[3]         = false;
		stateEjectShell[3]               = true;
		stateArmThread[3]                = "aimblaster";
		stateSequence[3]                 = "fire";
		stateSound[3]                    = WpnRaptorFireSound;
		//stateEmitter[3]                  = WpnRaptorFireEmitter;
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

function WpnRaptorImage::onMount(%this, %obj, %slot)
{
   Parent::onMount(%this, %obj, %slot);

   // Set up inaccuracy
   %obj.setImageInaccuracy(%slot, "radiusmin", 2.0);
   %obj.setImageInaccuracy(%slot, "radiusmax", 20.0);
   %obj.setImageInaccuracy(%slot, "a1", 1.0);
   %obj.setImageInaccuracy(%slot, "a2", 1.0);
   %obj.setImageInaccuracy(%slot, "b1", 0.95);
   %obj.setImageInaccuracy(%slot, "b2", 0.0);
   %obj.setImageInaccuracy(%slot, "c", 10.0);
   %obj.setImageInaccuracy(%slot, "d", 0.3);
   %obj.setImageInaccuracy(%slot, "f1", 1.00);
   %obj.setImageInaccuracy(%slot, "f2", 0.40);
   %obj.setImageInaccuracy(%slot, "enabled", true);

   // Set up recoil
   %obj.setImageRecoilEnabled(%slot, true);
   %obj.setImageCurrentRecoil(%slot, 7);
   %obj.setImageMaxRecoil(%slot, 7);
   %obj.setImageRecoilAdd(%slot, 0);
   %obj.setImageRecoilDelta(%slot, -0);

   %client = %obj.client;
   if(!isObject(%client)) return;

   // Set up HUD
   commandToClient(%client, 'Hud', "energy", true, "share/hud/alux/ammobar.universal.32.png");

   // Set up crosshair
   commandToClient(%client, 'Crosshair', 0);
   commandToClient(%client, 'Crosshair', 7, 2);
   commandToClient(%client, 'Crosshair', 2, 1, 64);
   commandToClient(%client, 'Crosshair', 3, 1, 20);
   commandToClient(%client, 'Crosshair', 5, "./rotc/ch1");
   commandToClient(%client, 'Crosshair', 1);
}


