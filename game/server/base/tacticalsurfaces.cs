//------------------------------------------------------------------------------
// Alux Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

//
// the following dynamic fields can be added to the TacticalSurface *objects*...
//	 initialOwner:  team id set on reset
//	 initiallyProtected: protected on reset?
//

function TacticalSurfaceData::create(%data)
{
	// The mission editor invokes this method when it wants to create
	// an object of the given datablock type.  For the mission editor
	%obj = new TacticalSurface() {
		dataBlock = %data;
		initialOwner = 0;
		initiallyProtected = false;
	};
	return %obj;
}

//------------------------------------------------------------------------------
// Territory Zones

function TerritorySurface_find(%name)
{
	%group = nameToID("EthPath");

	if (%group != -1)
	{
		%count = %group.getCount();
		if(%count != 0)
		{
				for (%i = 0; %i < %count; %i++)
				{
					%zone = %group.getObject(%i);
					if(%zone.getName() $= %name)
						return %zone;
				}
		}
		else
			error("TerritorySurfaces_call():" SPC
				"no TacticalSurfaces found in EthPath group!");
	}
	else
		error("TerritorySurfaces_call(): missing EthPath group!");

	return -1;
}

//------------------------------------------------------------------------------

function TerritorySurfaces_enableRepair(%shape)
{
	if(%shape.isCAT)
		return;

	if(%shape.getTeamId() == $Team1.teamId)
	{
		if(!$Team1.repairObjects.isMember(%shape))
			$Team1.repairObjects.add(%shape);
	}
	else if(%shape.getTeamId() == $Team2.teamId)
	{
		if(!$Team2.repairObjects.isMember(%shape))
			$Team2.repairObjects.add(%shape);
	}
}

function TerritorySurfaces_disableRepair(%shape)
{
	if(%shape.isCAT)
		return;

	if(%shape.getTeamId() == $Team1.teamId)
	{
		if($Team1.repairObjects.isMember(%shape))
			$Team1.repairObjects.remove(%shape);
	}
	else if(%shape.getTeamId() == $Team2.teamId)
	{
		if($Team2.repairObjects.isMember(%shape))
			$Team2.repairObjects.remove(%shape);
	}
	
	%shape.setRepairRate(0);
}

//------------------------------------------------------------------------------

function TerritorySurfaces_repairTick()
{
	cancel($TerritorySurfaces_repairTickThread);
 
    if(!isObject($Team1.repairObjects) || !isObject($Team2.repairObjects))
        return;

	%count = $Team1.repairObjects.getCount();
	if(%count != 0)
	{
		%repair = $Team1.repairSpeed / %count;
		for (%i = 0; %i < %count; %i++)
		{
			%obj = $Team1.repairObjects.getObject(%i);
			%obj.setRepairRate(%repair);
		}
	}

	%count = $Team2.repairObjects.getCount();
	if(%count != 0)
	{
		%repair = $Team2.repairSpeed / %count;
		for (%i = 0; %i < %count; %i++)
		{
			%obj = $Team2.repairObjects.getObject(%i);
			%obj.setRepairRate(%repair);
		}
	}

	$TerritorySurfaces_repairTickThread =
		schedule(100, 0, "TerritorySurfaces_repairTick");
}

//-----------------------------------------------------------------------------

// to reset all the territory zones...
function TerritorySurfaces_reset()
{
	%group = nameToID("EthPath");

	if (%group != -1)
	{
		%count = %group.getCount();
		if (%count != 0)
		{
				for (%i = 0; %i < %count; %i++)
				{
					%zone = %group.getObject(%i);
     
               if(%i == 0)
                  %zone.getDataBlock().setZoneOwner(%zone, 1);
               else if(%i == %count-1)
                  %zone.getDataBlock().setZoneOwner(%zone, 2);
               else
                  %zone.getDataBlock().setZoneOwner(%zone, 0);

               %zone.zHasNeighbour = false;
               if(%i > 0)
                  %zone.zNeighbour[0] = %group.getObject(%i-1);
               if(%i < %count-1)
                  %zone.zNeighbour[1] = %group.getObject(%i+1);

               //%zone.getDataBlock().updateOwner(%zone);
				}
				
				TerritorySurfaces_repairTick();
		}
		else
			error("TerritorySurfaces_reset():" SPC
				"no TacticalSurfaces found in EthPath group!");
	}
	else
		error("TerritorySurfaces_reset(): missing EthPath group!");
}

//-----------------------------------------------------------------------------

function TerritorySurfaces_call(%func)
{
	%group = nameToID("EthPath");

	if (%group != -1)
	{
		%count = %group.getCount();
		if (%count != 0)
		{
				for (%i = 0; %i < %count; %i++)
				{
					%zone = %group.getObject(%i);
					call(%func, %zone);
				}
		}
		else
			error("TerritorySurfaces_call():" SPC
				"no TacticalSurfaces found in EthPath group!");
	}
	else
		error("TerritorySurfaces_call(): missing EthPath group!");
}

//-----------------------------------------------------------------------------
// Territory Zone Sounds

datablock AudioProfile(ZoneAquiredSound)
{
	filename = "share/sounds/rotc/events/zone.aquired.wav";
	description = AudioCritical2D;
	preload = true;
};

datablock AudioProfile(ZoneAttackedSound)
{
	filename = "share/sounds/rotc/events/zone.attacked.wav";
	description = AudioCritical2D;
	preload = true;
};


//-----------------------------------------------------------------------------
// Territory Zone

datablock TacticalSurfaceData(TerritorySurface)
{
   allowColorization = true;

	category = "Tactical Surfaces"; // for the mission editor

	// The period is value is used to control how often the console
	// onTick callback is called while there are any objects
	// in the zone.  The default value is 100 MS.
	tickPeriodMS = 200;

	colorChangeTimeMS = 500;

	colors[0]  = "1 1 1 0.25"; // default
	colors[1]  = "1 1 1 0.9"; // flash
	colors[2]  = "1 1 1 0.1"; // blocked

    texture = "share/textures/rotc/zone.grid";
};

function TerritorySurface::onAdd(%this, %zone)
{
	%zone.teamId = 0;

	%zone.zNumReds = 0;
	%zone.zNumBlues = 0;
}

function TerritorySurface::onEnter(%this,%zone,%obj)
{
	//echo("entered territory zone");
	
	if(!%obj.getType() & $TypeMasks::ShapeBaseObjectType)
		return;

	if($Game::GameType == $Game::TeamJoust)
	{
		if(%obj.isCAT && %zone.initialOwner != 0 && %obj.getTeamId() != %zone.getTeamId())
        {
			%team = %obj.getTeamId() == 1 ? $Team1 : $Team2;
			%val = 1 - %obj.getDamagePercent();
			%val = %val / %team.numPlayersOnRoundStart;
			%team.score += %val;
			%zone.zValue -= %val;
			%zone.setColor(1, 8 + %zone.getTeamId(), %zone.zValue);
			%zone.zBlocked = true;
			%obj.kill();
		}
	}
//	else
//		%this.onTick(%zone);
	
//	%obj.getDataBlock().updateZone(%obj, %zone);
}

function TerritorySurface::onLeave(%this,%zone,%obj)
{
	// note that this function does not get called immediately when an
	// object leaves the zone but when the zone registers the object's
	// absence when the zone ticks.

	//echo("left territory zone");
	
	if(!%obj.getType() & $TypeMasks::ShapeBaseObjectType)
		return;
		
	%this.onTick(%zone);

	%obj.getDataBlock().updateZone(%obj, 0);
}

function TerritorySurface::onTick(%this, %zone)
{
	if($Game::GameType == $Game::TeamJoust)
		return;

	%zone.zNumReds = 0;
	%zone.zNumBlues = 0;
	
	for(%i = 0; %i < %zone.getNumObjects(); %i++)
	{
		%obj = %zone.getObject(%i);
  
      //error(%obj.isCAT SPC %obj.getObjectMount());
		if(!%obj.isCAT || isObject(%obj.getObjectMount()))
			continue;
	
		if(%obj.getDamageState() !$= "Enabled")
			continue;

		%start = %obj.getPosition();
		%end = VectorAdd(%start, "0 0 -1");
		%mask = $TypeMasks::TerrainObjectType | $TypeMasks::InteriorObjectType;
		%c = containerRayCast(%start, %end, %mask, %obj);

		if(!%c)
			continue;
	
		if(%zone.zProtected && %obj.getTeamId() != %zone.getTeamId())
			%obj.kill();
		else if(%obj.getTeamId() == $Team1.teamId)
			%zone.zNumReds++;
		else if(%obj.getTeamId() == $Team2.teamId)
			%zone.zNumBlues++;
	}

	%this.updateOwner(%zone);
}

function TerritorySurface::updateOwner(%this, %zone)
{
   %oldTeamId = %zone.getTeamId();
	%numConnections = 0;
	%connectedToRed = false;
	%connectedToBlue = false;

	for(%i = 0; %i < 2; %i++)
	{
		%z = %zone.zNeighbour[%i];
		if(isObject(%z))
		{
			%numConnections++;
			if(%z.getTeamId() == 1)
				%connectedToRed = true;
			if(%z.getTeamId() == 2)
				%connectedToBlue = true;
		}
	}
 
	if(%zone.zNumReds > 0 && %zone.zNumBlues == 0
	&& %connectedToRed)
	{
		%this.setZoneOwner(%zone, 1);
	}
	else if(%zone.zNumBlues > 0 && %zone.zNumReds == 0
	&& %connectedToBlue)
	{
		%this.setZoneOwner(%zone, 2);
	}
	else if(%zone.zNumReds > 0 && %zone.zNumBlues > 0
	&& %connectedToRed && %connectedToBlue)
	{
		%this.setZoneOwner(%zone, 0);		
	}
	else if(%numConnections == 1)
	{
		if(%zone.getTeamId() != 1 && %connectedToRed) // blue end zone
		{
			if(%zone.zNumReds > 0)
				%this.setZoneOwner(%zone, 0);
			else if(%zone.zNumBlues > 0)
				%this.setZoneOwner(%zone, 2);
		}
		else if(%zone.getTeamId() != 2 && %connectedToBlue) // red end zone
		{
			if(%zone.zNumBlues > 0)
				%this.setZoneOwner(%zone, 0);
			else if(%zone.zNumReds > 0)
				%this.setZoneOwner(%zone, 1);
		}			
	}
 
	%blocked = false;
	if(%zone.getTeamId() == 2 && %zone.zNumReds != 0)
		%blocked = true;
	else if(%zone.getTeamId() == 1 && %zone.zNumBlues != 0)
		%blocked = true;

 	%color = 0;
   if(%blocked)
   	%color = 2;
    
	%zone.setColor(%color, %color, 1);

	if(%zone.getTeamId() != %oldTeamId || %blocked != %zone.zBlocked)
      %zone.flash(1, 1, 1);
         
	%zone.zBlocked = %blocked;
}

function TerritorySurface::setZoneOwner(%this, %zone, %teamId)
{
	%oldTeamId = %zone.getTeamId();
 
   //echo("TerritorySurface::setZoneOwner()" SPC %oldTeamId SPC "->" SPC %teamId);
	
	if(%teamId == %oldTeamId)
		return;
		
	if(%oldTeamId == 1)
		$Team1.numTerritorySurfaces--;
	else if(%oldTeamId == 2)
		$Team2.numTerritorySurfaces--;
		
	%zone.setTeamId(%teamId);
	
	if(%teamId == 1)
		$Team1.numTerritorySurfaces++;
	else if(%teamId == 2)
		$Team2.numTerritorySurfaces++;

	for(%idx = 0; %idx < ClientGroup.getCount(); %idx++)
	{
		%client = ClientGroup.getObject(%idx);
	
		%sound = 0;

		if(%client.team == $Team1)
		{
			if(%teamId == 1)
				%sound = ZoneAquiredSound;
			else if(%teamId == 2)
				%sound = ZoneAttackedSound;
		}
		else if(%client.team == $Team2)
		{
			if(%teamId == 2)
				%sound = ZoneAquiredSound;
			else if(%teamId == 1)
				%sound = ZoneAttackedSound;
		}

		if(isObject(%sound))
			%client.play2D(%sound);
	}
	
	for(%i = 0; %i < %zone.getNumObjects(); %i++)
	{
		%obj = %zone.getObject(%i);
		if(%obj.getType() & $TypeMasks::ShapeBaseObjectType)
			%obj.getDataBlock().updateZone(%obj, 0);
	}
		
	echo("Number of zones:" SPC
		$Team1.numTerritorySurfaces SPC "red /" SPC
		$Team2.numTerritorySurfaces SPC "blue");
		
	checkRoundEnd();
}

