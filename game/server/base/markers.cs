//------------------------------------------------------------------------------
// Alux
// Copyright (C) 2013 Michael Goldener <mg@wasted.ch>
//------------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

datablock MissionMarkerData(SpawnSphereMarker)
{
	category = "Misc";
	shapeFile = "share/shapes/rotc/markers/octahedron.dts";
};

function SpawnSphereMarker::create(%data)
{
	// The mission editor invokes this method when it wants to create
	// an object of the given datablock type. For the mission editor
	%obj = new SpawnSphere() {
		dataBlock = %data;
	};

	return %obj;
}

//------------------------------------------------------------------------------
// - serveral marker types may share MissionMarker datablock type
function MissionMarkerData::create(%block)
{
	return %block.create();
}
