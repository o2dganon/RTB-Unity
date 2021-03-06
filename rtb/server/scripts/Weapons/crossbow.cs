//crossbow.cs
//crossbow and arrow weapon stuff

datablock AudioProfile(crossbowFireSound)
{
   filename    = "~/data/sound/crossbowFire.wav";
   description = AudioClosest3d;
   preload = true;
};


//arrow trail
datablock ParticleData(arrowTrailParticle)
{
	dragCoefficient		= 3.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 900;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= false;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "~/data/particles/dot";
	//animTexName		= "~/data/particles/dot";

	// Interpolation variables
	colors[0]	= "1 1 1 0.5";
	colors[1]	= "1 1 1 0.0";
	sizes[0]	= 0.2;
	sizes[1]	= 0.01;
	times[0]	= 0.0;
	times[1]	= 1.0;
};

datablock ParticleEmitterData(arrowTrailEmitter)
{
   ejectionPeriodMS = 7;
   periodVarianceMS = 0;

   ejectionVelocity = 0; //0.25;
   velocityVariance = 0; //0.10;

   ejectionOffset = 0;

   thetaMin         = 0.0;
   thetaMax         = 90.0;  

   particles = arrowTrailParticle;
};

//effects
datablock ParticleData(arrowExplosionParticle)
{
	dragCoefficient      = 5;
	gravityCoefficient   = 0.1;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 500;
	lifetimeVarianceMS   = 300;
	textureName          = "~/data/particles/chunk";
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]     = "0.9 0.9 0.6 0.9";
	colors[1]     = "0.9 0.5 0.6 0.0";
	sizes[0]      = 0.25;
	sizes[1]      = 0.0;
};

datablock ParticleEmitterData(arrowExplosionEmitter)
{
   ejectionPeriodMS = 7;
   periodVarianceMS = 0;
   ejectionVelocity = 5;
   velocityVariance = 1.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "arrowExplosionParticle";
};

datablock ExplosionData(arrowExplosion)
{
   //explosionShape = "";
	soundProfile = arrowExplosionSound;

   lifeTimeMS = 150;

   particleEmitter = arrowExplosionEmitter;
   particleDensity = 10;
   particleRadius = 0.2;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = false;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "1.0 1.0 1.0";
   camShakeDuration = 0.5;
   camShakeRadius = 10.0;

   // Dynamic light
   lightStartRadius = 0;
   lightEndRadius = 2;
   lightStartColor = "0.3 0.6 0.7";
   lightEndColor = "0 0 0";
};


//projectile
datablock ProjectileData(crossbowarrowprojectile)
{
   projectileShapeName = "~/data/shapes/arrow.dts";
   directDamage        = 10;
   radiusDamage        = 10;
   damageRadius        = 0.5;
   explosion           = arrowExplosion;
   particleEmitter     = arrowTrailEmitter;

   muzzleVelocity      = 99;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 9000;
   fadeDelay           = 8000;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = true;
   gravityMod = 0.10;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 1";
};


//////////
// item //
//////////
datablock ItemData(crossbow)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "~/data/shapes/crossbow.dts";
	skinName = 'base';
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	cost = 200;

	 // Dynamic properties defined by the scripts
	pickUpName = 'a crossbow and arrows.';
	invName = 'crossbow';
	image = crossbowImage;
};


function crossbow::onUse(%this, %player, %InvPosition)
{
	//check for quiver 
	//if you dont have it, regular crossbow
	//if you do, super crossbow

	%client = %player.client;

	%mountPoint = %this.image.mountPoint;
	%mountedImage = %player.getMountedImage(%mountPoint); 

	%skin = %this.skinname;

	if(%mountedImage)
	{
		if(%mountedImage == crossbowImage.getId() || %mountedImage == supercrossbowImage.getId())
		{
			//some kind of crossbow mounted so, unmount it
			%player.unMountImage(%mountPoint);
			messageClient(%client, 'MsgHilightInv', '', -1);
			%player.currWeaponSlot = -1;
		}
		else
		{
			//something other than crossbow mounted, so do crossbow selection and mount
			if(%player.getMountedImage($BackSlot))
			{
				if(%player.getMountedImage($BackSlot) == quiverImage.getId())
				{
					%player.mountimage(supercrossbowImage, $RightHandSlot, 1, %skin);
					messageClient(%client, 'MsgHilightInv', '', %InvPosition);
					%player.currWeaponSlot = %invPosition;
				}
				else
				{
					%player.mountimage(crossbowImage, $RightHandSlot, 1, %skin);
					messageClient(%client, 'MsgHilightInv', '', %InvPosition);
					%player.currWeaponSlot = %invPosition;
				}
			}
			else
			{
				%player.mountimage(crossbowImage, $RightHandSlot, 1, %skin);
				messageClient(%client, 'MsgHilightInv', '', %InvPosition);
				%player.currWeaponSlot = %invPosition;
			}
		}
		
	}
	else
	{
		//nothing mounted so do crossbow selection and mount
		//something other than crossbow mounted, so do crossbow selection and mount
		if(%player.getMountedImage($BackSlot))
		{
			if(%player.getMountedImage($BackSlot) == quiverImage.getId())
			{
				%player.mountimage(supercrossbowImage, $RightHandSlot, 1, %skin);
				messageClient(%client, 'MsgHilightInv', '', %InvPosition);
				%player.currWeaponSlot = %invPosition;
			}
			else
			{
				%player.mountimage(crossbowImage, $RightHandSlot, 1, %skin);
				messageClient(%client, 'MsgHilightInv', '', %InvPosition);
				%player.currWeaponSlot = %invPosition;
			}
		}
		else
		{
			%player.mountimage(crossbowImage, $RightHandSlot, 1, %skin);
			messageClient(%client, 'MsgHilightInv', '', %InvPosition);
			%player.currWeaponSlot = %invPosition;
		}
	}
}

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(crossbowImage)
{
   // Basic Item properties
   shapeFile = "~/data/shapes/crossbow.dts";
   PreviewFileName = "rtb/data/shapes/bricks/Previews/crossbow.png";
   skinName = 'base';
   emap = true;
	cloakable = false;
   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
   //eyeOffset = "0.1 0.2 -0.55";

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = crossbow;
   ammo = " ";
   projectile = crossbowarrowprojectile;
   projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   //casing = " ";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.5;
	stateTransitionOnTimeout[0]       = "Ready";
	stateSound[0]					= weaponSwitchSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "Fire";
	stateAllowImageChange[1]         = true;

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "Reload";
	stateTimeoutValue[2]            = 0.05;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = false;
	stateSequence[2]                = "Fire";
	stateScript[2]                  = "onFire";
	stateWaitForTimeout[2]			= true;
	stateSound[2]					= crossbowFireSound;

	stateName[3]			= "Reload";
	stateSequence[3]                = "Reload";
	stateAllowImageChange[3]        = false;
	stateTimeoutValue[3]            = 0.5;
	stateWaitForTimeout[3]		= true;
	stateTransitionOnTimeout[3]     = "Check";

	stateName[4]			= "Check";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4]	= "Fire";

	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Ready";
	stateTimeoutValue[5]            = 0.2;
	stateAllowImageChange[5]        = false;
	stateWaitForTimeout[5]		= true;
	//stateSequence[5]                = "Reload";
	stateScript[5]                  = "onStopFire";


};

//Super crossbow- you get it from using quiver with crossbow
datablock ShapeBaseImageData(supercrossbowImage)
{
   // Basic Item properties
   shapeFile = "~/data/shapes/crossbow.dts";
   skinName = 'base';
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
   //eyeOffset = "0.1 0.2 -0.55";

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = crossbow;
   ammo = " ";
   projectile = crossbowarrowprojectile;
   projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   //casing = " ";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]                    = "Activate";
	stateTimeoutValue[0]            = 0.5;
	stateTransitionOnTimeout[0]     = "Ready";
	stateSound[0]					= weaponSwitchSound;
	stateAllowImageChange[0]		= true;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "Fire";
	stateAllowImageChange[1]         = true;

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "Pause1";
	stateTimeoutValue[2]            = 0.05;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = true;
	stateSequence[2]                = "Fire";
	stateScript[2]                  = "onFire";
	stateWaitForTimeout[2]			= true;
	stateSound[2]					= crossbowFireSound;

	stateName[8]					= "Pause1";
	stateTransitionOnTimeout[8]     = "Fire2";
	stateTimeoutValue[8]            = 0.25;
	stateWaitForTimeout[8]			= true;
	stateSequence[8]                = "Reload";
	stateTransitionOnTriggerUp[8]	= "Reload";
	stateAllowImageChange[8]         = true;
	
	stateName[3]                    = "Fire2";
	stateTransitionOnTimeout[3]     = "Pause2";
	stateTimeoutValue[3]            = 0.05;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = true;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]			= true;
	stateSound[3]					= crossbowFireSound;

	stateName[9]					= "Pause2";
	stateTransitionOnTimeout[9]     = "Fire3";
	stateTimeoutValue[9]            = 0.25;
	stateWaitForTimeout[9]			= true;
	stateSequence[9]                = "Reload";
	stateTransitionOnTriggerUp[9]	= "Reload";
	stateAllowImageChange[9]         = true;

	stateName[4]                    = "Fire3";
	stateTransitionOnTimeout[4]     = "Reload";
	stateTimeoutValue[4]            = 0.05;
	stateFire[4]                    = true;
	stateAllowImageChange[4]        = true;
	stateSequence[4]                = "Fire";
	stateScript[4]                  = "onFire";
	stateWaitForTimeout[4]			= true;
	stateSound[4]					= crossbowFireSound;

	stateName[5]					= "Reload";
	stateSequence[5]                = "Reload";
	stateAllowImageChange[5]        = true;
	stateTimeoutValue[5]            = 0.5;
	stateWaitForTimeout[5]			= true;
	stateTransitionOnTimeout[5]     = "Check";

	stateName[6]					= "Check";
	stateTransitionOnTriggerUp[6]	= "StopFire";
	stateTransitionOnTriggerDown[6]	= "Fire";
	stateAllowImageChange[6]         = true;
	stateScript[6]					= "onCheck";

	stateName[7]                    = "StopFire";
	stateTransitionOnTimeout[7]     = "Ready";
	stateTimeoutValue[7]            = 0.5;
	stateAllowImageChange[7]        = true;
	stateWaitForTimeout[7]			= true;
	//stateSequence[7]                = "Reload";
	stateScript[7]                  = "onStopFire";


};

function supercrossbowImage::onCheck(%this,%obj,%slot)
{
	if(%obj.getMountedImage($BackSlot) !=  quiverImage.getId())
	{
		//we've somehow cheated so switch to regular crossbow
		//we have to do it this way because the image mount/unmount que system sucks
		//so its easy to cheat 

		%obj.mountimage(crossbowImage, $rightHandSlot);
	}
}


function crossbowarrowProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal) 
{ 
	weaponDamage(%obj,%col,%this,%pos,crossbow);
	radiusDamage(%obj,%pos,%this.damageRadius,%this.radiusDamage,crossbow,%this.areaImpulse);
}