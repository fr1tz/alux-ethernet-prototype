//------------------------------------------------------------------------------
// Alux Ethernet Prototype
// Copyright notices are in the file named COPYING.
//------------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

function om_init()
{
	return "<font:Arial:16><linkcolor:FFFFFF><linkcolorhl:FF0000>";
}

function om_head(%client, %title, %prev, %refresh)
{
	%r = "";

	if(%prev !$= "")
	{
		%r = %r @ "\<\< <a:cmd" SPC %prev @ ">Back</a>\n\n";
	}

	if(%title !$= "")
	{
		%r = %r @
			"<spush><font:Arial:24>" @ %title;

		if(%refresh !$= "")
		{
			%r = %r SPC
				"[ <a:cmd" SPC %refresh @ ">Refresh</a> ]" @
			"";
		}

		%r = %r @
			"<spop>\n";

      %r = %r @ "<just:center><spush><color:FFFF00>";
      %r = %r @ "Please remember: This is merely a prototype for a potential";
      %r = %r @ " future game, as such it's crude and minimalist!\n<sbreak>";
		%r = %r @ "<bitmap:share/ui/rotc/construction><sbreak>";
      %r = %r @ "<spop><just:left>";
	}

	return %r;
}
