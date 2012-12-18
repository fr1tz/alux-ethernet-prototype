//------------------------------------------------------------------------------
// Revenge Of The Cats: Ethernet
// Copyright (C) 2008, mEthLab Interactive
//------------------------------------------------------------------------------

//*************************************************
// Mission init script for Alux : Ethernet missions
//*************************************************

exec("./alux-types.cs");

$Game::GameType = $Game::Ethernet;
$Game::GameTypeString = "Alux_Ethernet";

$Server::MissionDirectory = strreplace($Server::MissionFile, ".mis", "") @ "/";
$Server::MissionEnvironmentFile = $Server::MissionDirectory @ "mission.env";

function executeMissionScript()
{
	exec($Server::MissionDirectory @ "mission.cs"); 
}

function executeEnvironmentScript()
{
	exec($Server::MissionEnvironmentFile @ ".cs"); 
}

function executeGameScripts()
{
	exec("game/server/base/exec.cs");
	exec("game/server/cats/soldiercat.cs");
	exec("game/server/eth/exec.cs");
	exec("game/server/weapons/weapons.cs");
	exec("game/server/weapons/styck/styck.cs");
	exec("game/server/weapons/raptor/exec.cs");
	exec("game/server/weapons/laserhawk/exec.cs");
	exec("game/server/weapons/minihawk/exec.cs");
	exec("game/server/weapons/badger/exec.cs");
	exec("game/server/blueprints/exec.cs");	
}

function loadManual()
{
	constructManual("game/server/eth/help/index.idx");
}

function loadHints()
{
   constructHints("game/server/eth/help/hints.rml");
}

function initMission()
{
	if(isObject($Server::Game))
		$Server::Game.delete();
	$Server::Game = new ScriptObject();
	$Server::Game.slowpokemod = 1.0;
	for(%i = 0; %i < getWordCount($Pref::Server::Mutators); %i++)
	{
		%mutator = getWord($Pref::Server::Mutators, %i);
		if(%mutator $= "slowpoke")
		{
			$Server::Game.slowpoke = true;
			$Server::Game.slowpokemod = 0.5;
			$Server::Game.mutators = true;
		}
		else if(%mutator $= "superblaster")
		{
			$Server::Game.superblaster = true;
			$Server::Game.mutators = true;
		}
	}
	if($Server::Game.mutators)
		$Server::MissionType = $Server::MissionType SPC "\c4*MOD*\co";
	executeGameScripts();
	executeMissionScript();
	executeEnvironmentScript();
	loadManual();
	loadHints();
}

function onMissionLoaded()
{
	// Called by loadMission() once the mission is finished loading.
	startGame();
}

function onMissionEnded()
{
	// Called by endMission(), right before the mission is destroyed

	if(isObject($Server::Game))
		$Server::Game.delete();

	// Normally the game should be ended first before the next
	// mission is loaded, this is here in case loadMission has been
	// called directly.  The mission will be ended if the server
	// is destroyed, so we only need to cleanup here.
	cancel($Game::Schedule);
	$Game::Running = false;
	$Game::Cycling = false;
}



		
