//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Torque Engine
// Copyright (c) GarageGames.Com
//-----------------------------------------------------------------------------
loadDir("common");

package Creator
{
	function onStart()
	{
		Parent::onStart();
		echo( "\n--------- Initializing: Torque Creator ---------" );
		
		// Scripts
		exec("./editor/editor.cs");
		exec("./editor/particleEditor.cs");
		exec("./scripts/scriptDoc.cs");
		
		// Gui's
		exec("./ui/creatorProfiles.cs");
		exec("./ui/InspectDlg.gui");
		exec("./ui/GuiEditorGui.gui");
	  
	  exec("./ui/lightEditor.gui");
	  exec("./ui/lightEditorNewDB.gui");
	}
};

activatePackage( Creator );
