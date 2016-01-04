--[[
	Separate player file for handling spells, inventory, stats, etc
]]
local entity = require("entity")
player = createEntity()
player.level = 1
player.scale = 10
playerState = "neutral"
print("Player speed is: "..player.speed)


function player.attack()
	--Attacks in the direction the player is facing (north for now)
	player.state = "attacking"
end

function boundingBox(x1,y1,w1,h1, x2,y2,w2,h2)
  ---Simple function to check if there's a collision between bounding boxes
  --DEV NOTE: Optimize later
  return x1 < x2+w2 and
			 x2 < x1+w1 and
			 y1 < y2+h2 and
			 y2 < y1+h1
end