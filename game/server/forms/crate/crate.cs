//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Placement proxy for actual form

datablock StaticShapeData(FrmCrateProxy)
{
   form = FrmCrate; // script field

	shadowEnable = false;
	shapeFile = "share/shapes/alux/crate.dts";
   emap = false;
   hudImageNameFriendly = "~/client/ui/hud/pixmaps/hudfill.png";
   hudImageNameEnemy = "~/client/ui/hud/pixmaps/hudfill.png";

	shapeFxTexture[0] = "share/textures/alux/grid1.png";
	shapeFxTexture[1] = "share/textures/alux/circuit1.png";

	shapeFxColor[0] = "1.0 1.0 1.0 1.0";
	shapeFxColor[1] = "1.0 0.0 0.0 1.0";
	shapeFxColor[2] = "1.0 1.0 0.0 1.0";
	shapeFxColor[3] = "0.0 1.0 0.0 1.0";
};

function FrmCrateProxy::adjustTransform(%this, %pos, %normal, %eyeVec)
{
   %vec = getWord(%eyeVec,0) SPC getWord(%eyeVec,1) SPC 0;
   %yaw = getWord(getAnglesFromVector(%vec), 0);
   %yaw = (%yaw*180.0)/3.14159265358979323846; // rad to deg
   %yaw = mFloor(%yaw);
   if((%yaw % 90) > 45)
      %yaw += 90;
   else
      %yaw += 0;
   %yaw -= (%yaw % 90);
   %yaw = (%yaw*3.14159265358979323846)/180.0; // deg to rad
   %vec = getVectorFromAngles(%yaw, 0);
   %transform = createOrientFromDir(%vec);
   %transform = setWord(%transform, 0, getWord(%pos, 0));
   %transform = setWord(%transform, 1, getWord(%pos, 1));
   %transform = setWord(%transform, 2, getWord(%pos, 2));
   return %transform;
}

//------------------------------------------------------------------------------

datablock StaticShapeData(FrmCrate)
{
   proxy = FrmCrateProxy; // script field

	//category = "Blueprints"; // for the mission editor
	//className = Blueprint;

   dynamicType = $TypeMasks::DamagableItemObjectType;

	fistPersonOnly = true;

	cameraDefaultFov = 110.0;
	cameraMinFov     = 110.0;
	cameraMaxFov     = 130.0;
	cameraMinDist    = 2;
	cameraMaxDist    = 3;
	
	shadowEnable = true;
	
	shapeFile = "share/shapes/alux/crate.dts";
   emap = true;

   hudImageNameFriendly = "~/client/ui/hud/pixmaps/hudfill.png";

	maxDamage = 400;
	damageBuffer = 0;
	maxEnergy = 100;

	//cloakTexture = "share/shapes/rotc/effects/explosion_white.png";
	shapeFxTexture[0] = "share/textures/alux/shiny.png";
	shapeFxTexture[1] = "share/textures/alux/grid1.png";
	shapeFxTexture[2] = "share/textures/alux/grid2.png";
	shapeFxColor[0] = "1.0 1.0 1.0 1.0";

	// damage info eyecandy...
	//damageBufferParticleEmitter = RedCatDamageBufferEmitter;
	//repairParticleEmitter = RedCatRepairEmitter;
	//bufferRepairParticleEmitter = RedCatBufferRepairEmitter;
	damageParticleEmitter = FrmCrate_DamageEmitter;
};

function FrmCrate::onAdd(%this, %obj)
{
	//error("FrmCrate::onAdd()");

	Parent::onAdd(%this, %obj);
	
	//%obj.startFade(0, 0, true);
	//%obj.shapeFxSetActive(0, true, true);
	//%obj.shapeFxSetActive(1, true, true);
}

// *** Callback function:
// Invoked by ShapeBase code whenever the object's damage level changes
function FrmCrate::onDamage(%this, %obj, %delta)
{
	%totalDamage = %obj.getDamageLevel();
	if(%totalDamage >= %this.maxDamage)
	{
      createExplosion(FrmCrateExplosion, %obj.getPosition(), "0 0 1");
      %obj.delete();
	}
}

// *** Callback function: called by engine
function FrmCrate::onTrigger(%this, %obj, %triggerNum, %val)
{
	if(%triggerNum == 1 && %val)
	{
      %obj.client.leaveForm();
   }
}

function FrmCrate::canMaterialize(%this, %client, %pos, %normal, %transform)
{
	%ownTeamId = %client.player.getTeamId();

	%inOwnZone = false;
	%inOwnTerritory = false;
	%inEnemyZone = false;

	InitContainerRadiusSearch(%pos, 0.0001, $TypeMasks::TacticalSurfaceObjectType);
	while((%srchObj = containerSearchNext()) != 0)
	{
		// object actually in this zone?
		//%inSrchZone = false;
		//for(%i = 0; %i < %srchObj.getNumObjects(); %i++)
		//{
      //   error(%srchObj.getObject(%i).getDataBlock().getName());
		//	if(%srchObj.getObject(%i) == %this.player)
		//	{
		//		%inSrchZone = true;
		//		break;
		//	}
		//}
		//if(!%inSrchZone)
		//	continue;

		%zoneTeamId = %srchObj.getTeamId();
		%zoneBlocked = %srchObj.zBlocked;

		if(%zoneTeamId != %ownTeamId && %zoneTeamId != 0)
		{
			%inEnemyZone = true;
			break;
		}
		else if(%zoneTeamId == %ownTeamId)
		{
			%inOwnZone = true;
			if(%srchObj.getDataBlock().getName() $= "TerritorySurface"
			|| %srchObj.getDataBlock().isTerritoryZone)
				%inOwnTerritory = true;
		}
	}

   %spawn = true;
	if(%inEnemyZone)
	{
		return "You cannot materialize on an enemy's surface!";
	}
	else if(%inOwnZone && !%inOwnTerritory)
	{
		return "This surface can't be used to materialize!";
	}
	else if(!%inOwnZone)
	{
		return "You can only materialize on your team's surfaces!";
	}
	else if(%zoneBlocked)
	{
		return "This surface is currently blocked!";
	}

   return "";
}

function FrmCrate::materialize(%this, %client, %pos, %normal, %transform)
{
	%player = new StaticShape() {
	  dataBlock = %this;
	  client = %client;
     teamId = %client.team.teamId;
   };
   MissionCleanup.add(%player);
   %this.materializeFx(%player);
	%player.playAudio(0, CatSpawnSound);
   return %player;
}

function FrmCrate::materializeFx(%this, %obj)
{
   //%obj.startFade(1000, 0, false);

	%obj.shapeFxSetTexture(0, 1);
	%obj.shapeFxSetColor(0, 0);
	%obj.shapeFxSetBalloon(0, 1.0, 0.0);
	%obj.shapeFxSetFade(0, 1.0, -0.5);
	%obj.shapeFxSetActive(0, true, true);

	%obj.shapeFxSetTexture(1, 0);
	%obj.shapeFxSetColor(1, 0);
	%obj.shapeFxSetBalloon(1, 1.0, 0.0);
	%obj.shapeFxSetFade(1, 1.0, -0.5);
	%obj.shapeFxSetActive(1, true, true);

	%obj.shapeFxSetTexture(2, 2);
	%obj.shapeFxSetColor(2, 0);
	%obj.shapeFxSetBalloon(2, 1.0, 0.0);
	%obj.shapeFxSetFade(2, 1.0, -1.0);
	%obj.shapeFxSetActive(2, true, true);

	//%obj.shapeFxSetTexture(3, 2);
	//%obj.shapeFxSetColor(3, 0);
	//%obj.shapeFxSetBalloon(3, 5.0, -20.0);
	//%obj.shapeFxSetFade(3, 1.0, -1.0);
	//%obj.shapeFxSetActive(3, true, false);
}

function FrmCrate::dematerialize(%this, %obj)
{
   createExplosion(FrmCrateExplosion, %obj.getPosition(), "0 0 1");
   %obj.schedule(0, "delete");
}
