//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

datablock StaticShapeData(FrmCrate)
{
	//category = "Blueprints"; // for the mission editor
	//className = Blueprint;

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

	maxDamage = 100;
	damageBuffer = 0;
	maxEnergy = 100;

	shapeFxTexture[0] = "share/textures/rotc/connection2.png";
	shapeFxTexture[1] = "share/textures/rotc/connection.png";
	shapeFxTexture[2] = "share/textures/rotc/barrier.green.png";
	shapeFxTexture[3] = "share/textures/rotc/armor.white.png";
	shapeFxTexture[4] = "share/textures/rotc/armor.orange.png";

	shapeFxColor[0] = "1.0 1.0 1.0 1.0";
	shapeFxColor[1] = "1.0 0.0 0.0 1.0";
	shapeFxColor[2] = "1.0 0.5 0.0 1.0";
	shapeFxColor[3] = "0.0 0.5 1.0 1.0";
	shapeFxColor[4] = "1.0 0.0 0.0 1.0";
	shapeFxColor[5] = "1.0 0.5 0.5 1.0";
	shapeFxColor[6] = "0.0 1.0 0.0 1.0";
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
      createExplosion(FrmLightProjectileExplosion, %obj.getPosition(), "0 0 1");
      %obj.delete();
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
		return "You can not manifest in an enemy zone!";
	}
	else if(%inOwnZone && !%inOwnTerritory)
	{
		return "This is not a territory zone!";
	}
	else if(!%inOwnZone)
	{
		return "You can only manifest in your team's territory zones!";
	}
	else if(%zoneBlocked)
	{
		return "This zone is currently blocked!";
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
   %player.setTransform(%pos);
   //%player.setPosition(%pos);
   return %player;
}

function FrmCrate::dematerialize(%this, %obj)
{
   %obj.schedule(0, "delete");
}
