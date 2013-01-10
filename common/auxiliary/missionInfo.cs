//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Server side of the scripted mission info system.
//------------------------------------------------------------------------------

function MissionInfo::load(%missionFile)
{
	$MissionInfo::File = %missionFile;

	$MissionInfo::Name        = "";
	$MissionInfo::Desc        = "";
	$MissionInfo::Type        = "";
	$MissionInfo::TypeDesc    = "";
	$MissionInfo::MutatorDesc = "";
	$MissionInfo::InitScript  = "";

	exec(%missionFile);
}

