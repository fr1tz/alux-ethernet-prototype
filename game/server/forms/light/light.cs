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
			%client.setNewbieHelp("You are in etherform! Press @bind34 inside a" SPC 
				(%client.team == $Team1 ? "red" : "blue") SPC "zone to change into CAT form.");
		}
		else if(%client.newbieHelpData_NeedsRepair && !%client.newbieHelpData_HasRepaired)
		{
			%client.setNewbieHelp("If you don't have enough health to change into CAT form," SPC
				"fly into one of your team's zones to regain your health.");
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
	hudImageNameFriendly = "~/client/ui/hud/pixmaps/teammate.etherform.png";
	hudImageNameEnemy = "~/client/ui/hud/pixmaps/enemy.etherform.png";
	
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
function FrmLight::onAdd(%this,%obj)
{
	Parent::onAdd(%this,%obj);
   FrmLight_updateProxyThread(%this, %obj);
   if(isObject(%obj.client))
   {
      %c = %obj.client;
      commandToClient(%c, 'Hud', "health", false);
      commandToClient(%c, 'Hud', "energy", true, "game/client/ui/hud/pixmaps/energy_meter.png");
   }
}

// *** Callback function: called by engine
function FrmLight::onTrigger(%this, %obj, %triggerNum, %val)
{
	if(%triggerNum == 0 && %val)
	{
      if(%obj.getEnergyLevel() < %this.maxEnergy)
      {
         %obj.client.play2D(BeepMessageSound);
         return;
      }

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
			//sourceSlot      = %slot;
			client	        = %obj.client;
		};
		MissionCleanup.add(%p);

      %obj.client.camera.setOrbitMode(%p, 0, 0, 10, 10, true);
      %obj.client.camera.setTransform(%obj.getTransform());
      %obj.client.control(%obj.client.camera);
      %obj.client.player = %p;

      %obj.schedule(0, "delete");
	}

	if(%triggerNum == 1 && %val)
      %obj.setVelocity("0 0 0");
}

function FrmLight_updateProxyThread(%this, %obj)
{
   schedule(32, %obj, "FrmLight_updateProxyThread", %this, %obj);

   %client = %obj.client;
   if(!isObject(%client) || !isObject(%client.proxy))
      return;

   %prevSpawnError = %client.spawnError;

   %eyeVec = %obj.getEyeVector();
   %start = %obj.getWorldBoxCenter();
   %end = VectorAdd(%start, VectorScale(%eyeVec, 9999));

   %c = containerRayCast(%start, %end, $TypeMasks::TerrainObjectType |
      $TypeMasks::InteriorObjectType | $TypeMasks::ShapeBaseObjectType , %obj);

   if(!%c)
   {
      %client.proxy.removeClientFromGhostingList(%client);
      %client.proxy.setTransform("0 0 0");
      return;
   }

   if(%obj.getEnergyLevel() < %this.maxEnergy)
   {
      %client.spawnError = "Not enough energy to materialize.";
      %client.proxy.shapeFxSetColor(0, 2);
      %client.proxy.shapeFxSetColor(1, 2);
   }
   else
      %client.spawnError = "";

   %x = getWord(%c,1); %x = mFloor(%x); //%x -= (%x % 2);
   %y = getWord(%c,2); %y = mFloor(%y); //%y -= (%y % 2);
   %z = getWord(%c,3);
   %pos = %x SPC %y SPC %z;
   %normal = getWord(%c,4) SPC getWord(%c,5) SPC getWord(%c,6);
   if(%pos $= %client.proxy.getPosition()
   && (%client.spawnError $= %prevSpawnError))
      return;

   %transform = %pos SPC %normal SPC "0";
   if(%client.proxy.getDataBlock().isMethod("adjustTransform"))
   {
      %transform = %client.proxy.getDataBlock().adjustTransform(
         %pos, %normal, %eyeVec);
   }
   %client.proxy.addClientToGhostingList(%client);
   %client.proxy.setTransform(%transform);

   if(%obj.getEnergyLevel() < %this.maxEnergy)
      return;

   %client.spawnError = "The selected form can't materialize";
   if(%client.proxy.getDataBlock().form.isMethod("canMaterialize"))
   {
      %client.spawnError = %client.proxy.getDataBlock().form.canMaterialize(
         %client, %pos, %normal, %transform);
   }

   if(%client.spawnError $= "")
   {
      %client.proxy.shapeFxSetColor(0, 3);
      %client.proxy.shapeFxSetColor(1, 3);
   }
   else
   {
      %client.proxy.shapeFxSetColor(0, 1);
      %client.proxy.shapeFxSetColor(1, 1);
   }
}
