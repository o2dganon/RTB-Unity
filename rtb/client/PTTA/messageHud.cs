//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//----------------------------------------------------------------------------
// Enter Chat Message Hud
//----------------------------------------------------------------------------

//------------------------------------------------------------------------------

function MessageHud::open(%this)
{
   %offset = 6;

   if(%this.isVisible())
      return;

   if(%this.isTeamMsg)
      %text = "TEAM:";
   else if(%this.isLocalMsg)
      %text = "LOCAL:";
   else
      %text = "GLOBAL:";

   MessageHud_Text.setValue(%text);
   
   %windowPos = "8 " @ ( getWord( outerChatHud.position, 1 ) + getWord( outerChatHud.extent, 1 ) + 1 );
   %windowExt = getWord( OuterChatHud.extent, 0 ) @ " " @ getWord( MessageHud_Frame.extent, 1 );
   
   %textExtent = getWord(MessageHud_Text.extent, 0);
   %ctrlExtent = getWord(MessageHud_Frame.extent, 0);

   Canvas.pushDialog(%this);
   
   messageHud_Frame.position = %windowPos;
   messageHud_Frame.extent = %windowExt;
   MessageHud_Edit.position = setWord(MessageHud_Edit.position, 0, %textExtent + %offset);
   MessageHud_Edit.extent = setWord(MessageHud_Edit.extent, 0, %ctrlExtent - %textExtent - (2 * %offset));
   
   %this.setVisible(true);
   deactivateKeyboard();
   MessageHud_Edit.makeFirstResponder(true);
   commandToServer('isTalking');
}

//------------------------------------------------------------------------------

function MessageHud::close(%this)
{
   if(!%this.isVisible())
      return;

   commandToServer('stopTalking');
      
   Canvas.popDialog(%this);
   %this.setVisible(false);
   if ( $enableDirectInput )
      activateKeyboard();
   MessageHud_Edit.setValue("");
}


//------------------------------------------------------------------------------

function MessageHud::toggleState(%this)
{
   if(%this.isVisible())
      %this.close();
   else
      %this.open();
}

//------------------------------------------------------------------------------

function MessageHud_Edit::onEscape(%this)
{
   MessageHud.close();
}

//------------------------------------------------------------------------------
function MessageHud_Edit::eval(%this)
{
   %text = trim(%this.getValue());

if(%text !$= "")
{

if ($Preff::x::XChatRandom)
{
xrandomchatcolor();
}
else
{
xchatcolor();
}

if($Preff::x::tags)
{
%text2 = $Preff::x::tagval1 @ $Pref::player::chatcolor @ %text @ $Preff::x::tagval2;
}
else
{
%text2 = %text;
}
if($Preff::x::XChatReversed)
{
xreversed(%text);
%text2=$xreversed;
}

     if(MessageHud.isTeamMsg)

         commandToServer('teamMessageSent', %text2);
         //return;
else if(MessageHud.isLocalMsg)
         commandToServer('localMessageSent', %text2);
         //return;
else if($Preff::x::tags)
    if(getSubStr(%text, 0, 1) $= "/" && $Preff::x::commands)
         slashcommandcheck(%text);
else
    commandToServer('messageSent',%text2);

else if($Preff::x::commands)
         slashcommandcheck(%text);

else if(getSubStr(%text, 0, 1) $= "/") 
            commandtoserver('MessageSent', %text2); 

else 
commandtoserver('MessageSent', %text2); 

}

MessageHud.close();
}
   
//----------------------------------------------------------------------------
// MessageHud key handlers
function toggleMessageHud(%make)
{
   if(%make)
   {
      MessageHud.isTeamMsg = false;
      MessageHud.isLocalMsg = false;
      MessageHud.toggleState();
   }
}

function teamMessageHud(%make)
{
   if(%make)
   {
      MessageHud.isTeamMsg = true;
      MessageHud.isLocalMsg = false;
      MessageHud.toggleState();
   }
}

function localMessageHud(%make)
{
   if(%make)
   {
      MessageHud.isTeamMsg = false;
      MessageHud.isLocalMsg = true;
      MessageHud.toggleState();
   }
}