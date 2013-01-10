//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Client side of the scripted material mapping system.
//------------------------------------------------------------------------------

function clientCmdMaterialMapping(%material, %sound, %color, %detail, %envmap)
{
	addMaterialMapping(
		%material,
		%sound  $= "" ? "" : "sound:" SPC %sound,
		%color  $= "" ? "" : "color:" SPC %color,
		%detail $= "" ? "" : "detail:" SPC %detail,
		%envmap $= "" ? "" : "environment:" SPC %envmap
	);
}
