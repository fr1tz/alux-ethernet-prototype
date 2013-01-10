//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

function BCONS_startNewRound()
{
	if(isObject($Round))
	{
		$Round.beacons.delete();
		$Round.delete();
	}

	$Round = new ScriptObject();
	MissionCleanup.add($Round);
	$Round.beacons = new SimSet();
	MissionCleanup.add($Round.beacons);

	for(%i = 0; %i < MissionEnvironment.getCount(); %i++ )
	{
		%obj = MissionEnvironment.getObject(%i);
		if(%obj.isMethod("getDataBlock"))
			if(%obj.getDataBlock().getName() $= "GridBeacon")
				$Round.beacons.add(%obj);
	}

	GameGrid_reset();
	$Round.grid.state = "";
	GameGrid_updateGridThread();
}

