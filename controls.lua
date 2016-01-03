local map = require("map")
local networking = require("networking")

function check_keyboard(dTime)
	--Checks the default movement keys here
	up = love.keyboard.isDown("up")
	down = love.keyboard.isDown("down")
	left = love.keyboard.isDown("left")
	right = love.keyboard.isDown("right")
	
	--Passes is the argument and distance player is checking to move, checks for collision, and moves if possible
	if up then 
		checkCollision("y", -1*player.speed*dTime)
	end
	
	if down then 
		checkCollision("y", player.speed*dTime)
	end
	
	if left then 
		checkCollision("x", -1*player.speed*dTime)
	end
	
	if right then 
		checkCollision("x", player.speed*dTime)
	end
	
	--Below will be other menu options
end

function boundingBox(x1,y1,w1,h1, x2,y2,w2,h2)
  ---Simple function to check if there's a collision between bounding boxes
  --DEV NOTE: Optimize later
  return x1 < x2+w2 and
         x2 < x1+w1 and
         y1 < y2+h2 and
         y2 < y1+h1
end

function checkCollision(axis, distance)
	--Checks the player's bounding box size and checks against all collision areas to ensure movement is possible
	if axis=="x" then
		if #collisionMap > 0 then
			for i=1, #collisionMap do
				--Checks if player will pass in with all collisions in the array, if not, allows movement
				if boundingBox(player.posX+distance, player.posY, 6.4*player.scale, 6.4*player.scale, collisionMap[i][1], collisionMap[i][2], 6.4*player.scale, 6.4*player.scale) then
					print("Collision detected at "..player.posX..","..player.posY..", not moving!")
					return
				end
			end
			player.posX = player.posX+distance
			server:send(string.format("100 %d %d\n", player.posX, player.posY))
		end
	elseif axis =="y" then
		if #collisionMap > 0 then
			for i=1, #collisionMap do
				--Checks if player will pass in with all collisions in the array, if not, allows movement
				if boundingBox(player.posX, player.posY+distance, 6.4*player.scale, 6.4*player.scale, collisionMap[i][1], collisionMap[i][2], 6.4*player.scale, 6.4*player.scale) then
					print("Collision detected at "..player.posX..","..player.posY..", not moving!")
					return
				end
			end
			player.posY = player.posY+distance
			server:send(string.format("100 %d %d\n", player.posX, player.posY))
		end
	else
		error("Pass in an axis. (x/y), you passed in: "..axis)
	end
end