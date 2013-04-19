//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

//-----------------------------------------------------------------------------

function EtherformData::useWeapon(%this, %obj, %nr)
{
	%client = %obj.client.changeInventory(%nr);
}

function EtherformData::damage(%this, %obj, %sourceObject, %pos, %damage, %damageType)
{
	// ignore damage
}

function EtherformData::onAdd(%this, %obj)
{
	Parent::onAdd(%this, %obj);

	// start singing...
	%obj.playAudio(1, FrmLightSingSound);

	// Make sure grenade ammo bar is not visible...
	messageClient(%obj.client, 'MsgGrenadeAmmo', "", 1);

	// lights...
	if(%obj.getTeamId() == 1)
		%obj.mountImage(FrmLightLightImage, 3);
	else
		%obj.mountImage(FrmLightLightImage, 3);

	%obj.client.inventoryMode = "show";
	%obj.client.displayInventory();
	
	if($Server::NewbieHelp && isObject(%obj.client))
	{
		%client = %obj.client;
		if(!%client.newbieHelpData_HasManifested)
		{
			%client.setNewbieHelp("You are currently pure light! Aim at a" SPC
				(%client.team == $Team1 ? "magenta" : "cyan") @
            "-colored surface and press @bind16 to materialize.");
		}
		else
		{
			%client.setNewbieHelp("random", false);
		}
	}			
}

function EtherformData::onDamage(%this, %obj, %delta)
{
	Parent::onDamage(%this, %obj, %delta);
}

//-----------------------------------------------------------------------------

datablock EtherformData(FrmLight)
{
   allowColorization = true;

	hudImageNameFriendly = "~/client/ui/hud/pixmaps/black.png";
	hudImageNameEnemy = "~/client/ui/hud/pixmaps/black.png";
	
	thirdPersonOnly = true;

    //category = "Vehicles"; don't appear in mission editor
	shapeFile = "share/shapes/alux/light.dts";
	emap = true;
 
	cameraDefaultFov = 110.0;
	cameraMinFov     = 110.0;
	cameraMaxFov     = 130.0;
	cameraMinDist    = 2;
	cameraMaxDist    = 3;
	
	//renderWhenDestroyed = false;
	//explosion = FlyerExplosion;
	//defunctEffect = FlyerDefunctEffect;
	//debris = BomberDebris;
	//debrisShapeName = "share/shapes/rotc/vehicles/bomber/vehicle.dts";

	mass = 90;
	drag = 0.99;
	density = 10;

	maxDamage = 0;
	damageBuffer = 0;
	maxEnergy = 100;

	damageBufferRechargeRate = 0.15;
	damageBufferDischargeRate = 0.05;
	energyRechargeRate = 0.5;
 
    // collision box...
    boundingBox = "1.0 1.0 1.0";
 
    // etherform movement...
    accelerationForce = 100;

	// impact damage...
	minImpactSpeed = 1;		// If hit ground at speed above this then it's an impact. Meters/second
	speedDamageScale = 0.0;	// Dynamic field: impact damage multiplier

	// damage info eyecandy...
//	damageBufferParticleEmitter = FrmLightDamageBufferEmitter;
//	repairParticleEmitter = FrmLightRepairEmitter;
//	bufferRepairParticleEmitter = FrmLightBufferRepairEmitter;

	// laser trail...
	laserTrail[0] = FrmLight_LaserTrailOne;
	laserTrail[1] = FrmLight_LaserTrailTwo;

	// contrail...
	minTrailSpeed = 1;
	//particleTrail = FrmLight_ContrailEmitter;
	
	// various emitters...
	//forwardJetEmitter = FlyerJetEmitter;
	//downJetEmitter = FlyerJetEmitter;

	//
//	jetSound = Team1ScoutFlyerThrustSound;
//	engineSound = EtherformSound;
	softImpactSound = FrmLightImpactSound;
	hardImpactSound = FrmLightImpactSound;
	//wheelImpactSound = WheelImpactSound;
};

// callback function: called by engine
function FrmLight::onAdd(%this, %obj)
{
	Parent::onAdd(%this,%obj);

   %obj.spores = new SimSet();

   if(isObject(%obj.client))
   {
      %c = %obj.client;
      %c.spawnError = "Please wait...";
      commandToClient(%c, 'Hud', "health", false);
      commandToClient(%c, 'Hud', "energy", true, "game/client/ui/hud/pixmaps/energy_meter.png");
   }

   //schedule(1000, %obj, "FrmLight_updateProxyThread", %this, %obj);
   //schedule(0, %obj, "FrmLight_createSpores", %this, %obj);
}

// callback function: called by engine
function FrmLight::onRemove(%this, %obj)
{
   if(isObject(%obj.spores))
   {
   	for(%idx = %obj.spores.getCount()-1; %idx >= 0; %idx-- )
      {
         %spore = %obj.spores.getObject(%idx);
         %spore.delete();
      }
      %obj.spores.delete();
   }
}

// *** Callback function: called by engine
function FrmLight::onTrigger(%this, %obj, %triggerNum, %val)
{
	if(%triggerNum == 0 && %val)
	{
      if(%obj.client.spawnError !$= "")
      {
         %obj.client.play2D(BeepMessageSound);
         return;
      }

      %obj.client.spawnForm();

      return;

      %pos = %obj.getWorldBoxCenter();
      %vec = %obj.getEyeVector();
 		%vel = VectorScale(%vec, FrmLightProjectile.muzzleVelocity);

		// create the projectile object...
		%p = new Projectile() {
			dataBlock       = FrmLightProjectile;
			teamId          = %obj.teamId;
			initialVelocity = %vel;
			initialPosition = %pos;
			sourceObject    = %obj;
			sourceSlot      =  0;
			client	       = %obj.client;
		};
		MissionCleanup.add(%p);

      %obj.client.camera.setOrbitMode(%p, %obj.getTransform(), 0, 10, 10, true);
      %obj.client.camera.setTransform(%obj.getTransform());
      %obj.client.control(%obj.client.camera);
      %obj.client.player = %p;

      %obj.schedule(0, "delete");
	}

	if(%triggerNum == 1 && %val)
      %obj.setVelocity("0 0 0");
}

// called by ShapeBase::impulse() script function
function FrmLight::impulse(%this, %obj, %position, %impulseVec, %src)
{
   return; // ignore impulses
}

// called by script code
function FrmLight::spawnSpore(%this, %obj, %data)
{
   return;

   %vel = getRandom() SPC getRandom() SPC getRandom();
   %vel = VectorScale(%vel, 10);
	%p = new OrbitProjectile() {
		dataBlock       = %data;
		teamId          = %obj.teamId;
		initialVelocity = %vel;
		initialPosition = %obj.getPosition();
		sourceObject    = %obj;
		sourceSlot      = 0;
		client	        = %obj.client;
	};
	MissionCleanup.add(%p);
   %p.setTarget(%obj);
   //%p.setTrackingAbility(%data.maxTrackingAbility);
   %obj.spores.add(%p);
}


// called by script code
function FrmLight_createSpores(%this, %obj)
{
   for(%form = 1; %form <= $Server::Game.formCount; %form++)
   {
      %spore = $Server::Game.form[%form].spore;
      for(%i = 0; %i < %obj.client.inventory.form[%form]; %i++)
         %obj.getDataBlock().spawnSpore(%obj, %spore);
   }
}
