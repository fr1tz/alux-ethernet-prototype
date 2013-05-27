//------------------------------------------------------------------------------
// Alux Prototype
// Copyright notices are in the file named COPYING.
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
	exec("game/server/eth/exec.cs");
	exec("game/server/weapons/weapons.cs");
	exec("game/server/weapons/styck/v3/exec.cs");
	exec("game/server/weapons/raptor/exec.cs");
	exec("game/server/weapons/laserhawk/v1/exec.cs");
	exec("game/server/weapons/minihawk/v2/exec.cs");
	exec("game/server/weapons/badger/v1/exec.cs");
 	exec("game/server/weapons/scorpion/v1/exec.cs");
	exec("game/server/forms/light/exec.cs");
	exec("game/server/forms/crate/exec.cs");
	exec("game/server/forms/parrot/exec.cs");
	exec("game/server/forms/bumblebee/exec.cs");
	exec("game/server/forms/soldier/exec.cs");
	exec("game/server/forms/pillar/exec.cs");
	exec("game/server/forms/soldierpod/exec.cs");
	exec("game/server/forms/hoverpod/exec.cs");
	exec("game/server/forms/hovercycle/v1/exec.cs");
   $Server::Game.form[0] = FrmLight;
   $Server::Game.form[1] = FrmParrot;
   $Server::Game.form[2] = FrmBumblebeeStatic;
   $Server::Game.form[3] = FrmCrate;
   $Server::Game.form[4] = FrmSoldier;
   $Server::Game.form[5] = FrmPillar;
   $Server::Game.form[6] = FrmHoverpod;
   $Server::Game.form[7] = FrmHovercycle;
   $Server::Game.formCount = 7;
   $Server::Game.weapon[1] = WpnBadgerImage;
   $Server::Game.weapon[2] = WpnStyckImage;
   $Server::Game.weapon[3] = WpnLaserhawkImage;
   $Server::Game.weapon[4] = WpnMinihawkImage;
   $Server::Game.weapon[5] = WpnRaptorImage;
   $Server::Game.weapon[6] = WpnScorpionImage;
   $Server::Game.weaponCount = 6;
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



		
