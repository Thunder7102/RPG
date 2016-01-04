--[[
	This will handle collision boxes and graphics for the client. 
]]

--This holds all collision points
collisionMap = {}
mapSize = {}

function newCollisionBox(...)
	--Creates a 64x64 pixel-sized block at the location
	local args = {...}
	if not #args == 2 then error("Pass in: (posX, posY), passed in: "..#args.." arguments.") end
	local o = {}
	local posX = args[1]
	local posY = args[2]
	
	collisionMap[#collisionMap+1] = {[1] = posX, [2] = posY}
	return #collisionMap
end

newCollisionBox(50, 300)
newCollisionBox(114, 300)
newCollisionBox(500, 100)
newCollisionBox(500, 800)
newCollisionBox(200, 500)

