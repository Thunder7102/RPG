local map = require("map")
local networking = require("networking")
local user = require("player")
local chat = require("chat")

local lastDirection = {
	dirX = 0,
	dirY = 0,
}

key_disable = { 
   --A quick list of keys to ignore when pulling input
   "up","down","left","right","home","end","pageup","pagedown", 
   "insert","tab","clear","delete",
   "f1","f2","f3","f4","f5","f6","f7","f8","f9","f10","f11","f12","f13","f14","f15", 
   "numlock","scrollock","ralt","lalt","rmeta","lmeta","lsuper","rsuper","mode","compose", "lshift", "rshift", "lctrl", "rctrl", "capslock",
   "pause","escape","help","print","sysreq","break","menu","power","euro","undo", "return" 
}

isShift = false
function keypress(key)
	--For non-continuous keypress handling
	
	if key == "return" then
		--Checks player's state to ensure player is neutral (and not doing anything) before switching to chat.
		--If they do switch to chat, locks them out of all keypresses until they move forward
		if player.state == "neutral" then
			player.state = "chat"
		elseif player.state == "chat" then
			player.state = "neutral"
			if string.len(messageBox)> 0 then 
				--printChat(player.name or "User", messageBox)
				server:send("200 " .. messageBox.."\n")
				messageBox = ""
			end
		end
	end
	
	
	if player.state == "chat" then
		--This will be where I capture input
		
		for i=1, #key_disable do
			--Checks key isn't in the ignore table
			if key==key_disable[i] then
				return
			end
		end
		
		if key == "space" then
			key = " "
		elseif key == "backspace" then
			messageBox = string.sub(messageBox, 1, string.len(messageBox) -1)
			key = ""
		end
		
		if love.keyboard.isDown("lshift") or love.keyboard.isDown("rshift") then
			--Allows caps
			key = string.upper(key)
		end
		
		--This will add the character to the table
		if string.len(messageBox) < 40 then
			--Just checks we don't go over maximum message amount
			messageBox = messageBox..key
		end
	end

	
end

function check_keyboard(dTime)
	--This checks for keys which you want to see if they are held down or to perform an operation constantly
	if player.state ~= "chat" then
		--I want to lock the player out of movement if they are chatting
		up = love.keyboard.isDown("w")
		down = love.keyboard.isDown("s")
		left = love.keyboard.isDown("a")
		right = love.keyboard.isDown("d") 
		
		--mousekeys
		left_mouse = love.mouse.isDown(1)
	end
	--other
	
	
	direction = { dirX = 0, dirY = 0 }
	
	--Passes is the argument and distance player is checking to move, checks for collision, and moves if possible
	if up then 
		checkCollision("y", -1*player.speed, dTime, direction)
	end
	
	if down then 
		checkCollision("y", player.speed, dTime, direction)
	end
	
	if left then 
		checkCollision("x", -1*player.speed, dTime, direction)
	end
	
	if right then 
		checkCollision("x", player.speed, dTime, direction)
	end
	
	if lastDirection.dirX ~= direction.dirX or lastDirection.dirY ~= direction.dirY then
		local strToSend = string.format("100 %f %f %f %f\n", player.posX, player.posY, direction.dirX, direction.dirY)
		-- print(strToSend)
		server:send(strToSend)
		lastDirection = direction
	end
	
	if direction.dirX ~= 0 or direction.dirY ~= 0 then
		player.posX = player.posX + direction.dirX * dTime
		player.posY = player.posY + direction.dirY * dTime
	end
	
	--Here is attacking
	if left_mouse then
		player.state = "attacking"
	end
	
	
end

function boundingBox(x1,y1,w1,h1, x2,y2,w2,h2)
  ---Simple function to check if there's a collision between bounding boxes
  --DEV NOTE: Optimize later
  return x1 < x2+w2 and
         x2 < x1+w1 and
         y1 < y2+h2 and
         y2 < y1+h1
end

function checkCollision(axis, distance, dTime, direction)
	--Checks the player's bounding box size and checks against all collision areas to ensure movement is possible
	if axis=="x" then
		if #collisionMap > 0 then
			for i=1, #collisionMap do
				--Checks if player will pass in with all collisions in the array, if not, allows movement
				if boundingBox(player.posX+distance*dTime, player.posY, 6.4*player.scale, 6.4*player.scale, collisionMap[i][1], collisionMap[i][2], 6.4*player.scale, 6.4*player.scale) then
					-- print("Collision detected at "..player.posX..","..player.posY..", not moving!")
					return
				end
			end
			direction.dirX = direction.dirX + distance
		end
	elseif axis =="y" then
		if #collisionMap > 0 then
			for i=1, #collisionMap do
				--Checks if player will pass in with all collisions in the array, if not, allows movement
				if boundingBox(player.posX, player.posY+distance*dTime, 6.4*player.scale, 6.4*player.scale, collisionMap[i][1], collisionMap[i][2], 6.4*player.scale, 6.4*player.scale) then
					-- print("Collision detected at "..player.posX..","..player.posY..", not moving!")
					return
				end
			end
			direction.dirY = direction.dirY + distance
		end
	else
		error("Pass in an axis. (x/y), you passed in: "..axis)
	end
end