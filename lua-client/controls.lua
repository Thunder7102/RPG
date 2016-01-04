local map = require("map")
local networking = require("networking")
local user = require("player")

local lastDirection = {
	dirX = 0,
	dirY = 0,
}

function keypress(key)
	--For non-continuous keypress handling
	if key == "return" then
		if player.state == "chatstart" then
			player.state = "chat"
		else
			player.state = "chatstart"
		end
	end
	
end

function check_keyboard(dTime)
	--This checks for keys which you want to see if they are held down or to perform an operation constantly
	up = love.keyboard.isDown("up")
	down = love.keyboard.isDown("down")
	left = love.keyboard.isDown("left")
	right = love.keyboard.isDown("right")
	--mousekeys
	left_mouse = love.mouse.isDown(1)
	
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
		player.attack()
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